using SQLite;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MechanicShop.Models
{
    public class Customer
    {
        [Required]
        [PrimaryKey]
        public string CustomerPhone { get; set; }
        // phone will be PK, cannot have 2 accounts with same number


        public string Name { get; set; }

        public string Address { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Vehicle> CustomerVehicles { get; set; }


        public Customer(string name, string address, string phone)
        {
            // inputs MUST ve validated from front end BEFORE they get here OR ELSE!!!!
            this.Name = name;
            this.Address = address;
            // Phone has to be unique as it is PK, validate in front end
            this.CustomerPhone = phone;
        }

        public Customer() { }   

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
