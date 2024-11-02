using Social_Media.Domain;
using System;
using System.Collections.Generic;

public class Post
{
    private Post(int userId, string content, DateTime createdAt)
    {
        UserId = userId;
        Content = content;
        CreatedAt = createdAt;
    }

    private Post(int userId, string content, DateTime createdAt, int postId)
    {
        UserId = userId;
        Content = content;
        CreatedAt = createdAt;
        PostId = postId;
    }

    public static Post CreatePost(int userId, string content, DateTime createdAt)
    {
        return new Post(userId, content, createdAt);
    }

    public static Post CreatePostForRead(int userId, string content, DateTime createdAt, int postId)
    {
        return new Post(userId, content, createdAt, postId);
    }


    public int PostId { get; set; } // Primary Key
    public int UserId { get; set; } // Foreign Key
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Property for User
    public virtual User User { get; set; }

    // Navigation Property for Related Comments
    public virtual ICollection<Comment> Comments { get; set; }

    // Navigation Property for Likes
    public virtual ICollection<Like> Likes { get; set; }

    // Navigation Property for Feeds
    public virtual ICollection<Feed> Feeds { get; set; }
}