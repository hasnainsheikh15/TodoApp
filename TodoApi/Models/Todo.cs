using System.Globalization;

namespace TodoApi.Models
{
    public class Todo
    {
        public int Id { get; set; }

        public String Title { get; set; } = string.Empty;

        public string? Description { get; set; }


        public bool IsCompleted { get; set; }

        public TodoPriority Priority { get; set; }

        public String Category { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public enum TodoPriority { 
        Low,Medium,High
    }
}
