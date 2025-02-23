using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.src.Hotel.Core.Entities
{
    class Customer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string EMAIL { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string PASSWORD { get; set; }

        // Constructor sin parámetros, necesario para EF Core
        public Customer() { }
        public Customer(int id, string email, string name, string password)
        {
            ID = id;
            EMAIL = email;
            NAME = name;
            PASSWORD = password;
        }
        

    }
}
