
using MechanicShop.Resources;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace MechanicShop.Models
{
    public class MechanicShopSQLite
    {
        //property for connection with database 
        private SQLiteConnection database;

        public MechanicShopSQLite()
        {
            
            // Creating connection with database
            this.database = new SQLiteConnection(Constant.DatabasePath);

            //Turning foreign keys on in the database
            this.database.Execute("PRAGMA foreign_keys = ON;");


            //Try creating this table
            try
            {
                this.database.CreateTable<Customer>();
            }
            //Catch statement for any tabless that have been created
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<RepairOrder>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<Vehicle>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<Technician>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<ServiceJob>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<VehicleDatabase>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<RepairOrderServiceJobBridge>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<ShopSettings>();
            }
            //Catch statement for any tabless that have been created
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

            //Save changes
            this.database.Commit();

        }

        //====================================================================================================
        // CUSTOMER -------------------------------
        //====================================================================================================

        //Creating Customer
        public void AddCustomer(Customer customer)
        {
            this.database.Insert(customer);
        }

        //Removing Customer using primary key, also removes all vehcles attached to customer
        public void RemoveCustomer(string customerPhoneNumber)
        {

            //retrieve customer for reference in Foreach loop
            Customer customer = this.database.Get<Customer>(customerPhoneNumber);

            //goes through each vehicle in the database
            foreach (Vehicle vehicle in this.database.Table<Vehicle>().ToList())
            {
                //If vehicle has foreign key of CustomerPhone Delete vehicle
                if (vehicle.CustomerPhone == customer.CustomerPhone) 
                {
                    this.database.Delete<Vehicle>(vehicle.VIN); 
                }
            }
            this.database.Delete<Customer>(customerPhoneNumber);
        }

        //returns a list of all customers in Database
        public List<Customer> GetAllCustomers()
        {
            return this.database.Table<Customer>().ToList();
        }


        //updates customer object in database
        public void UpdateCustomer(Customer customer)
        {
            this.database.Update(customer);
        }

        //Creates a list that finds all vehicles attached to the inputted
        //customer phone number
        public List<Vehicle> GetCustomerVehicles(string CustomerPhone)
        {
            return this.database.Table<Vehicle>().ToList()
                .Where(x => x.CustomerPhone == CustomerPhone).ToList();
        }

        //gets list of customers by customer name
        public List<Customer> GetCustomerByName(string customerName)
        {
            return this.database.Table<Customer>()
                .Where(x => x.Name.ToLower().Contains(customerName.ToLower()))
                .ToList();
        }

        //gets list of customers by phone number 
        public List<Customer> GetCustomerByPhone(string customerPhone)
        {
            return this.database.Table<Customer>()
                .Where(x => x.CustomerPhone.ToLower().Contains(customerPhone.ToLower()))
                .ToList();
        }

        //returns a single customer based on name
        public Customer GetACustomerByName(string customerName)
        {
            return this.database.Table<Customer>().FirstOrDefault(x => x.Name == customerName);
        }

        //returns a single customer by phone
        public Customer GetACustomerByPhone(string customerPhone)
        {
            return this.database.Table<Customer>().FirstOrDefault(x => x.CustomerPhone == customerPhone);
        }


        //====================================================================================================
        // Vehicles-------------------------------
        //====================================================================================================

        //Add a vehicle
        public void AddVehicle(Vehicle vehicle)
        {
            this.database.Insert(vehicle);
        }

        //Update vehicle to database
        public void UpdateVehicle(Vehicle vehicle)
        {
            this.database.Update(vehicle);
        }

        //Remove vehcle using primary key
        public void RemoveVehicle(string VIN)
        {
            this.database.Delete<Vehicle>(VIN);
        }

        //returns list of all vehicles
        public List<Vehicle> GetAllVehicles()
        {
            return this.database.Table<Vehicle>().ToList();
        }

        //TODO: For Logic layer this method would need checks to ensure that repairOrderId is valid
        //returns customer via vehicle VIN
        public Customer GetCustomerViaVIN(string VIN)
        {
            //finds target vehicle based on VIN
            Vehicle targetVehicle = this.database.Table<Vehicle>().First(x => x.VIN == VIN);

            //finds customer based on the phone number attached to vehicle
            Customer customer = this.database.Table<Customer>().First(x => x.CustomerPhone == targetVehicle.CustomerPhone);

            return customer;
        }

        //Returns a single vehicle based on VIN
        public Vehicle GetVehicleByVIN(string VIN)
        {
            return this.database.Table<Vehicle>().First(x => x.VIN == VIN); 
        }


        //====================================================================================================
        // Technician-------------------------------
        //====================================================================================================

        //Creates Technician
        public void AddTechnician(Technician technician)
        {
            this.database.Insert(technician);
        }

        //updates Technician to database
        public void UpdateTechnician(Technician technician)
        {
            this.database.Update(technician);
        }

        //removes technician based on primary key
        public void RemoveTechnician(int employeeId)
        {
            this.database.Delete<Technician>(employeeId);
        }

        //returns list of ALL technicians in database
        public List<Technician> GetAllTechnicians()
        {
            return this.database.Table<Technician>().ToList();
        }

        //returns single technician using primary key
        public Technician GetTechnician(int? employeeId) //Denver
        {
            return this.database.Table<Technician>().FirstOrDefault(x => x.EmployeeId == employeeId);
        }

        //====================================================================================================
        // Repair Order-------------------------------
        //====================================================================================================

        //Creates repair order
        public void AddRepairOrder(RepairOrder repairOrder)
        {
            this.database.Insert(repairOrder);
        }

        //removes a repair order based on primary key
        //also removes all RepairOrderServiceJobBridge tied with repair order
        public void RemoveRepairOrder(int repairOrderId)
        {
            RepairOrder repairOrder = this.database.Get<RepairOrder>(repairOrderId);

            //For loop to delete each RepairOrderServiceJob attached to the repair order
            foreach (RepairOrderServiceJobBridge bridge in this.GetAllRepairOrderServiceJobBridge())
            {
                //if the repair order id matches with the bridge repair order, deletes bridge
                if (bridge.RepairOrderId == repairOrder.RepairOrderId)
                {
                    this.database.Delete<RepairOrderServiceJobBridge>(bridge.BridgeIdForROandSJ);
                }
            }
            this.database.Delete<RepairOrder>(repairOrderId);
        }


        //Removes all RepairOrderServiceJobBridge rows associated with the repair order without deleting the repair order
        public void RemoveRepairOrderServiceJobBridgeAttachedToRepairOrder(int repairOrderId)
        {
            //goes through each bridge item and deletes rows with the same repairOrderId
            foreach (RepairOrderServiceJobBridge var in this.database.Table<RepairOrderServiceJobBridge>().ToList())
            {
                if (var.RepairOrderId == repairOrderId)
                {
                    this.database.Delete<RepairOrderServiceJobBridge>(var.BridgeIdForROandSJ);
                }
            }
        }

        //Update repair order
        public void UpdateRepairOrder(RepairOrder repairOrder)
        {
            this.database.Update(repairOrder);
        }

        //returns list of repair orders
        public List<RepairOrder> GetAllRepairOrders()
        {
            return this.database.Table<RepairOrder>().ToList();
        }

        //Returns list of repair orders that are not active
        public List<RepairOrder> GetRepairOrdersThatAreNOTActive()
        {
            return this.database.Table<RepairOrder>()
                .Where(x => x.IsActive == false).ToList();
        }

        //returns lisst of repair orders that are active
        public List<RepairOrder> GetRepairOrdersThatAreActive()
        {
            return this.database.Table<RepairOrder>()
                .Where(x => x.IsActive == true).ToList();
        }

        //returns repair order by VIN
        public RepairOrder GetRepairOrderByVIN(string VIN)
        {
            return this.database.Table<RepairOrder>().First(x => x.VIN == VIN);
        }

        //returns repair order by ID
        public RepairOrder GetRepairOrderByPK(int PK)
        {
            return this.database.Table<RepairOrder>().First(x => x.RepairOrderId == PK);
        }

        public List<RepairOrder> GetRepairOrdersByVIN(string vin) //Denver
        {
            return this.database.Table<RepairOrder>().ToList()
                .Where(x => x.VIN == vin).ToList();
        }

/// <summary>
///        //TESTED
/// </summary>
/// <param name="customerPhone"></param>
/// <returns></returns>
        public List<RepairOrder> GetRepairOrdersByCustPhone(string customerPhone) //Denver
        {
            return this.database.Table<RepairOrder>()
                .Where(x => x.CustomerPhoneNumber.ToLower().Contains(customerPhone.ToLower()))
                .ToList();
        }

        //TESTED
        public List<RepairOrder> GetRepairOrdersByCustName(string customerName)//Denver
        {
            return this.database.Table<RepairOrder>()
                .Where(x => x.CustomerName.ToLower().Contains(customerName.ToLower()))
                .ToList();
        }

        //TESTED
        public List<RepairOrder> GetRepairOrdersByTechnician(int employeeId)
        {
            //Same as above but with employee ID (for the RemoveTechChecker)
            //Arg is an int so no need for partial matches or case sensitivity
            return this.database.Table<RepairOrder>().Where(x => x.EmployeeId == employeeId).ToList();
        }

        /*----------------------------SERVICE JOB ------------------------------------*/

        //Add service job
        public void AddServiceJob(ServiceJob job)
        {
            this.database.Insert(job);
        }

        //update service job
        public void UpdateServiceJob(ServiceJob job)
        {
            this.database.Update(job);
        }

        //delete service job based on primary key
        public void RemoveServiceJob(string serviceJobId)
        {
            this.database.Delete<ServiceJob>(serviceJobId);
        }

        //Gets all service jobs
        public List<ServiceJob> GetAllServiceJobs()
        {
            return this.database.Table<ServiceJob>().ToList();
        }

        //====================================================================================================
        // Vehicle Database-------------------------------
        //====================================================================================================

        //adds vehicledatabase
        public void AddVehicleDatabse(VehicleDatabase vehicle)
        {
            this.database.Insert(vehicle);
        }

        //updates vehicle database
        public void UpdateVehicleDatabase(VehicleDatabase vehicle)
        { 
            this.database.Update(vehicle); 
        }

        //removes vehicle database based on VehicleModel (unique)
        public void RemoveVehicleDatabase(string VehicleModel)
        {
            this.database.Delete<VehicleDatabase>(VehicleModel);
        }

        //returns list of vehicle database
        public List<VehicleDatabase> GetAlLVehicleDatabase()
        {
            return this.database.Table<VehicleDatabase>().ToList();
        }
        
        //Returns a list of vehicles based on make 
        public List<VehicleDatabase> GetAllVehicleByMake(string VehicleMake)
        {
            List<VehicleDatabase> VehiclesByMake = this.database.Table<VehicleDatabase>().ToList().
                Where(x => x.VehicleMake == VehicleMake).ToList();

            return VehiclesByMake;
        }

        //Returnss a list of models (string) by make
        public List<String> GetListOfModelsByMake(string VehicleMake)
        {
            List<String> models = new List<string>();

            foreach (VehicleDatabase vehicle in this.database.Table<VehicleDatabase>().ToList()
                .Where(x => x.VehicleMake == VehicleMake).ToList())
            {
                if (models.Contains(vehicle.VehicleModel) == false)
                {
                    string vehicleMakeToBeAdded = vehicle.VehicleModel;

                    models.Add(vehicleMakeToBeAdded);
                }
            }
            return models;

        }

        //Returns a list of strings representing all makes currently stored 
        public List<String> GetListOfMakes()
        {

            List<String> makes = new List<string>();

            foreach (VehicleDatabase vehicle in this.database.Table<VehicleDatabase>().ToList())
            {
                if (makes.Contains(vehicle.VehicleMake) == false)
                { 
                    string vehicleMakeToBeAdded = vehicle.VehicleMake;

                    makes.Add(vehicleMakeToBeAdded);
                }
            }
            return makes;

        }

        //====================================================================================================
        // RepairOrderServiceJobBridge-------------------------------
        //====================================================================================================

        //returns list of bridge items
        public List <RepairOrderServiceJobBridge> GetAllRepairOrderServiceJobBridge()
        {
            return this.database.Table<RepairOrderServiceJobBridge>().ToList();
        }

        //creates bridge item
        public void AddRepairOrderServiceJobBridge(RepairOrderServiceJobBridge repairOrderServiceJobBridge)
        {
            this.database.Insert(repairOrderServiceJobBridge);
        }

        //updates bridge item
        public void UpdateRepairOrderServiceJobBridge(RepairOrderServiceJobBridge repairOrderServiceJobBridge)
        {
            this.database.Update(repairOrderServiceJobBridge);
        }

        //removes bridge item based on PK
        public void DeleteRepairOrderServiceJobBridge(int BridgePK)
        {
            this.database.Delete<RepairOrderServiceJobBridge>(BridgePK);
        }

        //returns list of service jobs based on the repairOrderId
        public List<ServiceJob> GetServiceJobListByRepairOrderId(int repairOrderId)
        {
            //New list of service jobs
            List<ServiceJob> serviceJobs = new List<ServiceJob>();

            //Goes through each row of data in the Repair order service job bridge table
            foreach (RepairOrderServiceJobBridge var in this.database.Table<RepairOrderServiceJobBridge>().ToList()) 
            {
                //if the line of data matches the repair order id, find the ServiceJob based on the service Job Id and add it to the list
                if (var.RepairOrderId == repairOrderId)
                {
                    ServiceJob serviceJob = this.database.Table<ServiceJob>().First(x => x.ServiceJobId == var.ServiceJobId); 
                    serviceJobs.Add(serviceJob);
                }
            }

            return serviceJobs;
        }

        //====================================================================================================
        // Shop settings-------------------------------
        //====================================================================================================


        //Updates shop settings to database
        public void UpdateShopSettings (ShopSettings shop)
        {
            this.database.Update(shop);
        }

        //returns shop settings
        public ShopSettings GetShopSettings()
        {
            return this.database.Table<ShopSettings>().First();
        }


    }


}
