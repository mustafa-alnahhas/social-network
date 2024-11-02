using Microsoft.AspNetCore.Mvc;

namespace Social_Media.Contracts.Users
{
    public interface IUserService
    {
        Task Register([FromBody] RegistarDto userInfo);
        Task<string> Login([FromBody] LoginDto userInfo);

    }
}
