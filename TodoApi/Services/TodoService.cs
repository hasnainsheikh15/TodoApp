using TodoApi.DTOs;
using TodoApi.Exceptions;
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

    public async Task<Todo> GetByIdAsync(int id)
    {
        var todo =  await _repository.GetByIdAsync(id);


        if (todo == null)
        {
            throw new NotFoundException(
                $"Todo with ID {id} was not found.");
        }

        return todo;
    }

    public async Task<Todo> CreateAsync(CreateTodoRequest request)
    {
        var todo = new Todo
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Category = request.Category,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        return await _repository.CreateAsync(todo);
    }

    public async Task<Todo> UpdateAsync(int id, UpdateTodoRequest request)
    {
        Console.WriteLine($"REQUEST IsCompleted: {request.IsCompleted}");
        var todo = new Todo
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Category = request.Category,
            IsCompleted = request.IsCompleted
        };

        Console.WriteLine($"TODO IsCompleted: {todo.IsCompleted}");
        var updatedTodo =  await _repository.UpdateAsync(id, todo);

        if (updatedTodo == null)
        {
            throw new NotFoundException(
                $"Todo with ID {id} was not found.");
        }
        Console.WriteLine($"UPDATED IsCompleted: {updatedTodo.IsCompleted}");
        return updatedTodo;

    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await _repository.DeleteAsync(id);

        if (!deleted) throw new NotFoundException(
            $"Todo with ID {id} was not found.");
    }
}