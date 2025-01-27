using Domain.Interfaces.Services;
using NReco.PdfGenerator;

namespace Infrastructure.Services.PDF
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> GeneratePdfFromHtmlAsync(string html)
        {
            return await Task.Run(() =>
            {
                var htmlToPdfConverter = new HtmlToPdfConverter();

                return htmlToPdfConverter.GeneratePdf(html);
            });
        }
    }
}
