namespace BlogApp.Application.DTOs.Post
{
    public class UpdatePostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
    }
}