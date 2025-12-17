using System.Xml.Linq;

namespace BlogApp.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public int GetDaysSincePublished()
        {
            var today = DateTime.Today;
            var days = (today - PublishDate).Days;
            return days;
        }


    }
}