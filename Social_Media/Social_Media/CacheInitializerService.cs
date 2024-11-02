//using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Social_Media.Contracts.Shared;
using Social_Media.Contracts.Users;
using Social_Media.Domain;
//using System.Data.Entity;

namespace Social_Media
{
    /*
        This class :
            - Is a IHostedService (background job) starts on startup of project.
            - Fills the users cache to be used later.
    */
    public class CacheInitializerService : IHostedService
    {
        private readonly IMemoryCache _cache;
        private readonly IDbContextFactory<SocialContext> _context;

        public CacheInitializerService(IMemoryCache cache, IDbContextFactory<SocialContext> context)
        {
            _cache = cache;
            _context = context;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var dbContext = _context.CreateDbContext())
            {
                List<User> users = new List<User>();
                users = await dbContext.Users.ToListAsync();
                if (users.Count > 0)
                {
                    _cache.Set(Constants.USERS_KEY, users);
                }
            }

                
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up if needed
            return Task.CompletedTask;
        }
    }
}
