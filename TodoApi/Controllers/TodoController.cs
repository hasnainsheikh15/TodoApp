using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpGet]

        public async Task<ActionResult<List<Todo>>> GetAll(
            [FromQuery] string? search , [FromQuery] string? category , [FromQuery] TodoPriority? priority) {

            var todos = await _service.GetAllTodosAsync(search,category,priority);

            return Ok(todos);

        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Todo>> GetById(int id) {

            var todo = await _service.GetByIdAsync(id);

            return Ok(todo);
        }

        [HttpPost]

        public async Task<ActionResult<Todo>> Create(CreateTodoRequest request) {

            var todo = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = todo.Id },todo);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> Update(int id, UpdateTodoRequest request) {

            var todo = await _service.UpdateAsync(id, request);

            return Ok(todo);

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }

    }
}
