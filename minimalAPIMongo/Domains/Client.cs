using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("_userid")]
        public string? UserId { get; set; }

        [BsonElement("cpf")]
        public string Cpf { get; set; }

        [BsonElement("Phone")]
        public string Phone { get; set; }

        [BsonElement("Adress")]
        public string Adress { get; set; }

        public Dictionary<string, string> AdditionalAttributs { get; set; }

        public Client()
        {

            AdditionalAttributs = new Dictionary<string, string>();

        }
    }
}

