using Domain.Models;

namespace Application.DTOs.Bookings
{
    public static class EmailRequests
    {
        public static EmailRequest GetEmailRequest(string toEmail, IEnumerable<AttachmentInvoice> attachments)
        {
            return new EmailRequest(
                [toEmail],
                "Your Hotel Booking is Confirmed! 🏨🏨",
                """
                    Dear Valued Guest,  

                    Thank you for choosing our platform to book your stay.  
                    We’re thrilled to have you and look forward to making your experience exceptional.  

                    Attached, you will find your invoice with all the details of your reservation.  
                    If you have any special requests or need assistance, feel free to contact us.  

                    Wishing you a fantastic stay!  

                    Best regards,  
                    The Booking Team  
                    """,
                attachments);
        }

    }
}
