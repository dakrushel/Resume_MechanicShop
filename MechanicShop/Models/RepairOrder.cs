using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class RepairOrder
    {
        //NOTE, An INACTIVE repair order is an APPOINTMENT!
        // NOTE CAN ONLY CREATE APPOINTMENT (INACTIVE)
        // CAN ACTIVATE INTO RO MODE

        /// ATTRIBUTES
        /// UNIQUE PK of RO number (generated)
        /// Customer
        /// Vehicle
        /// mechanic
        /// job or jobs
        /// problem description (notes)
        /// Date created 
        /// AppointmentDate
        /// Date Activated
        /// Date Closed
        /// hours (based on job)
        
        // cost maybe will be calculate in GUI based on hours times tech payrate

        public void SetActive()
        {
            // this turns the APT into RO
        }

        public void AssignTechnician()
        {
            //assign a tech to this job
        }


    }
}
