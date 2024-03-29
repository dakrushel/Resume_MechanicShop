using SQLite;

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


            //var checkVehicle = this.database.ExecuteScalar<string>("SELECT * FROM Vehicle");
            //if (checkVehicle == null && (this.database.ExecuteScalar<string>("SELECT COUNT (*) FROM Vehicle") != "0") )

            try
            {
                this.database.CreateTable<Customer>();
            }
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

            this.database.Commit();
        }


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

        public void AddVehicle(Vehicle vehicle) 
        {
            this.database.Insert(vehicle);
        }


    }
}
