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


        // cost maybe will be calculate in GUI based on hours times tech payrate

        //Primary key is Repair Order ID
        [PrimaryKey, NotNull]
        public string RepairOrderId { get; set; }

        public string RepairOrderDescription { get; set; }

        public string DateCreated { get; set; }
        public string AppointmentDate {  get; set; }

        public string DateClose { get; set; }


        //This property is NOT in the CTOR as it will need to be calculated based on the jobs
        public double RepairOrderHours { get; set; }


        //This property is NOT in the CTOR as will need to be calculated based on the jobs
        public double RepairOrderBill {  get; set; }

        //Property to act as foreign key
        [ForeignKey(typeof(Vehicle))]
        public string VIN {  get; set; }
        [OneToOne]
        //retrieves object reference from foreign key
        public Vehicle Vehicle { get; set; }

        [ForeignKey(typeof(Technician))]
        //Property to act as foreign key 
        public string EmployeeId { get; set; }
        [OneToOne]
        //retrieves object reference from foreign key
        public Technician RepairJobTechnician { get; set; }

        //Property to act as foreign key
        [ForeignKey(typeof(ServiceJob))]
        public string ServiceJobId { get; set; }

        //identifies one to many relationship
        //Sets any changes made to the one will affect the many
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ServiceJob> RepairOrderServiceJobs { get; set; }

        bool IsActive { get; set; }

        //Opens connection to Database
        MechanicShopSQLite ShopDB = new MechanicShopSQLite();


        //CTOR: For all variabless accounted for
        public RepairOrder(string repairOrderId, string description, string dateCreated, string appointmentDate, string VIN, string employeeId)
        {
            this.RepairOrderId = repairOrderId;
            this.RepairOrderDescription = description;
            this.DateCreated = dateCreated;
            this.AppointmentDate = appointmentDate;
            this.VIN = VIN;
            this.EmployeeId = employeeId;

            //Automatically sets is Active to False signifying this is NOT an active appointment
            this.IsActive = false;

        }

        //Overload CTOR for no technician assigned
        public RepairOrder(string repairOrderId, string description, string dateCreated, string appointmentDate, string VIN)
        {
            this.RepairOrderId = repairOrderId;
            this.RepairOrderDescription = description;
            this.DateCreated = dateCreated;
            this.AppointmentDate = appointmentDate;
            this.VIN = VIN;

            //Automatically sets is Active to False signifying this is NOT an active appointment
            this.IsActive = false;

        }

        public RepairOrder() { }


        //Changes Repair order from appointment to ACTIVE repair order
        public void SetActive()
        {
            this.IsActive = true;
        }

        //Assign technician 
        public void AssignTechnician(string employeeId)
        {
            this.EmployeeId = EmployeeId;
        }

        //Assigns a service job based on serviceJobId
        public void AssignServiceJob (string serviceJobId) 
        {
            //Opens the database and finds the service job with the corresponding
            //serviceJobId and returns it, it is then added to the list for Repair Orders
            ServiceJob newlyAddedServiceJob = ShopDB.GetAllServiceJobs().
                Find(x => x.ServiceJobId == serviceJobId);


            RepairOrderServiceJobs.Add(newlyAddedServiceJob);
        }

        public void SetRepairOrderHours()
        {
            //initialization for for loop
            double totalHours = 0;

            //iterates through each job and adds their hours together
            foreach (var serviceJob in RepairOrderServiceJobs)
            {
                totalHours = serviceJob.ServiceJobHours + totalHours;
            }

            this.RepairOrderHours = totalHours;
        }

        //Calculating bill for the customer
        public void CalculateBill()
        {
           
            //Retrieves the Technicians hourly rate 
            double technicianHourlyRate = RepairJobTechnician.HourlyRate;

            // calculates the cost by mulitplying total hours by the technicians hourly rate
            double calculateBill = this.RepairOrderHours * technicianHourlyRate;
            this.RepairOrderBill = calculateBill;
        }

        // Checkout or billing method 
        public void CloseAppointment(string appointmentClosedDate)
        {
            //Adds the Appointment close date
            this.DateClose = appointmentClosedDate;

            //Sets the appropriate hours on repair order based service job hours
            this.SetRepairOrderHours();

            //Calculates bill
            this.CalculateBill();

            //Changes from ACTIVE repair order to closed repair order
            this.IsActive = false; 
        }

    }
}
