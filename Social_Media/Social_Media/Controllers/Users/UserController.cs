using Microsoft.AspNetCore.Mvc;
using Social_Media.Contracts.Users;

namespace Social_Media.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase, IUserService
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        /*
            For each call on these api's api key must be provided 
        */

        [HttpPost("login")]
        public async Task<string> Login([FromBody] LoginDto userInfo)
        {
            return await _userService.Login(userInfo);
        }

        [HttpPost("register")]
        public async Task Register([FromBody] RegistarDto userInfo)
        {
            await _userService.Register(userInfo);
        }
    }
}
