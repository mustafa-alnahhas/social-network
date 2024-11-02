namespace Social_Media.Domain
{
    public class Like
    {
        private Like(int userId, int postId)
        {
            UserId = userId;
            PostId = postId;
        }

        public static Like CreateLike(int userId, int postId)
        {
            return new Like(userId, postId);
        }
        public int LikeId { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key
        public int PostId { get; set; } // Foreign Key

        // Navigation Property for User
        public virtual User User { get; set; }

        // Navigation Property for Post
        public virtual Post Post { get; set; }
    }
}
