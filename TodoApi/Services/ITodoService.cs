using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<List<Todo>> GetAllTodosAsync(string? search,
        string? category,
        TodoPriority? priority);

    Task<Todo> GetByIdAsync(int id);

    Task<Todo> CreateAsync(CreateTodoRequest request);

    Task<Todo> UpdateAsync(int id, UpdateTodoRequest request);

    Task DeleteAsync(int id);
}