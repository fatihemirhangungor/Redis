using StackExchange.Redis;

namespace Redis
{
    public class RedisClient : IDisposable
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisClient(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task<string?> GetValueAsync(string key)
        {
            var result = await _db.StringGetAsync(key);
            return result.HasValue ? result.ToString() : null;
        }

        public async Task SetValueAsync(string key, string value, TimeSpan expiry = default)
        {
            if (expiry != default)
            {
                await _db.StringSetAsync(key, value, expiry);
            }

            await _db.StringSetAsync(key, value, TimeSpan.FromHours(1));
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}
