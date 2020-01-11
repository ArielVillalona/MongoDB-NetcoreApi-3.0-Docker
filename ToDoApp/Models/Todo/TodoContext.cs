namespace ToDoApp.Models.Todo
{
    using MongoDB.Driver;
    using ToDoApp.Config;

    public class TodoContext : ITodoContext
    {
        private readonly IMongoDatabase _db;
        public TodoContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }
        public IMongoCollection<Todo> Todos => _db.GetCollection<Todo>("Todos");
    }
}
