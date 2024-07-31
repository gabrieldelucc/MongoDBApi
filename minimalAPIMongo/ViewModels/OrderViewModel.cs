using minimalAPIMongo.Domains;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace minimalAPIMongo.ViewModels
{
    public class OrderViewModel
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        //referências para os produtos do pedido 
        [BsonElement("productId")]
        public List<string>? ProductId { get; set; } = new List<string>();

        [BsonIgnore]
        [JsonIgnore]
        public List<Product>? Products { get; set; }

        // referência ao cliente que fez o pedido 
        [BsonElement("clienteId")]
        public string ClientId { get; set; }

        [BsonIgnore]
        [JsonIgnore] 
        public Client? Client { get; set; }
    }
}
