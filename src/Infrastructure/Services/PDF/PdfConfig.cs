using Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.PDF
{
    public static class PdfConfig
    {
        public static IServiceCollection AddPdf(this IServiceCollection services)
        {
            services.AddScoped<IPdfService, PdfService>();
            return services;
        }
    }
}
