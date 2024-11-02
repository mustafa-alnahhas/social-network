using Microsoft.Extensions.Hosting;

namespace Social_Media.Domain
{
    public class User
    {
        private User(string username, string hashedPassword, DateTime creationDate)
        {
            Username = username;
            HashedPassword = hashedPassword;
            CreationDate = creationDate;
        }

        public static User CreateUser(string username, string hashedPassword, DateTime creationDate)
        {
            return new User(username, hashedPassword, creationDate);
        }

        public void SetPassword(string pass)
        {
            this.HashedPassword = pass;
        }

        public int UserId { get; set; } // Primary Key
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public DateTime CreationDate { get; set; }

        // Navigation Property for Related Posts
        public virtual ICollection<Post> Posts { get; set; }

        // Navigation Property for Likes
        public virtual ICollection<Like> Likes { get; set; }

        // Navigation Property for Feeds
        public virtual ICollection<Feed> Feeds { get; set; }

        // Navigation Property for Comments
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
