namespace Social_Media.Contracts.Posts
{
    public class FeedDto
    {
        public int UserId { get; set; }
        public int NumberOfPostsPerPage { get;  set; }
        public int Skip { get; set; }

    }
}
