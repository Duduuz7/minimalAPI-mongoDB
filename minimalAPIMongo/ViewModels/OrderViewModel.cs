using minimalAPIMongo.Domains;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace minimalAPIMongo.ViewModels
{
    public class OrderViewModel
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("date")]
        public DateOnly? Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }


        [BsonElement("clientId"), BsonRepresentation(BsonType.ObjectId)]

        public string? ClientId { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public Client? Client { get; set; }


        [BsonElement("productId")]

        public List<string>? ProductId { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}
