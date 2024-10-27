using Domain.Common;

namespace Domain.Entities
{
    public class Topic : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }  // Almacena el ID del usuario
        public List<Post> Posts { get; set; }
    }
}
