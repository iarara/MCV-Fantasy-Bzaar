using MCV_Fantasy_Bzaar.Models;
using System.Globalization;
using CsvHelper;

namespace MCV_Fantasy_Bzaar.Services
{
    public class CSVDataLoader : IDataSetLoader
    {
        private readonly string datasetPath;
        private const int dataLimit = 5000
           ;

        public CSVDataLoader(string datasetPath) => this.datasetPath = datasetPath;

        public IEnumerable<BookDetails> LoadData()
        {
            if (!File.Exists(datasetPath)) return new List<BookDetails>();

            using var reader = new StreamReader(datasetPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<BookDetailsMap>();
            var records = csv.GetRecords<BookDetails>().Take(dataLimit).ToList();

            foreach (var r in records) CleanRecord(r);
            return records;
        }

        private void CleanRecord(BookDetails row)
        {
            row.Title = CleanString(row.Title);
            row.AuthorName = CleanString(row.AuthorName);
            row.ISBN = string.IsNullOrWhiteSpace(row.ISBN) ? "missing" : CleanString(row.ISBN);
        }

        private string CleanString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "missing";
            char[] unwanted = { '#', '&', '$', '%', '*', '@', '!', '+', '\'' };
            string cleaned = value.Trim();
            foreach (var c in unwanted) cleaned = cleaned.Replace(c.ToString(), "");
            return cleaned;
        }
        public bool CheckForUpdates()
        {
            return false;
        }

        public Task<IEnumerable<BookDetails>> LoadDataAsync() => Task.Run(() => LoadData());
    }
}

