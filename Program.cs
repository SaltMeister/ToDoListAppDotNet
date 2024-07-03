using MongoDB.Bson;
using MongoDB.Driver;
using System;
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

// Get All ToDoListGroups
app.MapGet("/toDoListGroup/", async (MongoDbService mongoDbService) =>
{
    var todoCollection = mongoDbService.GetCollection<ToDoItemGroup>("ToDoItemGroup");
    var filter = Builders<ToDoItemGroup>.Filter.Empty;

    List<ToDoItemGroup> searchResults = new List<ToDoItemGroup>();


    using (var cursor = await todoCollection.FindAsync(filter))
        cursor.ToList().ForEach(x => searchResults.Add(x));

    return new { Success = true, Reaspon = String.Empty, Body = searchResults};
});

app.MapPost("/toDoListGroup", async (ToDoItemGroup newToDoItemGroup, MongoDbService mongoDbService) =>
{
    var todoCollection = mongoDbService.GetCollection<ToDoItemGroup>("ToDoItemGroup");

    await todoCollection.InsertOneAsync(newToDoItemGroup);

    return new { Success = true, Reason = String.Empty, Body= newToDoItemGroup };
});

app.MapPut("toDoListGroup/{groupId}", async (String groupId, String newTitle, MongoDbService mongoDbService) =>
{
    var todoCollection = mongoDbService.GetCollection<ToDoItemGroup>("ToDoItemGroup");
    var filter = Builders<ToDoItemGroup>.Filter.Eq("_id", ObjectId.Parse(groupId));
    var update = Builders<ToDoItemGroup>.Update.Set(toDoItemGroup => toDoItemGroup.Title, newTitle);

    await todoCollection.UpdateOneAsync(filter, update);
});

app.MapDelete("/toDoListGroup", async (String groupId, MongoDbService mongoDbService) =>
{
    var todoCollection = mongoDbService.GetCollection<ToDoItemGroup>("ToDoItemGroup");
    var filter = Builders<ToDoItemGroup>.Filter.Eq("_id", ObjectId.Parse(groupId));

    await todoCollection.DeleteOneAsync(filter);

    return new { Success = true, Reason = String.Empty, Body = "" };
});

app.Run();