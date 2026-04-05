namespace NewsApp.API.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int NewsId { get; set; }

        public string? AppUserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}