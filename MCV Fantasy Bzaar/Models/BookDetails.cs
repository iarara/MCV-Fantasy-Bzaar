namespace MCV_Fantasy_Bzaar.Models
{
    public class BookDetails
    {
        public string Title { get; set; }
        public string Othertitles { get; set; }
        public string AuthorName { get; set; }
        public string Genre { get; set; }
        public string DateOfPublication { get; set; }
        public string ISBN { get; set; }
        public string Languages { get; set; }
        public bool IsFlagged { get; set; } = false;
    }
}