using Social_Media.Domain;
using System;

public class Comment
{
    private Comment(int postId, int userId, string content, DateTime createdAt)
    {
        PostId = postId;
        Content = content;
        CreatedAt = createdAt;
        UserId = userId;
    }

    public static Comment CreateComment( int postId, int userId,  string content, DateTime createdAt)
    {
        return new Comment( postId, userId, content, createdAt);
    }

    public int CommentId { get; set; } // Primary Key
    public int PostId { get; set; } // Foreign Key
    public int UserId { get; set; } // Foreign Key
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Property for Related Post
    public virtual Post Post { get; set; }
    public virtual User User {  get; set; }
}