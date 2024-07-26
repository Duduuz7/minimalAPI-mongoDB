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
    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product> _product;

        public ProductController(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product p)
        {
            try
            {
                //Product novoProduto = new Product()
                //{
                //    Name = p.Name,
                //    Price = p.Price,
                //    AdditionalAttributes = p.AdditionalAttributes
                //};

                await _product.InsertOneAsync(p);

                return Ok(p);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _product.DeleteOneAsync(p => p.Id == id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, Product product)
        {

            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var res = await _product.Find(filter).ToListAsync();

                if (product.Name == null)
                {
                    product.Name = res.First().Name;
                }

                if (product.Price == 0)
                {
                    product.Price = res.First().Price;
                }

                if (product.AdditionalAttributes == null)
                {
                    product.AdditionalAttributes = res.First().AdditionalAttributes;
                }

                var update = Builders<Product>.Update
                    .Set(p => p.Name, product.Name)
                    .Set(p => p.Price, product.Price);

                await _product.UpdateOneAsync(filter, update);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var result = _product.Find(filter);

                return Ok(result.First());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
