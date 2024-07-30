using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace minimalAPIMongo.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonIgnore]
        public User? User { get; set; }

        [BsonElement("cpf")]
        public string Cpf { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }

        public Client()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
