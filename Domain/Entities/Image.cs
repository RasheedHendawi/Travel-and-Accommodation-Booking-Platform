using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
