using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsinator.Controllers
{
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IHackerNewsClient _client;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ILogger<ArticlesController> logger, IHackerNewsClient client)
        {
            _logger = logger;
            _client = client;
        }

        [Route("Articles")]
        public async Task<IEnumerable<Article>> Get(int? after)
        {
            return await _client.GetFreshArticles(after);
        }

        [Route("Articles/Search")]
        public async Task<IEnumerable<Article>> Search(string q, int? after)
        {
            return await _client.SearchArticles(q, after);
        }
    }
}
