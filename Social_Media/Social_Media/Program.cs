using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Social_Media;
using Social_Media.Contracts.Posts;
using Social_Media.Contracts.Users;
using Social_Media.EFCore.Posts;
using Social_Media.EFCore.Users;
using Social_Media.Middlewares;
using Social_Media.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Register the DbContext with the connection string
builder.Services.AddDbContextFactory<SocialContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SocialConnection")));

// add jwt settings
var key = builder.Configuration["Jwt:Key"]; // Your secret key from appsettings.json
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Add services and repositories to the container.
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddHostedService<CacheInitializerService>();


// add in-memory cache
builder.Services.AddMemoryCache();
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();




app.UseAuthorization();

// Register APIKeyMiddleware to be used on every request
app.UseAPIKeyMiddleware();

app.MapControllers();

app.Run();


