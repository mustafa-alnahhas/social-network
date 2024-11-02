using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Social_Media.Contracts.Posts;
using Social_Media.Domain;

namespace Social_Media.EFCore.Posts
{
    /*
        This class:
        - represents Post repository that has the database funtionalities.
        - used in post service.
       
    */
    public class PostRepository: IPostRepository
    {
        private readonly SocialContext _context;

        public PostRepository(SocialContext context)
        {
            _context = context;
        }

        /*
            This method:
            - inserts new post into post table.
        */

        public async Task InsertPost(Post post)
        {
            try
            {
                await _context.Posts.AddAsync(post);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
        }

        /*
            This method:
            - adds a like for a comment in like table.
        */
        public async Task LikePost(Like like)
        {
            try
            {
                await _context.Likes.AddAsync(like);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /*
            This method:
            - inserts new comment for a post table.
        */
        public async Task CommentPost(Comment comment)
        {
            try
            {
                await _context.Comments.AddAsync(comment);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

        /*
            This method:
            - gets the newest posts for specific userId.
            - gets the data by paginations using skip parameter to determine 
            how much to skip and NumberOfPostsPerPage parameter to determine how much to get per request.
            - us notracking in ef core the boost performance.
            - joins between feed and post tables and or by the creation date desc to get the newest posts.
            - returns list of post entities.
        */
        public async Task<List<Post>> GetFeed(int userId, int skip, int NumberOfPostsPerPage)
        {
            // check if the user does exist
            User user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if(user is null)
            {
                return null;
            }

            var posts = await _context.Feeds
            .AsNoTracking() // for better performance (no need for tracking, only reading data)
            .Where(f => f.UserId == userId)
            .Include(f => f.Post) // Include the Post navigation property
            .Select(f => new // select the properties to use later
            {
                PostId = f.PostId,
                UserId = f.Post.UserId,
                Content = f.Post.Content,
                CreatedAt = f.Post.CreatedAt
            })
            .OrderByDescending(x => x.CreatedAt) // order by CreatedAt desc to get the newest posts in feed
            // apply pagination
            .Skip(skip) 
            .Take(NumberOfPostsPerPage) // take the number of posts per page
            .ToListAsync(); // get the result

            // return list of the newest posts
            return posts.Select(p => Post.CreatePostForRead(p.UserId, p.Content, p.CreatedAt, p.PostId)).ToList();

        }

        // check if postid does exist
        public async Task<bool> PostIsExist(int postId)
        {
            return await _context.Posts.AnyAsync(p => p.PostId == postId);
        }



    }
}
