using Social_Media.Domain;

namespace Social_Media.Contracts.Users
{
    public interface IUserRepository
    {
        Task InsertUser(User user);
        Task<bool> IsUserExist(string username);
        Task<User> GetUser(string username);
    }
}
