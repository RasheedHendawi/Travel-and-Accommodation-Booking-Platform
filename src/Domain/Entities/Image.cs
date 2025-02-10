using Domain.Enums;

namespace Domain.Entities
{
    public class Image : EntityKey
    {
        public Guid EntityId { get; set; }
        public string Format { get; set; }
        public ImageType Type { get; set; }
        public string Path { get; set; }
    }
}
