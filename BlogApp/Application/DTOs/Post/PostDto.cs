namespace BlogApp.Application.DTOs.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public int DaysSincePublished { get; set; }

        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }

        public string AuthorName { get; set; } = string.Empty;
    }
}