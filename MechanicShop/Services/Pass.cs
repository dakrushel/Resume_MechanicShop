using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicShop.Models;

namespace MechanicShop.Services
{
    public static class Pass
    {

        // FOR PASSING OBJECTS BETWEEN PAGES
        public static Customer? CustomerPass {  get; set; }
        public static Vehicle? VehiclePass { get; set; }
        public static RepairOrder? ROPass { get; set; }

        public static void PassCustomer (Customer c)
        {
            if (c != null)
            {
                CustomerPass = c;
            }          
        }
        public static Customer RetreiveCustomer()
        {
            Customer? customer = CustomerPass;
            CustomerPass = null;
            return customer;
        }
        public static void PassVehicle (Vehicle v)
        {
            if (v != null)
            {
                VehiclePass = v;
            }
        }
        public static Vehicle RetrieveVehicle()
        {
            Vehicle? vehicle = VehiclePass;
            VehiclePass = null;
            return vehicle;
        }
        public static void PassRO(RepairOrder r)
        {
            if (r != null)
            {
                ROPass = r;
            }
        }
        public static RepairOrder RetrieveRO()
        {
            RepairOrder? ro = ROPass;
            ROPass = null;
            return ro;
        }


    }
}
