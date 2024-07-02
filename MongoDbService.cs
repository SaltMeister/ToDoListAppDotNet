
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using ToDoListAppDotNet;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

public class MongoDbService
{
   private readonly IMongoDatabase _database;

    public MongoDbService ()
    {
        try
        {
            var mongoClient = new MongoClient(MongoDbEnv.ConnectionString);
            _database = mongoClient.GetDatabase(MongoDbEnv.DatabaseName);
            Console.WriteLine("Successful connection to db");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    // Called to grab collection from DB
    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

}
