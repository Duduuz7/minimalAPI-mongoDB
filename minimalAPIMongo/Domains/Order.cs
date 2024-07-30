using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Security.Cryptography.X509Certificates;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateOnly? Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        //Referência aos produtos do pedido

        [BsonElement("product_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string>? ProductId { get; set; }

        public List<Product>? Products { get; set; }

        //Referência ao cliente que está fazendo o pedido

        [BsonElement("client_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ClientId { get; set; }

        public Client? Client { get; set; }

        //public class OrderResponse()
        //{
        //    public Order Order { get; set; }
        //    public Client Client { get; set; }
        //    public List<Product> Products { get; set; }

        //}

    }
}
