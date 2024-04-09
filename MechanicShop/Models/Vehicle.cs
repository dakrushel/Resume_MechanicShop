using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace MechanicShop.Models
{
    [Table("Vehicle")]
    public class Vehicle
    {

        //Data annotations for the databasse creation
        //In Vehicle VIN is the Primary Key
        //All other properties cannot be null 
        [PrimaryKey, NotNull]
        public string VIN { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Colour { get; set; }

        public int Year { get; set; }

        [ForeignKey(typeof(Customer))]
        public string CustomerPhone { get; set; }


        public Vehicle (string VIN, string Make, string Model, string Colour, int Year, string CustomerPhone)
        {
            this.VIN = VIN;
            this.Make = Make;
            this.Model = Model;
            this.Colour = Colour;
            this.Year = Year;
            this.CustomerPhone = CustomerPhone;
        }

        public Vehicle() { }

        public static bool RemoveVehicleChecker(Vehicle vehicle) //Denver
        {
            //Start with a list of all ROs that a vehicle has
            List<RepairOrder> tempRO = MauiProgram.ShopDB.GetRepairOrdersByVIN(vehicle.VIN);
            List<RepairOrder> activeRO = new List<RepairOrder>();
            //If tempRO isn't empty, run sort block
            if (tempRO.Count > 0)
            {
                
                foreach (RepairOrder r in tempRO)
                {
                    if (r.IsActive == true)
                    {
                        activeRO.Add(r);
                    }
                }
                if (activeRO.Count > 0)
                {
                    return false;
                }
                else
                {
                    //MauiProgram.ShopDB.RemoveVehicle(vehicle.VIN);
                    return true;
                }
            }
            //Otherwise return true
            else
            {
                //MauiProgram.ShopDB.RemoveVehicle(vehicle.VIN);
                return true;
            }
        }
    }
}
