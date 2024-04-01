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
        public string ServiceJobId { get; set; }

        public string ServiceJobDescription { get; set; }

        public double ServiceJobHours { get; set; }


        public ServiceJob(string jobId, string jobDescription, double hours)
        {
            this.ServiceJobId = jobId;
            this.ServiceJobDescription = jobDescription;
            this.ServiceJobHours = hours;
              
        }

        public ServiceJob() { }



    }
}
