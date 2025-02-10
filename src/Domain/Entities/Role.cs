
namespace Domain.Entities
{
    public class Role : EntityKey
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
