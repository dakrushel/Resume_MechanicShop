using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class Customer
    {
        [Required]
        [PrimaryKey]
        public string CustomerPhone { get; set; }
        // phone will be PK, cannot have 2 accounts with same number

        [Required]
        public string Name {  get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List <Vehicle> CustomerVehicles { get; set; }


        public Customer(string name, string address, string phone)
        {
            // inputs MUST ve validated from front end BEFORE they get here OR ELSE!!!!
            this.Name = name;
            this.Address = address;
            // Phone has to be unique as it is PK, validate in front end
            this.CustomerPhone = phone;
        }

        public string GetName()
        {
            return this.Name;
        }

        public void AddVehicle()
        {
            // construct new VEHICLE OBJ
            // ADD to list for this customer
        }

    }
}
