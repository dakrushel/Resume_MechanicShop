using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class MechanicShopSQLite
    {

        private SQLiteConnection database; 

        public MechanicShopSQLite()
        {
            this.database = new SQLiteConnection(Constant.DatabasePath);

            this.database.Execute("PRAGMA foreign_keys = ON;");


            //var checkVehicle = this.database.ExecuteScalar<string>("SELECT * FROM Vehicle");
            //if (checkVehicle == null && (this.database.ExecuteScalar<string>("SELECT COUNT (*) FROM Vehicle") != "0") )


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
                this.database.CreateTable<Employee>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error creating RepairOrder table: " + ex.Message);
            }
        }

        public void AddCustomer(Customer customer)
        { 
            this.database.Insert(customer);
        }
    }
}
