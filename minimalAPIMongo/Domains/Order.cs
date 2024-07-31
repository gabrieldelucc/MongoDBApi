using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Order
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
        public List <string>? ProductId {  get; set; } = new List<string> ();

        public List<Product>? Products { get; set; }

        // referência ao cliente que fez o pedido 
        [BsonElement("clienteId")]
        public string ClientId { get ; set; }

        public Client? Client{ get; set; }
    }
}
