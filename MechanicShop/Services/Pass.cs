using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicShop.Models;

namespace MechanicShop.Services
{
    public static class Pass // Written by Chloe
    {
        // FOR PASSING OBJECTS BETWEEN PAGES. Certain tasks in this app require navigating to a 
        // different page/view WITH an object ready to load. The getters, setters and variables in
        // this class are built for this purpose. 

        // When passing something to another page, a view will load bojects into these variables and then
        // navigate to the other page as normal.

        // When loading the Appointment or Repair Order view, the system checks if anything is stored here
        // if so, the view will retrieve these objects and set up the GUI accordingly,
        // if there is nothing here, the default page view loads.
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
        public static Customer? RetreiveCustomer()
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
        public static Vehicle? RetrieveVehicle()
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
        public static RepairOrder? RetrieveRO()
        {
            RepairOrder? ro = ROPass;
            ROPass = null;
            return ro;
        }
    }
}
