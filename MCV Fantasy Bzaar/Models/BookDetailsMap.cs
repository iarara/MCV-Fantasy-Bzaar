using CsvHelper.Configuration;

namespace MCV_Fantasy_Bzaar.Models
{
    public class BookDetailsMap : ClassMap<BookDetails>
    {
        public BookDetailsMap()
        {
            Map(p => p.Title).Name("Title");
            Map(p => p.Othertitles).Name("Other titles");
            Map(p => p.AuthorName).Name("Name");
            Map(p => p.Genre).Name("Genre");
            Map(p => p.DateOfPublication).Name("Date of publication");
            Map(p => p.ISBN).Name("ISBN");
            Map(p => p.Languages).Name("Languages");
        }
    }
}
