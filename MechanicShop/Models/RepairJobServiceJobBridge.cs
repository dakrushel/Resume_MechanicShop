using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class RepairOrderServiceJobBridge
    {
        [PrimaryKey, NotNull]
        public int RepairJobServiceJobBridgeId {  get; set; }

        public int RepairOrderId { get; set; }

        public int ServiceJobId { get; set; }

        //Opens connection to Database
        MechanicShopSQLite ShopDB = new MechanicShopSQLite();   

        public RepairOrderServiceJobBridge(int repairOrderId, int serviceJobId)
        {
            Random random = new Random();

            int RepairJobServiceJobBridgeId = random.Next(1000);
            while (ShopDB.GetAllRepairOrderServiceJobBridge().FirstOrDefault(x => x.RepairOrderId == repairOrderId) != default)
            {
                repairOrderId = random.Next(1000);
            }
            this.RepairOrderId = repairOrderId;
            this.ServiceJobId = serviceJobId;
        }

        public RepairOrderServiceJobBridge() { }



    }
}
