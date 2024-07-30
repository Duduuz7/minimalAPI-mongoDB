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
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var clients = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();

                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User u)
        {
            try
            {
                await _user.InsertOneAsync(u);

                return Ok(u);
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
                await _user.DeleteOneAsync(p => p.Id == id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut()]
        public async Task<ActionResult> Put(User u)
        {

            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.Id, u.Id);

                if (filter != null)
                {
                    await _user.ReplaceOneAsync(filter, u);

                    return Ok(u);
                }

                return NotFound();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {

                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();

                return user is not null ? Ok(user) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
