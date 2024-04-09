using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
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
        [PrimaryKey, NotNull, ForeignKey(typeof(RepairOrderServiceJobBridge)), AutoIncrement]
        public int RepairOrderId { get; set; }

        public string RepairOrderDescription { get; set; }

        
        public string DateCreated { get; set; }
        public string AppointmentDate {  get; set; }

        public string DateClose { get; set; }


        //This property is NOT in the CTOR as it will need to be calculated based on the jobs
        public double RepairOrderHours { get; set; }


        public string CustomerName {  get; set; }

        public string CustomerPhoneNumber { get; set; }

        //Property to act as foreign key
        [ForeignKey(typeof(Vehicle))]
        public string VIN {  get; set; }

        [ForeignKey(typeof(Technician))]
        //Property to act as foreign key 
        public string? EmployeeId { get; set; }

        [ForeignKey(typeof(RepairOrderServiceJobBridge))]
        public int RepairOrderServiceJobBridgeId { get; set; }


        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ServiceJob>? ListOfServiceJobs { get; set; }

        public bool IsActive { get; set; }

        //Opens connection to Database
        //CTOR: For all variabless accounted for
        public RepairOrder(string description, string dateCreated, string appointmentDate, string VIN, string employeeId)
        {

            this.RepairOrderDescription = description;
            this.DateCreated = dateCreated;
            this.AppointmentDate = appointmentDate;
            this.VIN = VIN;
            this.EmployeeId = employeeId;


            Customer customerAttached = MauiProgram.ShopDB.GetCustomerViaVIN(VIN);

            this.CustomerName = customerAttached.Name;

            this.CustomerPhoneNumber = customerAttached.CustomerPhone;

            //Automatically sets is Active to False signifying this is NOT an active appointment
            this.IsActive = false;

            this.ListOfServiceJobs = new List<ServiceJob>();

            //Adding to Database as object is created
            MauiProgram.ShopDB.AddRepairOrder(this);
        }

        //Overload CTOR for no technician assigned
        public RepairOrder(string description, string dateCreated, string appointmentDate, string VIN)
        {
   
            this.RepairOrderDescription = description;
            this.DateCreated = dateCreated;
            this.AppointmentDate = appointmentDate;
            this.VIN = VIN;
            Customer customerAttached = MauiProgram.ShopDB.GetCustomerViaVIN(VIN);

            this.CustomerName = customerAttached.Name;

            this.CustomerPhoneNumber = customerAttached.CustomerPhone;


            this.ListOfServiceJobs = new List<ServiceJob>();

            //Automatically sets is Active to False signifying this is NOT an active appointment
            this.IsActive = false;

            //Adding to Database as object is created
            MauiProgram.ShopDB.AddRepairOrder(this);
        }

        public RepairOrder() { }


        //Changes Repair order from appointment to ACTIVE repair order
        public void SetActive()
        {
            this.IsActive = true;

            //Updating to database
            MauiProgram.ShopDB.UpdateRepairOrder(this);
        }

        //Assign technician 
        public void AssignTechnician(string employeeId)
        {
            this.EmployeeId = EmployeeId;

            //Updating to database
            MauiProgram.ShopDB.UpdateRepairOrder(this);
        }

        //Assigns a service job based on serviceJobId
        public void AssignServiceJob (int serviceJobId) 
        {
            //Opens the database and finds the service job with the corresponding
            //serviceJobId and returns it, it is then added to the list for Repair Orders
/*            ServiceJob? newlyAddedServiceJob = MauiProgram.ShopDB.GetAllServiceJobs().
                Find(x => x.ServiceJobId == serviceJobId);

            if (newlyAddedServiceJob != null)
            {
                ListOfServiceJobs?.Add(newlyAddedServiceJob);
            }*/
            
            //create new entry into bridging table between service jobs and repair orders
            RepairOrderServiceJobBridge bridgeCreation = new RepairOrderServiceJobBridge(this.RepairOrderId, serviceJobId);
            MauiProgram.ShopDB.AddRepairOrderServiceJobBridge(bridgeCreation);


            //Updating to database
            MauiProgram.ShopDB.UpdateRepairOrder(this);
        }

    }
}
