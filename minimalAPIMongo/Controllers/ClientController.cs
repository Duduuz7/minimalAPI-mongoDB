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
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<User> _user;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }
 
        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            var clientsWithUsers = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();

            foreach (var item in clientsWithUsers)
            {
                User? user = await _user.Find(x => x.Id == item.UserId).FirstOrDefaultAsync();

                item.User = user;
            }

            return Ok(clientsWithUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            var client = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
            var user = await _user.Find(x => x.Id == client.UserId).FirstOrDefaultAsync();

            client.User = user;


            return client == null ? NotFound() : Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Post(Client client)
        {
            await _client.InsertOneAsync(client);
            return Ok(client);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> Delete(string id)
        {
            await _client.DeleteOneAsync(x => x.Id == id);

            return NoContent();
        }
        [HttpPut]
        public async Task<ActionResult<Client>> Put(Client client)
        {
            await _client.ReplaceOneAsync(x => x.Id == client.Id, client);

            return NoContent();
        }
    }
}