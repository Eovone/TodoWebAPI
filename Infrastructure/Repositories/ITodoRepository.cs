using Entity;

namespace Infrastructure.Repositories
{
    public interface ITodoRepository
    {
        Task<TodoDbModel> AddTodo(TodoDbModel todo);
        Task<TodoDbModel> GetTodo(int id);
        Task<List<TodoDbModel>> GetTodos(int page, int pageSize, string filter);
        Task<TodoDbModel> DeleteTodo(int id);
        Task<TodoDbModel> UpdateTodo(TodoDbModel todo);
    }
}
