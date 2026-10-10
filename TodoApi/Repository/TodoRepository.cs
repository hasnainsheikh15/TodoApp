using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
namespace TodoApi.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext context) {

            _context = context;

        }

        public async Task<List<Todo>> GetAllTodosAsync(
             string? search, string? category, TodoPriority? priority
            ) 
        {

            var query = _context.Todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search)) {
                query = query.Where
                    (todo => todo.Title.Contains(search) || (todo.Description != null && todo.Description.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(category)) {
                query = query.Where(todo => todo.Category == category);
            }

            if(priority.HasValue)
            {
                query = query.Where(todo => todo.Priority == priority.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(int id) {

            return await _context.Todos.FindAsync(id);

        }

        public async Task<Todo> CreateAsync(Todo todo) {

             _context.Todos.Add(todo);

            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task<Todo?> UpdateAsync(int id, Todo todo) {



            var existingTodo = await _context.Todos.FindAsync(id);

            if (existingTodo == null) return null;

            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsCompleted = todo.IsCompleted;
            existingTodo.Priority = todo.Priority;
            existingTodo.Category = todo.Category;
            existingTodo.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingTodo;
        }

        public async Task<bool> DeleteAsync(int id) {

            var todo = await _context.Todos.FindAsync(id);

            if (todo == null) return false;

            _context.Todos.Remove(todo);

            await _context.SaveChangesAsync();

            return true;
        } 

    }
}
