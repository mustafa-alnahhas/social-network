using Microsoft.EntityFrameworkCore;
using Social_Media.Contracts.Users;
using Social_Media.Domain;
//using System.Data.Entity;

namespace Social_Media.EFCore.Users
{

    /*
        This class:
        - represents user repository that has the database funtionalities.
        - used in post service.
    */
    public class UserRepository: IUserRepository
    {
        private readonly SocialContext _context;

        public UserRepository(SocialContext context)
        {
            _context = context;
        }

        /*
            This method:
                - inserts new user into post table.
        */

        public async Task InsertUser(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        /*
            This method:
                - checks if the user is already exist.
                - returns boolean.
        */
        public async Task<bool> IsUserExist(string username)
        {
            var temp = await _context.Users.AnyAsync(u => u.Username == username);
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        /*
            This method:
                - finds user by name.
                - returns the name if found.
        */
        public async Task<User> GetUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

    }
}
