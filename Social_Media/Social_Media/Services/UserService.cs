using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Social_Media.Contracts.Shared;
using Social_Media.Contracts.Users;
using Social_Media.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Social_Media.Services
{
    public class UserService: IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        
        private PasswordHasher<Object> _passwordHasher;

        private readonly IMemoryCache _cache;




        public UserService(IConfiguration configuration, IUserRepository userRepository, IMemoryCache cache)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<object>();
            _cache = cache;
        }

        /*
            This method:
                - register a new user with username and password.
                - hash the password before save it into database.
        */
        public async Task Register([FromBody] RegistarDto userInfo)
        {
            // get the users list from cache
            _cache.TryGetValue(Constants.USERS_KEY, out List<User> UserList);

            // no need for check users from database anymore
            //if (await _userRepository.IsUserExist(userInfo.UserName))
            //{
            //    throw new ArgumentException("username already exist", userInfo.UserName);
            //}

            // check if user exist from cache
            if(UserList.Any(u => u.Username == userInfo.UserName))
            {
                throw new ArgumentException("username already exist", userInfo.UserName);
            }

            // Create a new user and hash the password
            User user = User.CreateUser(userInfo.UserName, _passwordHasher.HashPassword(null, userInfo.Password), DateTime.Now);

            // insert the user into database
            await _userRepository.InsertUser(user);

            // check if there is no register user yet 
            if (UserList is null) 
            {
                UserList = new List<User>();
            }
            
            // add the new created user to the cache
            UserList.Add(user);

            // update the list in the cache
            _cache.Set(Constants.USERS_KEY, UserList);


        }

        /*
            This method:
                - gets username and password from the user, if they are valid returns token.
                - compares the hash of the user password with the hash password stored in the database.
        */
        public async Task<string> Login([FromBody] LoginDto userInfo)
        {
            _cache.TryGetValue(Constants.USERS_KEY, out List<User> UserList);

            // no need to check from database anymore
            // check if user does exist
            //User user = await _userRepository.GetUser(userInfo.UserName);

            // check in cache for user
            User user = UserList.FirstOrDefault(u => u.Username == userInfo.UserName);

            if (user == null)
            {
                throw new ArgumentException("username does not exist", userInfo.UserName);
            }

            // verify the password
            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, userInfo.Password);

            if(result != PasswordVerificationResult.Success)
            {
                throw new ArgumentException("wrong password", userInfo.Password);
            }

            var token = GenerateJwtToken(user);

            return token;
        }

        /*
            This method: 
                - generate jwt bearer token based on username and jwt key in the appsettings.
        */
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

