using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsinator
{
    public interface IHackerNewsClient
    {
        public Task<IEnumerable<Article>> GetFreshArticles(int? after);
        public Task<IEnumerable<Article>> SearchArticles(string q, int? after);
    }
}
