using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class Customer
    {
        private string Phone { get; set; }
        // phone will be PK, cannot have 2 accounts with same number
        private string Name {  get; set; }
        private string Address { get; set; }
        
        private List<Vehicle> CustomerVehicles { get; set; }


        public Customer(string name, string address, string phone)
        {
            // inputs MUST ve validated from front end BEFORE they get here OR ELSE!!!!
            this.Name = name;
            this.Address = address;
            // Phone has to be unique as it is PK, validate in front end
            this.Phone = phone;
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
