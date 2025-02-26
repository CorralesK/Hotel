using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hotel.src.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.src.Core.Entities
{
    [Index(nameof(EMAIL), IsUnique = true)] // Unique_key
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [EmailAddress] // Validation of email
        public string EMAIL { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        [Required]
        public RoleUser ROLE { get; set; }

        // Constructor sin parámetros, necesario para EF Core
        public User() { }
        public User(int id, string email, string name, string password)
        {
            ID = id;
            EMAIL = email;
            NAME = name;
            PASSWORD = password;
        }
        

    }
}
