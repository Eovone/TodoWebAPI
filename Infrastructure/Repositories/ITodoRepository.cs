using Entity;

namespace Infrastructure.Repositories
{
    public interface ITodoRepository
    {
        Task<TodoDbModel> AddTodo(TodoDbModel todo);
        Task<TodoDbModel> GetTodo(int id);
        Task<List<TodoDbModel>> GetAllTodos(int limit);
    }
}
