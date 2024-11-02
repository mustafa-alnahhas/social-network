using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Contracts.Posts;

namespace Social_Media.Controllers.Posts
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase, IPostService
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /*
            For each call on these api's api key and jwt token must be provided 
        */
        [HttpGet("GetFeed")]
        [Authorize]
        public async Task<List<PostDto>> GetFeed([FromQuery] FeedDto feed)
        {
            return await _postService.GetFeed(feed);
        }

        [Authorize]
        [HttpPost("CommentPost")]
        public async Task CommentPost(CreateCommentDto comment)
        {
            await _postService.CommentPost(comment);
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public async Task CreatePost(CreatePostDto post)
        {
            await _postService.CreatePost(post);
        }

        [Authorize]
        [HttpPost("LikePost")]
        public async Task LikePost(LikePostDto like)
        {
            await _postService.LikePost(like);
        }
    }
}
