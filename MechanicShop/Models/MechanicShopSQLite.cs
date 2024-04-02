
using MechanicShop.Resources;
using SQLite;
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
            //this.database.DropTable<VehicleDatabase>();

            //Try creating this table
            try
            {
                this.database.CreateTable<Customer>();
            }
            //Catch statement for any tabless that have been created
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<RepairOrder>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<Vehicle>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<Employee>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<Technician>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<ServiceJob>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }

            try
            {
                this.database.CreateTable<VehicleDatabase>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }

          

            this.database.Commit();

        }

        /*----------------------------CUSTOMER ------------------------------------*/
        //TESTED 
        public void AddCustomer(Customer customer)
        {
            this.database.Insert(customer);
        }

        //TESTED
        public void RemoveCustomer(string customerPhoneNumber)
        {
            this.database.Delete<Customer>(customerPhoneNumber);
        }


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
        public void RemoveTechnician(string employeeId)
        {
            this.database.Delete<Technician>(employeeId);
        }

        public List<Technician> GetAllTechnicians()
        {
            return this.database.Table<Technician>().ToList();
        }

        /*---------------------------- REPAIR ORDER ------------------------------------*/

        //TESTED
        public void AddRepairOrder(RepairOrder repairOrder)
        {
            this.database.Insert(repairOrder);
        }

        //TESTED
        public void RemoveRepairOrder(string repairOrderId)
        {
            this.database.Delete<RepairOrder>(repairOrderId);
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
    }

}
