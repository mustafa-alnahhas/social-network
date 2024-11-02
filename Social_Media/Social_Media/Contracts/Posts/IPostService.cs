using Social_Media.Contracts.Posts;

namespace Social_Media.Contracts.Posts
{
    public interface IPostService
    {
        Task CreatePost(CreatePostDto post);
        Task LikePost(LikePostDto like);
        Task CommentPost(CreateCommentDto comment);
        Task<List<PostDto>> GetFeed(FeedDto feed);

    }
}
