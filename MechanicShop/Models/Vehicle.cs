using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class Vehicle
    {

        //Data annotations for the databasse creation
        //In Vehicle VIN is the Primary Key
        //All other properties cannot be null 
        [Required]
        [PrimaryKey]
        public string VIN { get; set; }
        // VIN WILL BE THE PK
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Colour { get; set; }
        [Required]
        public int Year { get; set; }

        //Customerphone is the foreign key for the table Customer
        [Required]
        public string CustomerPhone { get; set; }

        //Navigation for the database to link to the appropriate table
        [ForeignKey(nameof(CustomerPhone))]
        public Customer Customer { get; set; }

        public RepairOrder RepairOrder { get; set; }





        //constructor goes here
        // input validations will be done on front end
    }
}
