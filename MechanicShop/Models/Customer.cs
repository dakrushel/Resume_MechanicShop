
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MechanicShop.Models
{
    public class Customer
    {
        [Required]
        [PrimaryKey]
        public string CustomerPhone { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        //Provides access to Database
        MechanicShopSQLite ShopDB = new MechanicShopSQLite();

        //All vehicles attached to customer will be affected by changes to customer class

        [OneToMany(CascadeOperations = CascadeOperation.All)]

        public List<Vehicle> CustomerVehicles { get; set; } 

        public Customer(string name, string address, string phone)
        {
            // inputs MUST ve validated from front end BEFORE they get here OR ELSE!!!!
            this.Name = name;
            this.Address = address;
            // Phone has to be unique as it is PK, validate in front end
            this.CustomerPhone = phone;
            ShopDB.AddCustomer(this);
        }

        public Customer() { }

        //Provides access to Database
        public string GetName()
        {
            return this.Name;
        }

        //Method to create and add vehicle to customer
        public void AddVehicle(string VIN, string make, string model, string Colour, int year)
        {   
            //Initialize CustomerVehicle is none made yet
            if (CustomerVehicles == null)
            {
                CustomerVehicles = new List<Vehicle>();
            }
            //Construct vehicle with method inputs
            Vehicle newCustomerVehicle = new Vehicle(VIN, make, model, Colour, year, this.CustomerPhone);
            
            //Add newly constructed vehicle to customers lists of vehicles and to database
            ShopDB.AddVehicle(newCustomerVehicle);
            CustomerVehicles.Add(newCustomerVehicle);
            ShopDB.UpdateCustomer(this);
        }




    }
}
