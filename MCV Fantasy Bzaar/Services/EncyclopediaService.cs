using MCV_Fantasy_Bzaar.Models;
using System.Linq;

namespace MCV_Fantasy_Bzaar.Services
{
    public class EncyclopediaService
    {
        private readonly CSVDataLoader _loader;
        public List<BookDetails> AllComics { get; private set; }
        public Dictionary<string, int> SearchQueriesCounter { get; private set; } = new();
        public Dictionary<string, int> ComicAppearanceCounter { get; private set; } = new();

        public EncyclopediaService(string csvPath)
        {
            _loader = new CSVDataLoader(csvPath);
            LoadData();
        }

        private void LoadData()
        {
            var rawData = _loader.LoadData().ToList();
    
            AllComics = rawData.GroupBy(c => c.Title)
                .Select(g => new BookDetails
                {
                    Title = g.Key,
                    Othertitles = string.Join("; ", g.Select(x => x.Othertitles).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()),
                    AuthorName = string.Join("; ", g.Select(x => x.AuthorName).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()),
                    Genre = string.Join("; ", g.Select(x => x.Genre).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()),
                    DateOfPublication = string.Join("; ", g.Select(x => x.DateOfPublication).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()),
                    Languages = string.Join("; ", g.Select(x => x.Languages).Where(l => !string.IsNullOrWhiteSpace(l)).SelectMany(l => l.Split(';')).Select(l => l.Trim()).Distinct()),
                    ISBN = string.Join("; ", g.Select(x => string.IsNullOrWhiteSpace(x.ISBN) ? "missing" : x.ISBN).Distinct())
                }).ToList();
        }

        public List<BookDetails> SearchAndTrack(string query, string author, string year, string genre, string lang)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                string key = query.ToLower().Trim();
                SearchQueriesCounter[key] = SearchQueriesCounter.GetValueOrDefault(key) + 1;
            }

            var results = AllComics.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query))
                results = results.Where(c => c.Title != null && c.Title.Contains(query, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(author))
                results = results.Where(c => c.AuthorName != null && c.AuthorName.Contains(author, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(year))
                results = results.Where(c => c.DateOfPublication != null && c.DateOfPublication.Contains(year, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(genre))
                results = results.Where(c => c.Genre != null && c.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(lang))
                results = results.Where(c => c.Languages != null && c.Languages.Contains(lang, StringComparison.OrdinalIgnoreCase));

            var finalResults = results.ToList();

            foreach (var book in finalResults)
            {
                ComicAppearanceCounter[book.Title] = ComicAppearanceCounter.GetValueOrDefault(book.Title) + 1;
            }

            return finalResults;
        }

        public void FlagRecord(string title)
        {
            var book = AllComics.FirstOrDefault(b => b.Title == title);
            if (book != null) book.IsFlagged = true;
        }
    }
}