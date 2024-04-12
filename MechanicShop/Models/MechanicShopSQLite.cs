
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

        private SQLiteConnection database;

        public MechanicShopSQLite()
        {
            Console.WriteLine(Constant.DatabasePath);
            this.database = new SQLiteConnection(Constant.DatabasePath);

            this.database.Execute("PRAGMA foreign_keys = ON;");

            //For dropping tables, uncomment the next line and insert your table name in the <>


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


            this.database.Commit();

        }

        /*----------------------------CUSTOMER ------------------------------------*/
        //TESTED 
        public void AddCustomer(Customer customer)
        {
            this.database.Insert(customer);
        }

        //Tested
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

        //Search for customer
        //Take customer name or phone# as argument
        //If phone# search by primary key and return cutomer
        //If name only search by name and return list of matches


        public List<Customer> GetAllCustomers()
        {
            return this.database.Table<Customer>().ToList();
        }


        //TESTED
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

        //TESTED
        public List<Customer> GetCustomerByName(string customerName) //Denver
        {
            //Search for a partial match(es) based on customer name
            return this.database.Table<Customer>()
                .Where(x => x.Name.ToLower().Contains(customerName.ToLower()))
                .ToList();
        }

        public List<Customer> GetCustomerByPhone(string customerPhone) //Denver
        {
            //Search for partial matches based on customer phone number
            return this.database.Table<Customer>()
                .Where(x => x.CustomerPhone.ToLower().Contains(customerPhone.ToLower()))
                .ToList();
        }
        public Customer GetACustomerByName(string customerName)
        {
            return this.database.Table<Customer>().FirstOrDefault(x => x.Name == customerName);
        }
        public Customer GetACustomerByPhone(string customerPhone)
        {
            return this.database.Table<Customer>().FirstOrDefault(x => x.CustomerPhone == customerPhone);
        }


        /*---------------------------- VEHICLE ------------------------------------*/

        //TESTED
        public void AddVehicle(Vehicle vehicle)
        {
            this.database.Insert(vehicle);
        }

        //TESTED
        public void UpdateVehicle(Vehicle vehicle)
        {
            this.database.Update(vehicle);
        }

        //TESTED
        public void RemoveVehicle(string VIN)
        {
            this.database.Delete<Vehicle>(VIN);
        }

        public List<Vehicle> GetAllVehicles()
        {
            return this.database.Table<Vehicle>().ToList();
        }

        //TODO: For Logic layer this method would need checks to ensure that repairOrderId is valid
        public Customer GetCustomerViaVIN(string VIN)
        {
            Vehicle targetVehicle = this.database.Table<Vehicle>().First(x => x.VIN == VIN);

            Customer customer = this.database.Table<Customer>().First(x => x.CustomerPhone == targetVehicle.CustomerPhone);

            return customer;
        }

        public Vehicle GetVehicleByVIN(string VIN)
        {
            return this.database.Table<Vehicle>().First(x => x.VIN == VIN); 
        }


        /*----------------------------TECHNICIAN ------------------------------------*/
        //TESTED
        public void AddTechnician(Technician technician)
        {
            this.database.Insert(technician);
        }

        //TESTED
        public void UpdateTechnician(Technician technician)
        {
            this.database.Update(technician);
        }

        //TESTED
        public void RemoveTechnician(int employeeId)
        {
            this.database.Delete<Technician>(employeeId);
        }

        public List<Technician> GetAllTechnicians()
        {
            return this.database.Table<Technician>().ToList();
        }

        public Technician GetTechnician(int? employeeId) //Denver
        {
            //Get a single technician by employeeId
            return this.database.Table<Technician>().FirstOrDefault(x => x.EmployeeId == employeeId);
        }
        /*---------------------------- REPAIR ORDER ------------------------------------*/

        //TESTED
        public void AddRepairOrder(RepairOrder repairOrder)
        {
            this.database.Insert(repairOrder);
        }

        public void RemoveRepairOrder(int repairOrderId)
        {
            RepairOrder repairOrder = this.database.Get<RepairOrder>(repairOrderId);
            //For loop to delete each RepairOrderServiceJob attached to the repair order
            foreach (RepairOrderServiceJobBridge bridge in this.GetAllRepairOrderServiceJobBridge())
            {

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
            foreach (RepairOrderServiceJobBridge var in this.database.Table<RepairOrderServiceJobBridge>().ToList())
            {
                if (var.RepairOrderId == repairOrderId)
                {
                    this.database.Delete<RepairOrderServiceJobBridge>(var.BridgeIdForROandSJ);
                }
            }
        }

        //TESTED
        public void UpdateRepairOrder(RepairOrder repairOrder)
        {
            this.database.Update(repairOrder);
        }

        public List<RepairOrder> GetAllRepairOrders()
        {
            return this.database.Table<RepairOrder>().ToList();
        }

        public List<RepairOrder> GetRepairOrdersThatAreNOTActive()
        {
            return this.database.Table<RepairOrder>()
                .Where(x => x.IsActive == false).ToList();
        }

        public List<RepairOrder> GetRepairOrdersThatAreActive()
        {
            return this.database.Table<RepairOrder>()
                .Where(x => x.IsActive == true).ToList();
        }

        public RepairOrder GetRepairOrderByVIN(string VIN)
        {
            return this.database.Table<RepairOrder>().First(x => x.VIN == VIN);
        }

        public RepairOrder GetRepairOrderByPK(int PK)
        {
            return this.database.Table<RepairOrder>().First(x => x.RepairOrderId == PK);
        }

        //TESTED
        public List<RepairOrder> GetRepairOrdersByVIN(string vin) //Denver
        {
            //returns a list of RepairOrder with a particular VIN
            return this.database.Table<RepairOrder>().ToList()
                .Where(x => x.VIN == vin).ToList();
        }

        public List<RepairOrder> GetRepairOrdersByCustPhone(string customerPhone) //Denver
        {
            //returns a list (partial matches) of RepairOrders based on customer phone #
            return this.database.Table<RepairOrder>()
                .Where(x => x.CustomerPhoneNumber.Contains(customerPhone.ToLower()))
                .ToList();
        }

        public List<RepairOrder> GetRepairOrdersByCustName(string customerName)
        {
            //Same as above but with customer name and case insensitive
            return this.database.Table<RepairOrder>()
                .Where(x => x.CustomerName.ToLower().Contains(customerName.ToLower()))
                .ToList();
        }

        public List<RepairOrder> GetRepairOrdersByTechnician(int employeeId)
        {
            //Same as above but with employee ID (for the RemoveTechChecker)
            //Arg is an int so no need for partial matches or case sensitivity
            return this.database.Table<RepairOrder>().Where(x => x.EmployeeId == employeeId).ToList();
        }

        /*----------------------------SERVICE JOB ------------------------------------*/

        //TESTED
        public void AddServiceJob(ServiceJob job)
        {
            this.database.Insert(job);
        }

        //TESTED
        public void UpdateServiceJob(ServiceJob job)
        {
            this.database.Update(job);
        }

        //TESTED
        public void RemoveServiceJob(string serviceJobId)
        {
            this.database.Delete<ServiceJob>(serviceJobId);
        }

        public List<ServiceJob> GetAllServiceJobs()
        {
            return this.database.Table<ServiceJob>().ToList();
        }

        /*---------------------------- VEHICLE DATABASE ------------------------------------*/
        
        //Tested
        public void AddVehicleDatabse(VehicleDatabase vehicle)
        {
            this.database.Insert(vehicle);
        }

        public void UpdateVehicleDatabase(VehicleDatabase vehicle)
        { 
            this.database.Update(vehicle); 
        }

        public void RemoveVehicleDatabase(string VehicleModel)
        {
            this.database.Delete<VehicleDatabase>(VehicleModel);
        }
        public List<VehicleDatabase> GetAlLVehicleDatabase()
        {
            return this.database.Table<VehicleDatabase>().ToList();
        }


/*        -------------------------------MAKES AND MODELS --------------------------------------*/
        public List<VehicleDatabase> GetAllVehicleByMake(string VehicleMake)
        {
            List<VehicleDatabase> VehiclesByMake = this.database.Table<VehicleDatabase>().ToList().
                Where(x => x.VehicleMake == VehicleMake).ToList();

            return VehiclesByMake;
        }

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

/*        ---------------------------- Repair Order Service Job Bridge -------------------------*/

        public List <RepairOrderServiceJobBridge> GetAllRepairOrderServiceJobBridge()
        {
            return this.database.Table<RepairOrderServiceJobBridge>().ToList();
        }

        public void AddRepairOrderServiceJobBridge(RepairOrderServiceJobBridge repairOrderServiceJobBridge)
        {
            this.database.Insert(repairOrderServiceJobBridge);
        }

        public void UpdateRepairOrderServiceJobBridge(RepairOrderServiceJobBridge repairOrderServiceJobBridge)
        {
            this.database.Update(repairOrderServiceJobBridge);
        }

        public void DeleteRepairOrderServiceJobBridge(int BridgePK)
        {
            this.database.Delete<RepairOrderServiceJobBridge>(BridgePK);
        }

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

        /*        ---------------------------- Job Settings -------------------------*/
        


        public void UpdateShopSettings (ShopSettings shop)
        {
            this.database.Update(shop);
        }

        public ShopSettings GetShopSettings()
        {
            return this.database.Table<ShopSettings>().First();
        }


    }


}
