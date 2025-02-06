using Domain.Entities;
using System.Text;

namespace Application.DTOs.InvoicePdf
{
    public static class InvoiceGenerator
    {
        public static string GetInvocieHtml(Booking booking)
        {

            var stringBuilder = new StringBuilder();

            // Invoice HTML generation starts here
            stringBuilder.Append(
                """
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Hotel Booking Invoice</title>
            <style>
                body {
                    font-family: 'Arial', sans-serif;
                    background-color: #f8f9fa;
                    color: #333;
                    margin: 0;
                    padding: 20px;
                }

                .invoice-container {
                    max-width: 800px;
                    margin: auto;
                    background: white;
                    padding: 20px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    border-radius: 8px;
                }

                .invoice-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    padding-bottom: 15px;
                    border-bottom: 2px solid #007bff;
                }

                .hotel-details, .customer-details {
                    margin: 15px 0;
                    padding: 10px;
                    background: #f0f8ff;
                    border-radius: 5px;
                }

                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 20px;
                }

                th, td {
                    border: 1px solid #ddd;
                    padding: 12px;
                    text-align: left;
                }

                th {
                    background-color: #007bff;
                    color: white;
                }

                .total-section {
                    text-align: right;
                    font-weight: bold;
                    font-size: 18px;
                    margin-top: 20px;
                }
            </style>
        </head>
        """)
              .Append(
                $"""
        <body>
            <div class="invoice-container">
                <div class="invoice-header">
                    <h1>Booking Invoice</h1>
                    <p><strong>Date:</strong> {DateTime.UtcNow:MMMM dd, yyyy}</p>
                </div>

                <div class="hotel-details">
                    <h3>Hotel Information</h3>
                    <p><strong>Hotel:</strong> {booking.Hotel.Name}</p>
                    <p><strong>Location:</strong> {booking.Hotel.City.Name}, {booking.Hotel.City.Country}</p>
                </div>

                <div class="customer-details">
                    <h3>Booking Details</h3>
                    <p><strong>Check-in:</strong> {booking.CheckIn:MMMM dd, yyyy}</p>
                    <p><strong>Check-out:</strong> {booking.CheckOut:MMMM dd, yyyy}</p>
                    <p><strong>Booking Date:</strong> {booking.BookingDate:MMMM dd, yyyy}</p>
                </div>

                <table>
                    <thead>
                        <tr>
                            <th>Room No.</th>
                            <th>Room Type</th>
                            <th>Price (per night)</th>
                            <th>Discount (%)</th>
                            <th>Final Price</th>
                        </tr>
                    </thead>
                    <tbody>
        """);

            foreach (var invoiceRecord in booking.Invoice)
            {
                decimal finalPrice = invoiceRecord.Price * (1 - (invoiceRecord.DiscountPercentage ?? 0) / 100m);

                stringBuilder.Append(
                  $"""
        <tr>
            <td>{invoiceRecord.RoomNumber}</td>
            <td>{invoiceRecord.RoomClassName}</td>
            <td>${invoiceRecord.Price:F2}</td>
            <td>{invoiceRecord.DiscountPercentage ?? 0}%</td>
            <td>${finalPrice:F2}</td>
        </tr>
        """);
            }

            stringBuilder.Append(
              $"""
            </tbody>
        </table>

        <div class="total-section">
            <p><strong>Total Amount:</strong> ${booking.TotalPrice:F2}</p>
        </div>
        </div>
        </body>
        </html>
        """);

            return stringBuilder.ToString();
        }
    }
}
