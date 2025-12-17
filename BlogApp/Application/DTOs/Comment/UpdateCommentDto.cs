namespace BlogApp.Application.DTOs.Comment
{
    public class UpdateCommentDto
    {
        public DateTime CommentDate { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? UserName { get; set; }
    }
}