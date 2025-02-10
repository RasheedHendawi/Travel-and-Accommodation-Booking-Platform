

namespace Domain.Models
{
    public record AttachmentInvoice(
      string Name,
      byte[] File,
      string MediaType,
      string SubMediaType);
}
