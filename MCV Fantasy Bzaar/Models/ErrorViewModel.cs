namespace MCV_Fantasy_Bzaar.Models
{
    public class ErrorViewModel
    {
        // This is the model for the error view, which will display the request ID and a message if there is an error in the application
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
