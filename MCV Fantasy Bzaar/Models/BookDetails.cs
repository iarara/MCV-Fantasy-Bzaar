namespace MCV_Fantasy_Bzaar.Models
{
    public class BookDetails
    {
        // Here I set up the properties for the BookDetails model, which will be used to store and display all the relevant information
        // about each book in the encyclopedia
        // I also added a boolean property 'IsFlagged' to track whether a record has been flagged by staff for review
        public string Title { get; set; } = "";
        public string Othertitles { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string Genre { get; set; } = "";
        public string DateOfPublication { get; set; } = "";
        public string ISBN { get; set; } = "";
        public string Languages { get; set; } = "";
        public bool IsFlagged { get; set; } = false;
    }
}