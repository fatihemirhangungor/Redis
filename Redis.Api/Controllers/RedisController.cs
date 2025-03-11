using Microsoft.AspNetCore.Mvc;

namespace Redis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly RedisClient _redisClient;

        public RedisController(RedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<string>> GetKey(string key)
        {
            var result = await _redisClient.GetValueAsync(key);

            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] KeyValuePair<string, string> keyValue)
        {
            await _redisClient.SetValueAsync(keyValue.Key, keyValue.Value);
            return Ok();
        }
    }
}
