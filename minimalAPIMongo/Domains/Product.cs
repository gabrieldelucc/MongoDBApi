using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    // Classe que representa um Produto no MongoDB
    public class Product
    {
        // Atributo que representa o ID do produto no MongoDB
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }  // O ID do produto, que é um ObjectId no MongoDB, mas é representado como string na aplicação

        
        [BsonElement("name")]
        public string Name { get; set; }  

       
        [BsonElement("price")]
        public string Price { get; set; }  // O preço do produto

        // Dicionário que contém atributos adicionais do produto
        public Dictionary<string, string> AdditionalAttributs { get; set; }  // Dicionário para armazenar atributos adicionais do produto

        // Construtor padrão da classe Product
        public Product()
        {
            // Inicializa o dicionário de atributos adicionais
            AdditionalAttributs = new Dictionary<string, string>();
        }
    }
}
