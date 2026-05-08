using MCV_Fantasy_Bzaar.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCV_Fantasy_Bzaar.Tests
{
        public class EncyclopediaTests
        {
            [Fact]
            public void Service_ShouldLoadData_OnInitialization()
            {
                var service = new EncyclopediaService();

                Assert.NotEmpty(service.AllComics);
                Assert.True(service.AllComics.Count > 0);
            }

            [Fact]
            public void Search_ShouldReturnCorrectResults_WhenQueryMatches()
            {
                var service = new EncyclopediaService();
                var query = service.AllComics.First().Title;

                var results = service.SearchAndTrack(query, "", "", "", "");

                Assert.Contains(results, b => b.Title == query);
            }

            [Fact]
            public void Analytics_ShouldTrackSearchFrequency()
            {
                var service = new EncyclopediaService();
                var query = "test-comic";

                service.SearchAndTrack(query, "", "", "", "");
                service.SearchAndTrack(query, "", "", "", "");

                Assert.Equal(2, service.SearchCounts[query.ToLower()]);
            }

            [Fact]
            public void Flagging_ShouldSetIsFlaggedToTrue()
            {
                var service = new EncyclopediaService();
                var title = service.AllComics.First().Title;

                service.FlagRecord(title);

                var book = service.AllComics.FirstOrDefault(b => b.Title == title);
                Assert.True(book.IsFlagged);
            }

            [Theory]
            [InlineData("NonExistentBook")]
            [InlineData("")]
            public void Search_ShouldReturnEmpty_WhenNoMatchFound(string query)
            {
                var service = new EncyclopediaService();

                var results = service.SearchAndTrack(query, "", "", "", "");

                if (string.IsNullOrEmpty(query)) Assert.NotEmpty(results);
                else Assert.Empty(results);
            }
        [Fact]
        public void Search_IsCaseInsensitive()
        {
            var service = new EncyclopediaService();
            var title = service.AllComics.First().Title;

            var results = service.SearchAndTrack(title.ToUpper(), "", "", "", "");

            Assert.Contains(results, b => b.Title == title);
        }

        [Fact]
        public void Search_ReturnsMaximumOf100Results()
        {
            var service = new EncyclopediaService();

            var results = service.SearchAndTrack("", "", "", "", "");

            Assert.True(results.Count <= 100, "Search should be limited to 100 for performance.");
        }

        [Fact]
        public void Flagging_DoesNotCrash_IfTitleDoesNotExist()
        {
            var service = new EncyclopediaService();

            var exception = Record.Exception(() => service.FlagRecord("NonExistentTitle123"));
            Assert.Null(exception);
        }
    }
    }


