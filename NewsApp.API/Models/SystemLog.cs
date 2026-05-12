using System;

namespace NewsApp.API.Models
{
    public class SystemLog
    {
        public int Id { get; set; }
        public string Action { get; set; } 
        public string Details { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now; 
    }
}