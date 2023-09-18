using AutoMapper;
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
        private readonly IMapper _mapper;
        public TodoController(ITodoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<TodoDtoModel>> GetTodo(int id)
        {
            var todo = await _repository.GetTodo(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TodoDtoModel>(todo));
        }

        [Route("GetAllTodos")]
        [HttpGet]
        public async Task<ActionResult<List<TodoDtoModel>>> GetAllTodos()
        {
            var todos = await _repository.GetAllTodos();

            if(todos == null) 
            {
                return NotFound("No Todos Exist");
            }

            return Ok(todos);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDbModel>> PostTodo(TodoDbModel todoDb)
        {
            var createdTodo = await _repository.AddTodo(todoDb);
            return CreatedAtAction("GetTodo", new { id = createdTodo.Id }, createdTodo);
        }
    }
}
