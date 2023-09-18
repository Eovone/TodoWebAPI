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

        public async Task<List<TodoDbModel>> GetAllTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<TodoDbModel> GetTodo(int id)
        {
            return await _context.Todos.FindAsync(id);
        }
    }
}
