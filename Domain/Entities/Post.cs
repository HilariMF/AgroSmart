using Domain.Common;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }  // Almacena el ID del usuario
        public int TopicId { get; set; }     // Relación con el tema
        public Topic Topic { get; set; }
    }
}
