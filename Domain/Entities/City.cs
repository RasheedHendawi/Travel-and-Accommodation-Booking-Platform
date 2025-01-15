using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class City
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string PostOffice { get; set; }
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}
