using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using minimalAPIMongo.ViewModels;
using MongoDB.Driver;
using System.Linq;
using static minimalAPIMongo.Domains.Order;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();


                //Foreach para setar os produtos a partir da lista Orders e o client

                //foreach (var order in orders)
                //{
                //Verifica se eciste um id de prodto
                //    if (order.ProductId != null)
                //    {
                //Filtra os que tem o mesmo idProduto e pega esse id
                //        var filter = Builders<Product>.Filter.In(p => p.Id, order.ProductId);

                //usa o Id captado no filtro anterior para buscar os produtos pelo id, ja setando os Produtos da Order com esses produtos buscados aq
                //        order.Products = await _product.Find(filter).ToListAsync();
                //    }

                      //if (order.ClientId != null)
                      //{
                      //   order.Client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();
                      //}
                    //}



                    return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("id")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post(OrderViewModel order)
        {
            try
            {
                Order ord = new Order();

                ord.Id = order.Id;
                ord.Date = order.Date;
                ord.Status = order.Status;
                ord.ProductId = order.ProductId;
                ord.ClientId = order.ClientId;

                Client client = await _client.Find(x => x.Id == ord.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("Cliente não encontrado!");
                }

                ord.Client = client;

                var l = new List<Product>();

                foreach (var item in ord.ProductId)
                {
                    Product product = await _product.Find(x => x.Id == item).FirstOrDefaultAsync();

                    if (product != null)
                    {
                        l.Add(product);
                    }

                }

                ord.Products = l;

                await _order.InsertOneAsync(ord);

                return StatusCode(201, ord);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut]
        public async Task<ActionResult<Order>> Put(Order order)
        {
            try
            {
                Order ord = new Order();

                ord.Id = order.Id;
                ord.Date = order.Date;
                ord.Status = order.Status;
                ord.ProductId = order.ProductId;
                ord.ClientId = order.ClientId;

                Client client = await _client.Find(x => x.Id == ord.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("Cliente não encontrado!");
                }

                ord.Client = client;

                var l = new List<Product>();

                foreach (var item in ord.ProductId!)
                {
                    Product product = await _product.Find(x => x.Id == item).FirstOrDefaultAsync();

                    if (product != null)
                    {
                        l.Add(product);
                    }

                }

                ord.Products = l;

                await _order.ReplaceOneAsync(x => x.Id == ord.Id, ord);

                return StatusCode(201, ord);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Order>> Delete(string id)
        {
            try
            {
                await _order.DeleteOneAsync(x => x.Id == id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            
        }


    }
}
