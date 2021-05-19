using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace HackerNewsinator
{
    public class HackerNewsAPI : IHackerNewsAPI
    {
        private static readonly HttpClient _http = new(); // do not dispose HttpClient!

        public class SearchHit
        {
            public string objectID { get; set; }
        }

        public class SearchResults
        {
            public SearchHit[] hits { get; set; }
        }

        public async Task<IEnumerable<int>> GetFreshStoryIDs()
        {
            return await _http.GetFromJsonAsync<int[]>("https://hacker-news.firebaseio.com/v0/newstories.json");
        }

        public async Task<IEnumerable<int>> SearchForStoryIDs(string q)
        {
            var queryString = QueryHelpers.AddQueryString("http://hn.algolia.com/api/v1/search",
                new Dictionary<string, string> {
                        { "query", q },
                        { "hitsPerPage", "500" },
                        { "tags", "story" }
                });
            var results = await _http.GetFromJsonAsync<SearchResults>(queryString);

            return results.hits.Select(h => int.Parse(h.objectID)).ToArray();
        }

        public class Item
        {
            public string title { get; set; }
            public string url { get; set; }
            public int score { get; set; }
        };

        public async Task<Article> GetItemDetails(int id)
        {
            var item = await _http.GetFromJsonAsync<Item>($"https://hacker-news.firebaseio.com/v0/item/{id}.json");

            if (string.IsNullOrEmpty(item.url))
            {
                item.url = $"https://news.ycombinator.com/item?id={id}";
            }
            if (string.IsNullOrEmpty(item.title))
            {
                item.title = "(no title)";
            }

            return new Article()
            {
                ID = id,
                Title = item.title,
                URL = item.url,
                Score = item.score
            };
        }
    }
}
