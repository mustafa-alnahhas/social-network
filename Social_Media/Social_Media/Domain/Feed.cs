namespace Social_Media.Domain
{
    public class Feed
    {
        public int Id { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key
        public int PostId { get; set; } // Foreign Key

        // Navigation Property for User
        public virtual User User { get; set; }

        // Navigation Property for Post
        public virtual Post Post { get; set; }
    }
}
