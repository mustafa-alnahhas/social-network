using Social_Media.Domain;

namespace Social_Media.Contracts.Posts
{
    public interface IPostRepository
    {
        Task InsertPost(Post post);
        Task LikePost(Like like);
        Task CommentPost(Comment comment);
        Task<List<Post>> GetFeed(int userId, int skip, int NumberOfPostsPerPage);
        Task<bool> PostIsExist(int postId);
    }
}
