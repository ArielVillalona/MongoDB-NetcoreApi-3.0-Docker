
namespace ToDoApp.Models.Todo
{
    using MongoDB.Driver;
    public interface ITodoContext
    {
        IMongoCollection<Todo> Todos { get; }
    }
}
