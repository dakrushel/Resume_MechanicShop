using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicShop.Models;
using MechanicShop.Views;
using System.Collections.ObjectModel;

namespace MechanicShop.Services
{
    public static class AppointmentViewService
    {

        //FOR APPOINTMENTS PAGE
        public static ObservableCollection<RepairOrder> upcomingAppointments = new ObservableCollection<RepairOrder>();
        public static ObservableCollection<RepairOrder> expiredAppointments = new ObservableCollection<RepairOrder>();
        public static void refreshAppointments()
        {
            upcomingAppointments.Clear();
            expiredAppointments.Clear();
            List<RepairOrder> appointments = MauiProgram.ShopDB.GetRepairOrdersThatAreNOTActive();

            foreach (var appointment in appointments)
            {
                DateTime appointmentDate = DateTime.Parse(appointment.AppointmentDate);
                if (appointmentDate < AppointmentView.currentDate.AddDays(-1))
                {
                    expiredAppointments.Add(appointment);
                }
                else
                {
                    upcomingAppointments.Add(appointment);
                }
            }
        }


        //FOR REPAIR ORDERS PAGE
        public static ObservableCollection<RepairOrder> unassigned = new ObservableCollection<RepairOrder>();
        public static ObservableCollection<RepairOrder> inProgress = new ObservableCollection<RepairOrder>();
        public static void refreshROs()
        {
            unassigned.Clear();
            inProgress.Clear();
            List<RepairOrder> roList = MauiProgram.ShopDB.GetRepairOrdersThatAreActive();

            foreach (var ro in roList)
            {
                if (ro.EmployeeId == null)
                {
                    unassigned.Add(ro);
                }
                else
                {
                    inProgress.Add(ro);
                }
            }
        }

        // Get a list of technicians that are not currently assigned to any repair orders.
        public static ObservableCollection<Technician> openTechnicians = new ObservableCollection<Technician>();
        public static void RefreshOpenTechnicians()
        {
            openTechnicians.Clear();
            List<Technician> techList = MauiProgram.ShopDB.GetAllTechnicians();

            foreach (var tech in techList)
            {
                foreach (var ro in MauiProgram.ShopDB.GetAllRepairOrders())
                {
                    if (ro.EmployeeId == tech.EmployeeId) 
                    {
                        continue;
                    }
                    
                }
                openTechnicians.Add(tech);
            }
        }

        public static Technician GetTechByID (int id)
        {
            foreach (var tech in MauiProgram.ShopDB.GetAllTechnicians())
            {
                if (tech.EmployeeId == id)
                {
                    return tech;
                }
            }
            return null;
        }
        

        
    }
}
