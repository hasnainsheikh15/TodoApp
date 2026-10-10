using System.ComponentModel.DataAnnotations;
using TodoApi.Models;

namespace TodoApi.DTOs
{
    public class CreateTodoRequest
    {
        [Required]
        [MaxLength(200)]

        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public TodoPriority Priority { get; set; }

        [Required]
        [MaxLength(100)]

        public string Category { get; set; } = string.Empty;


    }
}
