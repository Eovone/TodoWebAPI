using Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<TodoDbModel> AddTodo(TodoDbModel todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task<TodoDbModel> DeleteTodo(int id)
        {
            var todo =_context.Todos.FindAsync(id);

            if(todo.Result == null)
            {
                return null;
            }

            _context.Todos.Remove(todo.Result);
            await _context.SaveChangesAsync();

            return todo.Result;
        }

        public async Task<List<TodoDbModel>> GetAllTodos(int page, int pageSize, string filter)
        {
            if (pageSize <= 0)
            {
                return await _context.Todos.ToListAsync();
            }

            int offset = (page - 1) * pageSize;

            if (filter == "all")
            {
                return await _context.Todos.Skip(offset)
                                           .OrderBy(t => t.Deadline)
                                           .Take(pageSize)
                                           .ToListAsync();
            }  
            
            return await _context.Todos.Where(filter == "notdone" ? todo => todo.Completed == false : todo => todo.Completed == true)
                                        .Skip(offset)
                                        .OrderBy(t => t.Deadline)
                                        .Take(pageSize)
                                        .ToListAsync();    
        }

        public async Task<TodoDbModel> GetTodo(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<TodoDbModel> UpdateTodo(TodoDbModel todo)
        {
            var todoToUpdate = _context.Todos.FindAsync(todo.Id);

            if(todoToUpdate.Result == null)
            {
                return null;
            }

            todoToUpdate.Result.Title = todo.Title;
            todoToUpdate.Result.Description = todo.Description;
            todoToUpdate.Result.Completed = todo.Completed;
            todoToUpdate.Result.Deadline = todo.Deadline;

            await _context.SaveChangesAsync();

            return (todoToUpdate.Result);
        }
    }
}
