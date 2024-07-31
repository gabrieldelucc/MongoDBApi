using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    // Define a rota base para este controlador e o especifica como um controlador de API
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        // Declaração de uma coleção MongoDB para a entidade User
        private readonly IMongoCollection<User> _users;

        // Construtor do controlador que injeta o serviço MongoDBService
        public UserController(MongoDBService mongoDBService)
        {
            // Obtém a coleção de usuários do banco de dados MongoDB
            _users = mongoDBService.GetDatabase.GetCollection<User>("user");
        }

        // Método para obter a lista de todos os usuários
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                // Busca todos os usuários na coleção
                var users = await _users.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(users); // Retorna a lista de usuários com o status HTTP 200 (OK)
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); // Retorna o erro com o status HTTP 400 (Bad Request)
            }
        }

        // Método para adicionar um novo usuário
        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            try
            {
                // Insere o novo usuário na coleção
                await _users.InsertOneAsync(user);
                return StatusCode(201, user); // Retorna o usuário criado com o status HTTP 201 (Created)
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); // Retorna o erro com o status HTTP 400 (Bad Request)
            }
        }

        // Método para deletar um usuário pelo ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                // Cria um filtro para encontrar o usuário pelo ID
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);
                if (filter != null)
                {
                    // Deleta o usuário que corresponde ao filtro
                    await _users.DeleteOneAsync(filter);
                    return Ok(); // Retorna o status HTTP 200 (OK) se a operação for bem-sucedida
                }
                return NotFound(); // Retorna o status HTTP 404 (Not Found) se o filtro não encontrar um usuário
            }
            catch (Exception)
            {
                return BadRequest(); // Retorna o status HTTP 400 (Bad Request) em caso de erro
            }
        }

        // Método para atualizar um usuário
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(User user)
        {
            try
            {
                // Cria um filtro para encontrar o usuário pelo ID
                var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                // Substitui o usuário existente pelo novo usuário
                var result = await _users.ReplaceOneAsync(filter, user);

                if (result.MatchedCount > 0)
                {
                    return Ok(); // Retorna o status HTTP 200 (OK) se a operação for bem-sucedida
                }
                return NotFound(); // Retorna o status HTTP 404 (Not Found) se o usuário não for encontrado
            }
            catch (Exception)
            {
                return BadRequest(); // Retorna o status HTTP 400 (Bad Request) em caso de erro
            }
        }
    }
}
