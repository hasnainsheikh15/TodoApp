using TodoApi.Models;

namespace TodoApi.Repository
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllTodosAsync(
            string? search , string? category , TodoPriority? priority);

        Task<Todo?> GetByIdAsync(int id);

        Task<Todo> CreateAsync(Todo todo);

        Task<Todo?> UpdateAsync(int id, Todo todo);

        Task<bool> DeleteAsync(int id);

    }

}
