using MongoDB.Bson;
using MongoDB.Driver;
using ToDoListAppDotNet.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Setup Database Service
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/toDoList/{idString}", async (string idString, MongoDbService mongoDbService) =>
{
    var todoCollection = mongoDbService.GetCollection<ToDoItem>("ToDoItems");
    var filter = Builders<ToDoItem>.Filter.Eq("_id", ObjectId.Parse(idString));

    List<ToDoItem> searchResults = new List<ToDoItem>();


    using (var cursor = await todoCollection.FindAsync(filter))
    {
        cursor.ToList().ForEach(x => searchResults.Add(x));
    }


    return new { Success = true, Reaspon = String.Empty, Body = searchResults};
});

app.MapPost("/toDoList", async (ToDoItem newToDoItem, MongoDbService mongoDbService) =>
{

    if (newToDoItem.Title == String.Empty)
    {
        return new { Success=false, Reason="No Title Specified", Body=new ToDoItem() };
    }

    var todoCollection = mongoDbService.GetCollection<ToDoItem>("ToDoItems");

    await todoCollection.InsertOneAsync(newToDoItem);

    return new { Success = true, Reason = String.Empty, Body=newToDoItem};
});

app.MapDelete("/toDoList", async (String listId, MongoDbService mongoDbService) =>
{

});

app.Run();