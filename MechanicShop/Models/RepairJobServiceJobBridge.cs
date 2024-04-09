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
        [PrimaryKey, AutoIncrement]
        public int BridgeIdForROandSJ { get; set; }
        [NotNull]
        public int RepairOrderId { get; set; }

        [NotNull]
        public int ServiceJobId { get; set; }

        public RepairOrderServiceJobBridge(int repairOrderId, int serviceJobId)
        {
            this.RepairOrderId = repairOrderId;
            this.ServiceJobId = serviceJobId;
        }

        public RepairOrderServiceJobBridge() { }



    }
}
