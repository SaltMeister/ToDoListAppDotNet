using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoListAppDotNet.Models
{
    public class ToDoItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; } = false;


    }
}
