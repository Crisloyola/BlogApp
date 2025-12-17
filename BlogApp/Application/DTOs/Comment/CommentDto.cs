namespace BlogApp.Application.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime CommentDate { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public bool CanBeDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
    }
}