using MechanicShop.Models;
using MechanicShop.Views;
using System.Collections.ObjectModel;


namespace MechanicShop.Services
{
    public static class AppointmentViewService
    {
        //====================================================================================================
        //               FOR APPOINTMENTS PAGE
        //====================================================================================================
        public static ObservableCollection<RepairOrder>? upcomingAppointments;
        public static ObservableCollection<RepairOrder>? expiredAppointments;
        public static void refreshAppointments() // Chloe
        {
            // Establish new collections
            upcomingAppointments = new ObservableCollection<RepairOrder>();
            expiredAppointments = new ObservableCollection<RepairOrder>();
            // Get a list of all INACTIVE orders (aka appointments) from the DB
            List<RepairOrder> appointments = MauiProgram.ShopDB.GetRepairOrdersThatAreNOTActive();
            // Send to sort method to populate the colelctions
            foreach (var appointment in appointments)
            {
                SortByDate(appointment);
            }
        }
        public static void RefreshByPhone(string phone) //Chloe
        {
            upcomingAppointments = new ObservableCollection<RepairOrder>();
            expiredAppointments = new ObservableCollection<RepairOrder>();
            // Get a list of all repair orders matching the input phone number
            List<RepairOrder> appointments = MauiProgram.ShopDB.GetRepairOrdersByCustPhone(phone);
            // Sent to sorter to build collections
            foreach (var appointment in appointments)
            {
                if (appointment.IsActive == false) // filter out active (aka Repaior Orders)
                {
                    SortByDate(appointment);
                }         
            }
        }
        public static void RefreshByName(string name) //Chloe
        {
            upcomingAppointments = new ObservableCollection<RepairOrder>();
            expiredAppointments = new ObservableCollection<RepairOrder>();
            // Get a list of all repair orders matching (or containing) the input name
            List<RepairOrder> appointments = MauiProgram.ShopDB.GetRepairOrdersByCustName(name);
            foreach (var appointment in appointments)
            {
                if (appointment.IsActive == false) // filter out active (aka Repaior Orders)
                {
                    SortByDate(appointment);
                }
            }
        }
        public static void SortByDate(RepairOrder appointment)
        {
            // Sorts appointments into upcoming and expired for seperate listviews in APPOINTMENTS page
            DateTime appointmentDate = DateTime.Parse(appointment.AppointmentDate);
            if (appointmentDate < AppointmentView.currentDate.AddDays(-1))
            {
                expiredAppointments?.Add(appointment);
            }
            else
            {
                upcomingAppointments?.Add(appointment);
            }
        }


        //====================================================================================================
        //               FOR REPAIR ORDERS PAGE
        //====================================================================================================
        public static ObservableCollection<RepairOrder> unassigned = new ObservableCollection<RepairOrder>();
        public static ObservableCollection<RepairOrder> inProgress = new ObservableCollection<RepairOrder>();
        public static void refreshROs() // Chloe
        {
            // Sorts all REPAIR ORDERS by active or inactive for seperate listview displays in the Repair Order View
            unassigned.Clear();
            inProgress.Clear();
            List<RepairOrder> roList = MauiProgram.ShopDB.GetRepairOrdersThatAreActive();

            foreach (var ro in roList)
            {
                if (ro.EmployeeId > 0)
                {
                    inProgress.Add(ro);
                }
                else
                {
                    unassigned.Add(ro);
                }
            }
        }

        // Get a list of technicians that are not currently assigned to any repair orders.
        public static ObservableCollection<Technician>? openTechnicians;

        public static void RefreshOpenTechnicians() //Chloe
        {
            // clears the static list
            openTechnicians = new ObservableCollection<Technician>();
            // queries a list of all techs and a list of all orders from Zack's code
            List<Technician>? techList = MauiProgram.ShopDB.GetAllTechnicians();
            List<RepairOrder>? roList = MauiProgram.ShopDB.GetAllRepairOrders();
            List<int?>? assignedIDs = new List<int?>();
            if (roList != null)
            {
                // geta a list of technicians (IDs) that are currently assigned to active jobs
                foreach (RepairOrder ro in roList)
                {
                    if (ro.EmployeeId != null)
                    {
                        assignedIDs.Add(ro.EmployeeId);
                    }
                }
            }
            // if any techs are already assigned, removes them from the collection of available technicians
            if (assignedIDs != null && techList != null)
            {
                for (int i = techList.Count - 1; i >= 0; i--)
                {
                    if (assignedIDs.Contains(techList[i].EmployeeId))
                    {
                        techList.RemoveAt(i);
                    }
                }
            }
            // sets the collection
            if (techList != null)
            {
                openTechnicians = new ObservableCollection<Technician>(techList);
            }        
        }

        public static Technician? GetTechByID (int? id) //Chloe
        {
            // returns a particular technician object based on the ID entered
            foreach (var tech in MauiProgram.ShopDB.GetAllTechnicians())
            {
                if (tech.EmployeeId == id)
                {
                    return tech;
                }
            }
            // returns null if no match is found
            return null;
        }

        public static double CalculateEstimate(List<ServiceJob> jobs) //Chloe
        {
            // get the current shop hourly rate from the database
            // this should automatically recalculate ROs even after changing the shop rate
            ShopSettings rates = MauiProgram.ShopDB.GetShopSettings();
            // get total hours
            double hours = GetHours(jobs);
            // multiply total hours by shop rate and return value
            return hours * rates.ShopHourlyRate;
        }

        public static double GetTotal(List<ServiceJob> jobs) //Chloe
        {
            // get the shop supplies cost
            ShopSettings rates = MauiProgram.ShopDB.GetShopSettings();
            // Total the cost before shop supplies using the existing estimate method above
            double jobCost = CalculateEstimate(jobs);
            // return the cost with shop supplies added
            return jobCost + rates.ShopSupplyCost;
        }

        public static double GetHours(List<ServiceJob> jobs) //Chloe
        {
            // Returns total hours for a joblist
            double hours = 0;
            foreach(var job in jobs)
            {
                hours += job.ServiceJobHours;
            }
            return hours;
        }
        

        
    }
}
