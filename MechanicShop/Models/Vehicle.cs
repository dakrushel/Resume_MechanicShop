using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace MechanicShop.Models
{
    [Table("Vehicle")]
    public class Vehicle
    {

        //Data annotations for the databasse creation
        //In Vehicle VIN is the Primary Key
        //All other properties cannot be null 
        [PrimaryKey, NotNull]
        public string VIN { get; set; }

        [NotNull]
        public string Make { get; set; }

        [NotNull]
        public string Model { get; set; }

        [NotNull]
        public string Colour { get; set; }

        [NotNull]
        public int Year { get; set; }

        [NotNull]
        [ForeignKey(typeof(Customer))]
        public string CustomerPhone { get; set; }


        public Vehicle (string VIN, string Make, string Model, string Colour, int Year, string CustomerPhone)
        {
            this.VIN = VIN;
            this.Make = Make;
            this.Model = Model;
            this.Colour = Colour;
            this.Year = Year;
            this.CustomerPhone = CustomerPhone;
        }

        public Vehicle() { }
    }
}
