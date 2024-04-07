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
        [PrimaryKey]
        public int RepairOrderServiceJobBridgeId { get; set; }

        public int RepairOrderId { get; set; }


        public int ServiceJobId { get; set; }

        //Opens connection to Database
        MechanicShopSQLite ShopDB = new MechanicShopSQLite();   

        public RepairOrderServiceJobBridge(int repairOrderId, int serviceJobId)
        {
            Random random = new Random();

            int repairOrderServiceJobId = random.Next(1000);
            while (MauiProgram.ShopDB.GetAllRepairOrders().FirstOrDefault(x => x.RepairOrderId == repairOrderId) != default)
            {
                repairOrderServiceJobId = random.Next(1000);
            }

            this.RepairOrderServiceJobBridgeId = repairOrderServiceJobId;
            this.RepairOrderId = repairOrderId;
            this.ServiceJobId = serviceJobId;
        }

        public RepairOrderServiceJobBridge() { }



    }
}
