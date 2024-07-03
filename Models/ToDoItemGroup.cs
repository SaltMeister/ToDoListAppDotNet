using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoListAppDotNet.Models
{
    public class ToDoItemGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("title")]
        public string Title { get; set; } = String.Empty;

        [BsonElement("taskGroupDate")]
        public DateTime createdDate { get; set; } = DateTime.UtcNow;


    }
}
