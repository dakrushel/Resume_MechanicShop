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

        
    }
}
