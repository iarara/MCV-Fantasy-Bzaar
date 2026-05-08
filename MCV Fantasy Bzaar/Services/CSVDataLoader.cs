using MCV_Fantasy_Bzaar.Models;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace MCV_Fantasy_Bzaar.Services
{
    public class CSVDataLoader : IDataSetLoader
    {
        private readonly string datasetPath;
        private const int dataLimit = 5000;

        public CSVDataLoader(string datasetPath) => this.datasetPath = datasetPath;

        public IEnumerable<BookDetails> LoadData()
        {
            if (!File.Exists(datasetPath)) return new List<BookDetails>();

            // Here I set up the configuration for CsvHelper to handle the specific formatting of our dataset,
            // which includes ignoring missing fields and validating headers
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using (var reader = new StreamReader(datasetPath))
            using (var csv = new CsvReader(reader, config))
            {
                // Here I registered the class map to ensure that the CSV columns are correctly mapped to the properties of the BookDetails class
                csv.Context.RegisterClassMap<BookDetailsMap>();
                var records = csv.GetRecords<BookDetails>().Take(dataLimit).ToList();

                foreach (var row in records)
                {
                    // Here I clean the string data to ensure that it's ready to be displayed on the kiosk, removing any unwanted characters
                    // and dealing with any missing values
                    row.Title = CleanString(row.Title);
                    row.Othertitles = CleanString(row.Othertitles);
                    row.AuthorName = CleanString(row.AuthorName);
                    row.Genre = CleanString(row.Genre);
                    row.DateOfPublication = CleanString(row.DateOfPublication);
                    row.ISBN = CleanString(row.ISBN);
                }
                return records;
            }
        }

        private string CleanString(string value)
        {
            // Here I handle missing values by checking if the string is null, empty, or contains the word "missing", and return an empty string in those cases
            if (string.IsNullOrWhiteSpace(value) || value.ToLower() == "missing")
                return "";

            // Here I remove any unwanted characters that might cause issues in the display or search functionality, such as special characters
            return value.Trim().Replace("#", "").Replace("&", "").Replace("$", "")
                        .Replace("*", "").Replace("@", "").Replace("!", "")
                        .Replace("'", "").Replace("+", "").Replace(".", "");
        }

        public Task<IEnumerable<BookDetails>> LoadDataAsync() => Task.Run(() => LoadData());
    }
}