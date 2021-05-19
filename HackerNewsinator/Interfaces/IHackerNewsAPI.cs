using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsinator
{
    public interface IHackerNewsAPI
    {
        public Task<IEnumerable<int>> GetFreshStoryIDs();
        public Task<IEnumerable<int>> SearchForStoryIDs(string q);
        public Task<Article> GetItemDetails(int id);
    }
}
