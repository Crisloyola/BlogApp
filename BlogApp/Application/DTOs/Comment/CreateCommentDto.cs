namespace BlogApp.Application.DTOs.Comment
{
    public class CreateCommentDto
    {

        public DateTime CommentDate { get; set; }
        public string Content { get; set; } = string.Empty;

        public string? UserName { get; set; }

        public int PostId { get; set; }
    }
}