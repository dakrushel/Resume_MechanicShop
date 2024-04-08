using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class ShopSettings
    {
        [PrimaryKey]
        public double ShopHourlyRate {  get; set; }

        public double ShopSupplyCost {  get; set; }   

        public ShopSettings () { }


    }
}
