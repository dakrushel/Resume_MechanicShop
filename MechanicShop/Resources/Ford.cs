using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Resources
{
    public class Ford
    {
        public string Model { get; set; }

        public Ford (string model)
        { 
            this.Model = model;
        }

        public Ford () { }

    }
}
