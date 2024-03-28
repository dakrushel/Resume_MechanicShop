using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        [PrimaryKey, NotNull]
        public string RepairOrderId { get; set; }

        [Required, NotNull]
        //Foreign key to VIN table
        public string VIN { get; set; }

        //Connects this work order with a vehicle based on the VIN
        [ForeignKey(nameof(VIN))]
        public Vehicle Vehicle { get; set; }

        [Required]
        //Foreign key to Employee table
        public string EmployeeId { get; set; }

        //Connect this work order with an employee based on the VIN
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        [Required]
        public string ProblemDescription { get; set; }

        [Required, NotNull]
        public string DateCreated {  get; set; }

        [Required, NotNull]
        public string DateAppointment { get; set; }

        [Required]
        public string DateClosed { get; set; }
        [Required]
        public int hours { get; set; }








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
