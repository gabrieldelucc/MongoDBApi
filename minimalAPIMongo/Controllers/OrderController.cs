using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using minimalAPIMongo.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace minimalAPIMongo.Controllers
{
    // Define a rota base para este controlador e o especifica como um controlador de API
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Declaração de coleções MongoDB para as entidades Order, Client e Product
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        // Construtor do controlador que injeta o serviço MongoDBService
        public OrderController(MongoDBService mongoDBService)
        {
            // Obtém as coleções necessárias do banco de dados MongoDB
            _order = mongoDBService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDBService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDBService.GetDatabase.GetCollection<Product>("product");
        }

        // Método para criar um novo pedido
        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderViewModel orderViewModel)
        {
            try
            {
                // Cria uma nova instância de Order a partir do ViewModel
                Order order = new Order
                {
                    Id = orderViewModel.Id,
                    Date = orderViewModel.Date,
                    Status = orderViewModel.Status,
                    ProductId = orderViewModel.ProductId,
                    ClientId = orderViewModel.ClientId
                };

                // Verifica se o cliente existe
                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("Cliente não existe!");
                }
                order.Client = client;

                // Insere o novo pedido na coleção
                await _order.InsertOneAsync(order);

                return StatusCode(201, order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Método para obter a lista de todos os pedidos
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                // Busca todos os pedidos na coleção
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
                foreach (var order in orders)
                {
                    // Busca os produtos associados ao pedido
                    if (order.ProductId != null)
                    {
                        var filter = Builders<Product>.Filter.In(p => p.Id, order.ProductId);
                        order.Products = await _product.Find(filter).ToListAsync();
                    }

                    // Busca o cliente associado ao pedido
                    if (order.ClientId != null)
                    {
                        order.Client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();
                    }
                }
                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest($"não foi possível encontrar {e.Message}");
            }
        }

        // Método para obter um pedido pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                // Busca o pedido pelo ID
                var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }
        }

        // Método para deletar um pedido pelo ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                // Cria um filtro para encontrar o pedido pelo ID
                var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
                var result = await _order.DeleteOneAsync(filter);

                if (result.DeletedCount > 0)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }
        }

        // Método para atualizar um pedido
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, OrderViewModel orderViewModel)
        {
            try
            {
                // Verifica se o cliente existe
                var client = await _client.Find(x => x.Id == orderViewModel.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("Cliente não existe!");
                }

                // Verifica se os produtos existem
                var products = await _product.Find(x => orderViewModel.ProductId.Contains(x.Id)).ToListAsync();

                if (products.Count != orderViewModel.ProductId.Count)
                {
                    return NotFound("Um ou mais produtos não existem!");
                }

                // Cria uma nova instância de Order atualizada a partir do ViewModel
                Order updatedOrder = new Order
                {
                    Id = orderViewModel.Id,
                    Date = orderViewModel.Date,
                    Status = orderViewModel.Status,
                    ProductId = orderViewModel.ProductId,
                    ClientId = orderViewModel.ClientId,
                    Client = client,
                    Products = products
                };

                // Cria um filtro para encontrar o pedido pelo ID
                var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
                var result = await _order.ReplaceOneAsync(filter, updatedOrder);

                if (result.MatchedCount > 0)
                {
                    return Ok(updatedOrder);
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
