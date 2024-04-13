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
        //Annotation for DB creation
        [PrimaryKey, NotNull]
        public int ServiceJobId { get; set; }

        public string ServiceJobDescription { get; set; }

        public double ServiceJobHours { get; set; }

        //ctor
        public ServiceJob(string jobId, string jobDescription, double hours)
        {
            Random random = new Random();

            //Auto generate serviceJobId
            int serviceJobId = random.Next(1000);
            //Make sure serviceJobId is unique
            while (MauiProgram.ShopDB.GetAllServiceJobs().FirstOrDefault(x => x.ServiceJobId == serviceJobId) != default)
            {
                serviceJobId = random.Next(1000);
            }
            //Straight forward ctor from here
            this.ServiceJobId = serviceJobId;
            this.ServiceJobDescription = jobDescription;
            this.ServiceJobHours = hours;   
        }

        public ServiceJob() { }



    }
}
