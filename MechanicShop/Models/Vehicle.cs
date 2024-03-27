using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class Vehicle
    {
        
        private string VIN {  get; set; }
        // VIN WILL BE THE PK

        private string Make { get; set; }
        private string Model { get; set; }
        private string Colour { get; set; }
        private int Year { get; set; }

        //constructor goes here
        // input validations will be done on front end
    }
}
