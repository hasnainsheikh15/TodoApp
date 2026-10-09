using TodoApi.Models;

namespace TodoApi.Repository
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllTodosAsync();

        Task<Todo?> GetByIdAsync(int id);

        Task<Todo> CreateAsync(Todo todo);

        Task<Todo?> UpdateAsync(int id, Todo todo);

        Task<bool> DeleteAsync(int id);

    }

}
