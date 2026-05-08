using CsvHelper.Configuration;

namespace MCV_Fantasy_Bzaar.Models
{
    public sealed class BookDetailsMap : ClassMap<BookDetails>
    {
        public BookDetailsMap()
        {
            // Here I used CsvHelper to create a mapping between the columns in the CSV file and the properties of the BookDetails model,
            // so that I can easily read from the data
            Map(m => m.Title).Index(0);
            Map(m => m.Othertitles).Index(1);
            Map(m => m.ISBN).Index(7);
            Map(m => m.AuthorName).Index(8);
            Map(m => m.DateOfPublication).Index(18);
            Map(m => m.Genre).Index(24);
            Map(m => m.Languages).Index(25);
        }
    }
}