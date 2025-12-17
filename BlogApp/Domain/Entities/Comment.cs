namespace BlogApp.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime CommentDate { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public int PostId { get; set; }
        public Post? Post { get; set; }


        public bool IsRecentComment() => CommentDate > DateTime.Now.AddDays(-7);

        public bool CanBeDeleted() => Status == "Pending" || Status == "Approved";

    }
}