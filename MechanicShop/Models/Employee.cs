using SQLite;
using System.ComponentModel.DataAnnotations;

namespace MechanicShop.Models
{
    public class Employee
    {
        //PK IS ID

        [Required]
        [PrimaryKey]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string HireDate { get; set; }

    }
}
