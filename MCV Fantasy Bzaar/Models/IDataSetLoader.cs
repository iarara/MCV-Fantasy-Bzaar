namespace MCV_Fantasy_Bzaar.Models
{
    public interface IDataSetLoader
    {
        //This is the interface for loading the dataset. It defines two methods: LoadData and LoadDataAsync
        IEnumerable<BookDetails> LoadData();
        Task<IEnumerable<BookDetails>> LoadDataAsync();
    }
}
