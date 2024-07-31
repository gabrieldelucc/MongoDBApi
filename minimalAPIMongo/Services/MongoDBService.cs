using MongoDB.Driver;

namespace minimalAPIMongo.Services
{
    public class MongoDBService
    {
        /// <summary>
        /// armazenar a condiguração da aplicação 
        /// </summary>
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// armazenar a referência ao MongoDB
        /// </summary>
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Contém a configuração necessária para acesso ao MongoDB
        /// </summary>
        /// <param name="configuration"> Objeto contendo toda configuralção da aplicação</param>
        public MongoDBService(IConfiguration configuration)
        {
            //atribui a config recebida em _configuration 
            _configuration = configuration;

            //Acessa a string de conexão 
           var connectionString = _configuration.GetConnectionString("DbConnection");

            //Transforma a string obtida em MongoUrl 
          var mongoUrl = MongoUrl.Create(connectionString); 

            // Cria um client 
            var mongoClient =  new MongoClient(mongoUrl);

            //Obtém a referência ao MongoDb 
           _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        // propriedade para acessar o db, transforma os dados em _database 
        public IMongoDatabase GetDatabase => _database; 
    }
}
