using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    // Define o roteamento da API e indica que esta classe é um controlador de API.
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        // Instância da coleção de produtos do MongoDB.
        private readonly IMongoCollection<Product> _product;

        // O construtor recebe um serviço MongoDB e inicializa a coleção de produtos.
        public ProductController(MongoDBService mongoDBService)
        {
            // Obtém a coleção "product" do banco de dados MongoDB.
            _product = mongoDBService.GetDatabase.GetCollection<Product>("product");
        }

        // Endpoint para obter todos os produtos.
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                // Recupera todos os produtos da coleção.
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                // Retorna a lista de produtos com o status HTTP 200 (OK).
                return Ok(products);
            }
            catch (Exception e)
            {
                // Se ocorrer um erro, retorna o status HTTP 400 (Bad Request) com a mensagem do erro.
                return BadRequest(e.Message);
            }
        }

        // Endpoint para adicionar um novo produto.
        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            try
            {
                // Insere o novo produto na coleção.

                await _product.InsertOneAsync(product);
                // Retorna o status HTTP 201 (Created) com o produto inserido.
                return StatusCode(201, product);
            }
            catch (Exception e)
            {
                // Se ocorrer um erro, retorna o status HTTP 400 (Bad Request) com a mensagem do erro.
                return BadRequest(e.Message);
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
              
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
              
                if (filter != null)
                {
                    await _product.DeleteOneAsync(filter);

                    return Ok();
                }

               return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Endpoint para atualizar um produto pelo ID.
        [HttpPut]
        public async Task<ActionResult> Update(Product p)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(x => x.Id , p.Id);

                if (filter != null)
                {
                    // substituindo o objeto buscando pelo novo objeto 
                    await _product.ReplaceOneAsync(filter, p);

                    return Ok();

                }
                return NotFound();
        
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();


                return product is not null ? Ok(product) : NotFound(); 

                
           
            }
            catch (Exception e)
            {

               return BadRequest(e.Message);
            }
        }

    }
}
