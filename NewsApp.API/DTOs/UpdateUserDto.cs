namespace NewsApp.API.DTOs
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? ProfilePhotoPath { get; set; }
    }
}