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
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoRepository repository, IMapper mapper, ILogger<TodoController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDtoModel>> GetTodo(int id)
        {
            try
            {
                var todo = await _repository.GetTodo(id);
                if (todo == null)
                {
                    _logger.LogWarning($"Todo with Id: {id}, does not exist");
                    return NotFound();
                }

                _logger.LogInformation($"Fetching Todo with Id: {id}, was successful");
                return Ok(_mapper.Map<TodoDtoModel>(todo));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while fetching Todo with Id: {id}.");
                return StatusCode(500, "Internal Server Error");
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoDtoModel>>> GetAllTodos(int page, int pageSize, string filter)
        {            
            try
            {
                var todos = await _repository.GetAllTodos(page, pageSize, filter);
                if (todos == null)
                {
                    _logger.LogWarning("No Todos Exist");
                    return NotFound();
                }
                var todosDto = _mapper.Map<List<TodoDtoModel>>(todos);

                _logger.LogInformation("Fetching all Todos successful");
                return Ok(todosDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while fetching all Todos");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoDbModel>> PostTodo(TodoDbModel todoDb)
        {
            try
            {
                var createdTodo = await _repository.AddTodo(todoDb);

                _logger.LogInformation($"Todo created with Id: {createdTodo.Id}");
                return CreatedAtAction("GetTodo", new { id = createdTodo.Id }, createdTodo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while adding a Todo");
                return StatusCode(500, "Internal Server Error");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            try
            {
                var deletedTodo = await _repository.DeleteTodo(id);
                if (deletedTodo == null)
                {
                    _logger.LogWarning($"Deleting Todo with Id: {id}, does not exist.");
                    return NotFound();
                }

                _logger.LogInformation($"Deleted Todo with Id: {id}, was successful.");
                return NoContent();
            }
            catch (Exception e)
            {

                _logger.LogError(e, $"Error while deleting Todo with Id: {id}.");
                return StatusCode(500, "Internal Server Error");
            }
            
        }

        [HttpPut]
        public async Task<ActionResult<TodoDtoModel>> UpdateTodo(TodoDtoModel todoDto)
        {
            try
            {
                var updatedTodo = await _repository.UpdateTodo(_mapper.Map<TodoDbModel>(todoDto));
                if (updatedTodo == null)
                {
                    _logger.LogWarning($"Updating Todo with Id: {todoDto.Id}, does not exist.");
                    return NotFound();
                }

                var updatedTodoDto = _mapper.Map<TodoDtoModel>(updatedTodo);
                _logger.LogInformation($"Updating Todo with Id: {updatedTodoDto.Id}, was successful.");
                return Ok(updatedTodoDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while updating Todo with Id: {todoDto.Id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
