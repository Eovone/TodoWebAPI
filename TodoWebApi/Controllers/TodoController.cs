using Entity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using TodoWebApi.DtoModels;

namespace TodoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;
        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<TodoDbModel>> GetTodo(int id)
        {
            var todo = await _repository.GetTodo(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDbModel>> PostTodo(TodoDbModel todoDb)
        {
            var createdTodo = await _repository.AddTodo(todoDb);
            return CreatedAtAction("GetTodo", new { id = createdTodo.Id }, createdTodo);
        }
    }
}
