using Application.Contracts;
using Application.DTOs.Bookings;
using Application.DTOs.InvoicePdf;
using Application.DTOs.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;


namespace Application.Services.Bookings
{
    public class BookingService : IBookingsService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private readonly IHttpUserContextAccessor _httpUserContextAccessor;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepository,
            IHotelRepository hotelRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPdfService pdfService,
            IEmailService emailService,
            IHttpUserContextAccessor httpUserContextAccessor,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _pdfService = pdfService;
            _emailService = emailService;
            _httpUserContextAccessor = httpUserContextAccessor;
            _mapper = mapper;
        }

        public async Task<BookingResponse> CreateBookingAsync(BookingCreationRequest request)
        {

            if (!await _userRepository.ExistsByIdAsync(_httpUserContextAccessor.Id))
            {
                throw new Exception("User Not Found");
            }

            if (_httpUserContextAccessor.Role != UserRoles.Guest)
            {
                throw new Exception("Forbidden User (Not Guest)");
            }

            var hotel = await _hotelRepository.GetByIdAsync(request.HotelId, true, false, false)
                        ?? throw new Exception("Hotel Not Found");

            var rooms = await ValidateRooms(request.RoomIds, request.HotelId, request.CheckIn, request.CheckOut);

            var booking = new Booking
            {
                Hotel = hotel,
                Rooms = rooms,
                GuestId = _httpUserContextAccessor.Id,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                TotalPrice = CalculateTotalPrice(rooms, request.CheckIn, request.CheckOut),
                BookingDate = DateOnly.FromDateTime(DateTime.UtcNow),
                GuestRemarks = request.GuestRemarks,
                PaymentMethod = request.PaymentMethod
            };

            var createdBooking = await _bookingRepository.CreateAsync(booking);
            foreach (var room in rooms)
            {
                var invoiceRecord = new InvoiceRecord
                {
                    RoomId = room.Id,
                    Booking = createdBooking,
                    RoomClassName = room.RoomClass.Name,
                    RoomNumber = room.Number,
                    Price = room.RoomClass.Price,
                    DiscountPercentage = room.RoomClass.Discounts.FirstOrDefault()?.Percentage
                };

                createdBooking.Invoice.Add(invoiceRecord);
            }

            await _unitOfWork.SaveChangesAsync();

            var invoiceHtmlToPdf = await _pdfService.GeneratePdfFromHtmlAsync(
                InvoiceGenerator.GetInvocieHtml(createdBooking));
            Console.WriteLine("Invoic"+ _httpUserContextAccessor.Email);
            await _emailService.SendEmailAsync(
                EmailRequests.GetEmailRequest(_httpUserContextAccessor.Email,
                [ new AttachmentInvoice(
                        "invoice.pdf", invoiceHtmlToPdf, "application", "pdf")
                ]));


            return _mapper.Map<BookingResponse>(createdBooking);
        }

        public async Task DeleteBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId)
                          ?? throw new Exception("Booking Not Found For Guest");

            if (booking.CheckIn <= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new Exception("Cannot Cancel Booking");
            }

            await _bookingRepository.DeleteAsync(bookingId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<byte[]> GetInvoiceAsPdfAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId, _httpUserContextAccessor.Id, true)
                          ?? throw new Exception("Booking Not Found For Guest");
            return await _pdfService.GeneratePdfFromHtmlAsync(InvoiceGenerator.GetInvocieHtml(booking));
        }

        public async Task<BookingResponse> GetBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId, _httpUserContextAccessor.Id, false)
                          ?? throw new Exception("Booking Not Found For Guest");

            return _mapper.Map<BookingResponse>(booking);
        }

        public async Task<PaginatedList<BookingResponse>> GetBookingsAsync(BookingsGetRequest request)
        {
            var bookings = await _bookingRepository.GetAsync(
                new Query<Booking>(
                    b => b.GuestId == _httpUserContextAccessor.Id,
                    request.SortOrder ?? SortMethod.Ascending,
                    request.SortColumn,
                    request.PageNumber,
                    request.PageSize)
                );

            return _mapper.Map<PaginatedList<BookingResponse>>(bookings);
        }

        private async Task<List<Room>> ValidateRooms(IEnumerable<Guid> roomIds, Guid hotelId, DateOnly checkInDate, DateOnly checkOutDate)
        {
            var rooms = new List<Room>();

            foreach (var roomId in roomIds)
            {
                var room = await _roomRepository.GetByIdWithRoomClassAsync(roomId)
                            ?? throw new Exception("Room Not Found");

                if (room.RoomClass.HotelId != hotelId)
                {
                    throw new Exception("Room Not In Same Hotel");
                }
                if (!await _roomRepository.ExistsAsync(
                      r => r.Id == roomId &&
                           r.Bookings.All(
                             b => checkInDate >= b.CheckOut ||
                                  checkOutDate <= b.CheckIn)))
                {
                    throw new Exception($"Room Not Available ({roomId})");
                }
                rooms.Add(room);
            }

            return rooms;
        }

        private static decimal CalculateTotalPrice(IEnumerable<Room> rooms, DateOnly checkInDate, DateOnly checkOutDate)
        {
            var totalPricePerNight = rooms.Sum(r => r.RoomClass.Price * (100 - (r.RoomClass.Discounts.FirstOrDefault()?.Percentage ?? 0)) / 100);
            var bookingDuration = checkOutDate.DayNumber - checkInDate.DayNumber;
            return totalPricePerNight * bookingDuration;
        }
    }
}
