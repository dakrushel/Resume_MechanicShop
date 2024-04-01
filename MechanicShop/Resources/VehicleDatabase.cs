using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Resources
{
    public class VehicleDatabase
    {
        public string VehicleMake {  get; set; }

        [PrimaryKey]
        public string VehicleModel { get; set; }

        public VehicleDatabase(string VehicleMake, string VehicleModel)
        {
            this.VehicleMake = VehicleMake;
            this.VehicleModel = VehicleModel;
        }

        public VehicleDatabase() { }    

    }
}
