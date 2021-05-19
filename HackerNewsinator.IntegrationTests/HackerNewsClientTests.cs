using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System;
using Xunit;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HackerNewsinator.IntegrationTests
{
    public class HackerNewsMockAPI : IHackerNewsAPI
    {
        public Task<IEnumerable<int>> GetFreshStoryIDs()
        {
            return Task.FromResult(Enumerable.Range(100, 500));
        }

        public Task<IEnumerable<int>> SearchForStoryIDs(string q)
        {
            return Task.FromResult(Enumerable.Range(10, 100));
        }

        public Task<Article> GetItemDetails(int id)
        {
            return Task.FromResult(new Article()
            {
                ID = id,
                Title = $"Article #{id}",
                URL = $"https://news.ycombinator.com/item?id={id}",
                Score = id % 10
            });
        }
    }

    public class HackerNewsClientTests
    {
        private readonly HackerNewsClient _client;
        private readonly IHackerNewsAPI _api;
        private readonly IMemoryCache _localCache;
        private readonly IDistributedCache _distCache;

        public HackerNewsClientTests()
        {
            _api = new HackerNewsMockAPI();
            _localCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            _distCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            _client = new HackerNewsClient(_api, _localCache, _distCache);
        }

        [Fact]
        public void TestExpandItem()
        {
            var id = 200;

            var article = _client.ExpandItem(id).Result;
            Assert.Equal(200, article.ID);
            Assert.Equal($"Article #{id}", article.Title);

            var article2 = _client.ExpandItem(id).Result;
            Assert.Equal(article.ID, article2.ID);
            Assert.Equal(article.Title, article2.Title);
            Assert.Equal(article.URL, article2.URL);
            Assert.Equal(article.Score, article2.Score);
        }
    }
}
