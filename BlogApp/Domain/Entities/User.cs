namespace BlogApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = "Reader"; // e.g., Admin, Editor, Reader
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }


    }
}