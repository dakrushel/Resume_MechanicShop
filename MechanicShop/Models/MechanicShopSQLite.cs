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


            this.database.CreateTable<Vehicle>();
            this.database.CreateTable<Employee>();
            this.database.CreateTable<RepairOrder>();
            this.database.CreateTable<Customer>();

            
        }

        public void AddCustomer(Customer customer)
        { 
            this.database.Insert(customer);
        }
    }
}
