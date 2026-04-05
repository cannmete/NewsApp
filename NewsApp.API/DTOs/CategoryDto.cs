using NewsApp.API.DTOs;

namespace NewsApp.API.DTOs
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}