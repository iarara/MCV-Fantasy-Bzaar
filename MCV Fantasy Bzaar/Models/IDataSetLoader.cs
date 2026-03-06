namespace MCV_Fantasy_Bzaar.Models
{
    public interface IDataSetLoader
    {
        IEnumerable<BookDetails> LoadData();
        Task<IEnumerable<BookDetails>> LoadDataAsync();
    }
}
