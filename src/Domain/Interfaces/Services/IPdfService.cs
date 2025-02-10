namespace Domain.Interfaces.Services
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfFromHtmlAsync(string html);
    }
}
