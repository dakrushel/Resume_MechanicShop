using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class Customer
    {
        private string Name {  get; set; }
        private string Address { get; set; }
        private string Phone { get; set; }


        public Customer(string name, string address, string phone)
        {
            this.Name = name;
            this.Address = address;
            this.Phone = phone;
        }

        public string GerName()
        {
            return this.Name;
        }


    }
}
