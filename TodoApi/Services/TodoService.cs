using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Todo>> GetAllTodosAsync()
    {
        return await _repository.GetAllTodosAsync();
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Todo> CreateAsync(Todo todo)
    {
        todo.CreatedAt = DateTime.UtcNow;
        todo.UpdatedAt = null;

        return await _repository.CreateAsync(todo);
    }

    public async Task<Todo?> UpdateAsync(int id, Todo todo)
    {
        return await _repository.UpdateAsync(id, todo);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}