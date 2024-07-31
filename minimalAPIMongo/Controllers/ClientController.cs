using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        // Declaração da coleção MongoDB para a entidade Client
        private readonly IMongoCollection<Client> _client;

        // Construtor do controlador que injeta o serviço MongoDBService
        public ClientController(MongoDBService mongoDBService)
        {
            // Obtém a coleção necessária do banco de dados MongoDB
            _client = mongoDBService.GetDatabase.GetCollection<Client>("client");
        }

        // Método para obter a lista de todos os clientes
        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                // Busca todos os clientes na coleção
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Método para criar um novo cliente
        [HttpPost]
        public async Task<ActionResult> Post(Client client)
        {
            try
            {
                // Insere o novo cliente na coleção
                await _client.InsertOneAsync(client);
                return StatusCode(201, client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Método para deletar um cliente pelo ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                // Cria um filtro para encontrar o cliente pelo ID
                var filter = Builders<Client>.Filter.Eq(c => c.Id, id);
                var result = await _client.DeleteOneAsync(filter);

                if (result.DeletedCount > 0)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Método para atualizar um cliente
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Client client)
        {
            try
            {
                // Cria um filtro para encontrar o cliente pelo ID
                var filter = Builders<Client>.Filter.Eq(c => c.Id, id);
                var result = await _client.ReplaceOneAsync(filter, client);

                if (result.MatchedCount > 0)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
