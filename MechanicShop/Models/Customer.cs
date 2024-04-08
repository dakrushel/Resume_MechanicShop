
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
            // inputs MUST be validated from front end BEFORE they get here OR ELSE!!!!
            this.Name = name;
            this.Address = address;
            // Phone has to be unique as it is PK, validate in front end
            this.CustomerPhone = phone;
            MauiProgram.ShopDB.AddCustomer(this);
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
            MauiProgram.ShopDB.AddVehicle(newCustomerVehicle);
            CustomerVehicles.Add(newCustomerVehicle);
            MauiProgram.ShopDB.UpdateCustomer(this);
        }

        //RemoveCustomer method must check if customer has an active appointment or repair order before removal from database
        public bool RemoveCustomerChecker(string phoneNum) //Denver
        {
            //If customer with phone# of phoneNum has a scheduled appointment or active Repair Order, return false

            //Variables:
            List<RepairOrder> customerROs = new List<RepairOrder>();
            List<Vehicle> custVehicles = MauiProgram.ShopDB.GetCustomerVehicles(phoneNum);

            if (custVehicles.Count > 0)
            {
                List<RepairOrder> tempRO = new List<RepairOrder>();
                foreach (Vehicle v in custVehicles)
                {
                    //tempRO.AddRange(MauiProgram.ShopDB.GetRepairOrderByVIN(v.VIN));
                }
                foreach (RepairOrder ro in tempRO)
                {
                    if (ro.IsActive && ro.DateClose == null)
                    {
                        customerROs.Add(ro);
                    }
                }
                if (customerROs.Count > 0) { return false; }

                else
                {
                    //MauiProgram.ShopDB.RemoveCustomer(phoneNum);
                    return true;
                }
            }
            else { return true; }
        }
    }
}
