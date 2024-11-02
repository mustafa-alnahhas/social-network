using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Social_Media.Contracts.Posts;
using Social_Media.Contracts.Shared;
using Social_Media.Domain;

namespace Social_Media.Services
{
    public class PostService : ControllerBase, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMemoryCache _cache;

        public PostService(IPostRepository postRepository, IMemoryCache cache)
        {
            _postRepository = postRepository;
            _cache = cache;
        }

        /*
            This method:
                - creates post from CreatePostDto.
        */
        public async Task CreatePost(CreatePostDto post)
        {
            _cache.TryGetValue(Constants.USERS_KEY, out List<User> UserList);

            if (!UserList.Any(u => u.UserId == post.UserId))
            {
                throw new ArgumentException("userId not found", post.UserId.ToString());
            }

            Post entity = Post.CreatePost(post.UserId, post.Content, DateTime.Now);

            await _postRepository.InsertPost(entity);

        }

        /*
            This method:
                - likes post from LikePostDto.
        */
        public async Task LikePost(LikePostDto like)
        {
            _cache.TryGetValue(Constants.USERS_KEY, out List<User> UserList);

            if (!UserList.Any(u => u.UserId == like.UserId))
            {
                throw new ArgumentException("userId not found", like.UserId.ToString());
            }

            if (!await _postRepository.PostIsExist(like.PostId))
            {
                throw new ArgumentException("postId not found", like.PostId.ToString());
            }

            Like entity = Like.CreateLike(like.UserId, like.PostId);

            await _postRepository.LikePost(entity);
        }

        /*
            This method:
                - comments on post from CreateCommentDto.
        */
        public async Task CommentPost( CreateCommentDto comment)
        {
            _cache.TryGetValue(Constants.USERS_KEY, out List<User> UserList);

            if (!UserList.Any(u => u.UserId == comment.UserId))
            {
                throw new ArgumentException("userId not found", comment.UserId.ToString());
            }

            if (!await _postRepository.PostIsExist(comment.PostId))
            {
                throw new ArgumentException("postId not found", comment.PostId.ToString());
            }
            Comment entity = Comment.CreateComment(comment.PostId, comment.UserId, comment.Content, DateTime.Now);

            await _postRepository.CommentPost(entity);

        }


        /*
            This method:
                - gets the newest feeds for a specific user.
                - uses pagination for performance.
        */
        public async Task<List<PostDto>> GetFeed(FeedDto feed)
        {
            var posts = await _postRepository.GetFeed(feed.UserId, feed.Skip, feed.NumberOfPostsPerPage);

            return posts.Select(p => new PostDto(p.PostId, p.Content)).ToList();
        }


    }
}
