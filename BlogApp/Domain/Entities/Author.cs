namespace BlogApp.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }

        // 🔑 RELACIÓN CON USER
        public int? UserId { get; set; }
        public User? User { get; set; } = null!;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public string GetFullName() => $"{FirstName} {LastName}";
    }
}
