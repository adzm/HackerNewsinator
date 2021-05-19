using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HackerNewsinator
{
    public class HackerNewsClient : IHackerNewsClient
    {
        private readonly IHackerNewsAPI _api;
        private readonly IMemoryCache _localCache;
        private readonly IDistributedCache _distCache;

        public HackerNewsClient(IHackerNewsAPI api, IMemoryCache localCache, IDistributedCache distCache)
        {
            _api = api;
            _localCache = localCache;
            _distCache = distCache;
        }

        public async Task<IEnumerable<Article>> ExpandArticles(IEnumerable<int> storyIDs)
        {
            return await Task.WhenAll(storyIDs.Select((id) => ExpandItem(id)));
        }

        public async Task<Article> ExpandItem(int id)
        {
            var key = $"id_{id}";
            var articleString = await _distCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(articleString))
            {
                var article = await _api.GetItemDetails(id);

                articleString = JsonSerializer.Serialize(article);

                await _distCache.SetStringAsync(key, articleString);

                return article;
            }

            return JsonSerializer.Deserialize<Article>(articleString);
        }

        static public IEnumerable<int> PaginateArticles(IEnumerable<int> stories, int? after)
        {
            if (after.HasValue)
            {
                stories = stories.SkipWhile(id => id != after.Value).SkipWhile(id => id == after.Value);
            }

            return stories.Take(20);
        }

        public async Task<IEnumerable<Article>> GetFreshArticles(int? after)
        {
            var newStoryIDs = await _localCache.GetOrCreateAsync<IEnumerable<int>>("FreshArticles", async key =>
            {
                key.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);

                return await _api.GetFreshStoryIDs();
            });

            return await ExpandArticles(PaginateArticles(newStoryIDs, after));
        }

        public async Task<IEnumerable<Article>> SearchArticles(string q, int? after)
        {
            var newStoryIDs = await _localCache.GetOrCreateAsync<int[]>($"srch_{q}", async key =>
            {
                key.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300);
                key.Priority = CacheItemPriority.Low;
                var results = await _api.SearchForStoryIDs(q);
                return results.ToArray();
            });

            return await ExpandArticles(PaginateArticles(newStoryIDs, after));
        }
    }
}