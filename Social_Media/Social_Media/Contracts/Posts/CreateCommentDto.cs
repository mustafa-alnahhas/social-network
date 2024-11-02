namespace Social_Media.Contracts.Posts
{
    public class CreateCommentDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
