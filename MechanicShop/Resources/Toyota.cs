using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Resources
{
    public class Toyota
    {
        //For GUI

        public string Model { get; set; }

        public Toyota(string model) 
        { 
            this.Model = model; 
        } 

        public Toyota() { }


    }
}
