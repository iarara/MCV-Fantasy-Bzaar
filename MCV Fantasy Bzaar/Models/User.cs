using System.ComponentModel.DataAnnotations;

namespace MCV_Fantasy_Bzaar.Models
{
    public class User
    {
        // This is the model for the User, which will be used to store the username and password for each user in the database

        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
