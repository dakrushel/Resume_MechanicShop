using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class RepairOrderServiceJobBridge
    {
        [Required]
        [PrimaryKey]
        public int RepairOrderId { get; set; }
        [Required]
        [PrimaryKey]
        public int ServiceJobId { get; set; }

        //Opens connection to Database
        MechanicShopSQLite ShopDB = new MechanicShopSQLite();   

        public RepairOrderServiceJobBridge(int repairOrderId, int serviceJobId)
        {
            this.RepairOrderId = repairOrderId;
            this.ServiceJobId = serviceJobId;
        }

        public RepairOrderServiceJobBridge() { }



    }
}
