namespace NewsApp.API.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public int Count { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string? ProfilePhotoPath { get; set; }
    }
}