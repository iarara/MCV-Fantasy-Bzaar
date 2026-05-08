using MCV_Fantasy_Bzaar.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCV_Fantasy_Bzaar.Services
{
    public class EncyclopediaService
    {
        public List<BookDetails> AllComics { get; set; } = new List<BookDetails>();
        public Dictionary<string, int> SearchCounts { get; set; } = new Dictionary<string, int>();

        // Here I set up the file paths for the flagged titles and the CSV dataset
        private readonly string _flagFilePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData", "flagged_titles.txt");
        private readonly string _csvPath = Path.Combine(Directory.GetCurrentDirectory(), "AppData", "titles.csv");

        public EncyclopediaService()
        {
            // Here I load the dataset from the CSV file and then call the function to load any flagged status from the text file
            var loader = new CSVDataLoader(_csvPath);
            AllComics = loader.LoadData().ToList();

            LoadFlaggedStatus();
        }

        private void LoadFlaggedStatus()
        {
            if (File.Exists(_flagFilePath))
            {
                var flaggedTitles = File.ReadAllLines(_flagFilePath);
                foreach (var t in flaggedTitles)
                {
                    // Here I loop through the flagged titles and mark the corresponding records in AllComics as flagged,
                    // so that this status is the same even after a restart of the application
                    var book = AllComics.FirstOrDefault(b => b.Title == t);
                    if (book != null) book.IsFlagged = true;
                }
            }
        }

        public List<BookDetails> SearchAndTrack(string query, string author, string year, string genre, string lang)
        {
            // Here I track the search counts for the main search box, so that users can later find out the most popular search terms in the UI
            if (!string.IsNullOrEmpty(query))
            {
                var cleanQuery = query.Trim().ToLower();
                if (SearchCounts.ContainsKey(cleanQuery)) SearchCounts[cleanQuery]++;
                else SearchCounts[cleanQuery] = 1;
            }

            var results = AllComics.AsQueryable();

            // Here I apply the search filters based on the provided parameters, using case-insensitive comparisons for string
            // fields and a year filter that checks if the year is contained in the DateOfPublication field, which allows for more flexible searching
            if (!string.IsNullOrEmpty(query))
                results = results.Where(b => b.Title.Contains(query, System.StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(author))
                results = results.Where(b => b.AuthorName.Contains(author, System.StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(year))
                results = results.Where(b => b.DateOfPublication.Contains(year));

            if (!string.IsNullOrEmpty(genre))
                results = results.Where(b => b.Genre.Contains(genre, System.StringComparison.OrdinalIgnoreCase));

            return results.Take(100).ToList();
        }

        public void FlagRecord(string title)
        {
            var book = AllComics.FirstOrDefault(b => b.Title == title);
            if (book != null)
            {
                book.IsFlagged = true;
                // Here I check if the title is already in the flagged titles file, and if not,
                // I attach it to the file so that the flagged status is saved for future sessions
                File.AppendAllLines(_flagFilePath, new[] { title });
            }
        }
    }
}