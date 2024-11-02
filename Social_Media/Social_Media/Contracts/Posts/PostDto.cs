namespace Social_Media.Contracts.Posts
{
    public class PostDto
    {
        public PostDto(int postId, string content)
        {
            PostId = postId;
            Content = content;
        }

        public int PostId { get;  set; }
        public string Content { get;  set; }
    }
}
