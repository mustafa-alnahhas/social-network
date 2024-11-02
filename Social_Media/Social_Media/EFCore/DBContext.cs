

using Microsoft.EntityFrameworkCore;
using Social_Media.Domain;

public class SocialContext : DbContext
{
    IConfiguration appConfig;

    public SocialContext(IConfiguration config)
    {
        appConfig = config;
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Feed> Feeds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(appConfig.GetConnectionString("SocialConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships:

        // User to Post (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // check for the username uniquness
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();


        // User to Post (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        // Post to Comment (One-to-Many)
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId);

        // Like relationship (Many-to-Many)
        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId);

        // Feed relationship (Many-to-Many)
        modelBuilder.Entity<Feed>()
            .HasOne(f => f.User)
            .WithMany(u => u.Feeds)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Feed>()
            .HasOne(f => f.Post)
            .WithMany(p => p.Feeds)
            .HasForeignKey(f => f.PostId);

        // Remove pluralizing table names using conventions
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Post>().ToTable("Post");
        modelBuilder.Entity<Comment>().ToTable("Comment");
        modelBuilder.Entity<Like>().ToTable("Like");
        modelBuilder.Entity<Feed>().ToTable("Feed");

        base.OnModelCreating(modelBuilder);
    }


}