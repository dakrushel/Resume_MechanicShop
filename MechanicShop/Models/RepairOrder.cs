using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;

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

        //Primary key is Repair Order ID
        [PrimaryKey, NotNull]
        public string RepairOrderId { get; set; }

        public string RepairOrderDescription { get; set; }

        public string DateCreated { get; set; }
        public string AppointmentDate {  get; set; }

        public string DateClose { get; set; }

        public int hours { get; set; }

        //Property to act as foreign key
        [ForeignKey(typeof(Vehicle))]
        public string VIN {  get; set; }
        [OneToOne]
        //retrieves object reference from foreign key
        public Vehicle Vehicle { get; set; }

        [ForeignKey(typeof(Employee))]
        //Property to act as foreign key 
        public string EmployeeId { get; set; }
        [OneToOne]
        //retrieves object reference from foreign key
        public Employee Employee { get; set; }

        //Property to act as foreign key
        [ForeignKey(typeof(ServiceJob))]
        public string JobId { get; set; }

        //identifies one to many relationship
        //Sets any changes made to the one will affect the many
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ServiceJob> RepairOrderServiceJobs { get; set; }

        

        public RepairOrder(string repairOrderId, string description, string dateCreated, string appointmentDate, string dateClose, int hours, string VIN, string employeeId)
        {
            this.RepairOrderId = repairOrderId;
            this.RepairOrderDescription = description;
            this.DateCreated = dateCreated;
            this.AppointmentDate = appointmentDate;
            this.DateClose = dateClose;
            this.hours = hours;
            this.VIN = VIN;
            this.EmployeeId = employeeId;
        }


        public RepairOrder() { }



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
