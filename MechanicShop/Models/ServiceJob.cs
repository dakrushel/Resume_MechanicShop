using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class ServiceJob
    {

        [PrimaryKey, NotNull]
        public int ServiceJobId { get; set; }

        public string ServiceJobDescription { get; set; }

        public double ServiceJobHours { get; set; }

        //Opens connection to database
        MechanicShopSQLite ShopDB = new MechanicShopSQLite();

        public ServiceJob(string jobId, string jobDescription, double hours)
        {
            Random random = new Random();

            int serviceJobId = random.Next(1000);
            while (ShopDB.GetAllServiceJobs().FirstOrDefault(x => x.ServiceJobId == serviceJobId) != default)
            {
                serviceJobId = random.Next(1000);
            }
            this.ServiceJobId = serviceJobId;
            this.ServiceJobDescription = jobDescription;
            this.ServiceJobHours = hours;
              
        }

        public ServiceJob() { }



    }
}
