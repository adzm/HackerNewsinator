using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System;
using Xunit;
using Microsoft.Extensions.Options;
using System.Linq;

namespace HackerNewsinator.Tests
{
    public class HackerNewsClientTests
    {
        public HackerNewsClientTests()
        {
        }

        [Fact]
        public void TestPaginateArticlesFromStart()
        {
            var storyIDs = Enumerable.Range(100, 500);

            var nextPage = HackerNewsClient.PaginateArticles(storyIDs, null);

            Assert.Equal(20, nextPage.Count());
            Assert.Equal(100, nextPage.First());
            Assert.Equal(119, nextPage.Last());
        }

        [Fact]
        public void TestPaginateArticles()
        {
            var storyIDs = Enumerable.Range(100, 500);

            var nextPage = HackerNewsClient.PaginateArticles(storyIDs, 200);

            Assert.Equal(20, nextPage.Count());
            Assert.Equal(201, nextPage.First());
            Assert.Equal(220, nextPage.Last());
        }

        [Fact]
        public void TestPaginateArticlesAtEnd()
        {
            var storyIDs = Enumerable.Range(100, 500);

            var nextPage = HackerNewsClient.PaginateArticles(storyIDs, 590);

            Assert.Equal(9, nextPage.Count());
            Assert.Equal(591, nextPage.First());
            Assert.Equal(599, nextPage.Last());
        }
    }
}
