using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicShop.Models;

namespace MechanicShop.Services
{
    public static class GenerateInvoice //Denver
    {
        //Getters and setters
        public static string RepairOrderOutput { get; set; }
        
        //TESTED
        public static void SaveInvoiceDelRO(RepairOrder rO)
        {
            ShopSettings shopSettings = MauiProgram.ShopDB.GetShopSettings();
            Vehicle tempVehicle = MauiProgram.ShopDB.GetVehicleByVIN(rO.VIN);
            List<ServiceJob> tempJobs = MauiProgram.ShopDB.GetServiceJobListByRepairOrderId(rO.RepairOrderId);
            Technician tech = MauiProgram.ShopDB.GetTechnician(rO.EmployeeId);
            string? techName = tech.Name;
            string workDone = null;
            double totalHours = 0;
            if (tempJobs != null)
            {
                foreach (ServiceJob sj in tempJobs)
                {
                    workDone += $"{sj.ServiceJobDescription}\n    Hours.................................................. {sj.ServiceJobHours}\n\n";
                    totalHours += sj.ServiceJobHours;
                }
            }
            //Write RO to txt
            RepairOrderOutput =
                $"==================================================================\n\n" +
                $"    REPAIR ORDER: {rO.RepairOrderId}\n" +
                $"    ----------------------------------------------------------\n" +
                $"    CUSTOMER\n\n" +
                $"    Name: {rO.CustomerName}\t\t Phone: {rO.CustomerPhoneNumber}\n\n" +
                $"    Vehicle: {tempVehicle.Year} {tempVehicle.Make} {tempVehicle.Model}\n" +
                $"    VIN: {rO.VIN}\n\n" +
                $"    ----------------------------------------------------------\n" +
                $"    DATE\n\n" +
                $"    Created:\t     Appointment:\t   Closed:\n" +
                $"    {rO.DateCreated}         {rO.AppointmentDate}\t   {rO.DateClose}\n\n" +
                $"    ----------------------------------------------------------\n" +
                $"    DESCRIPTION\n\n" +
                $"    {rO.RepairOrderDescription}\n\n" +
                $"    Employee ID: {rO.EmployeeId}\t\t\tTechnician: {techName}\n\n" +
                $"    Work done:\n" +
                $"    {workDone}\n\n\n" +
                $"    Total Hours............................................ {totalHours}\n\n" +
                $"    Shop Supplies........................................ {shopSettings.ShopSupplyCost.ToString("C")}\n\n" +
                $"    Amount Owing......................................... {(totalHours * shopSettings.ShopHourlyRate).ToString("C")}" +
                $"\n\n==================================================================";
                                                    
            //Generate NEW txt file for each Invoice (based on Customer Name, RO ID, and Date Created)
            Constant.repairOrderFilename += $"{rO.RepairOrderId}_{rO.CustomerName}_{rO.DateCreated}";

            using (StreamWriter sw = new StreamWriter(Constant.RepairOrderPath))
            {
                sw.Write(RepairOrderOutput);
            }

            //Delete RO from database RepairOrder, Customer, Vehicle, ServiceJob, Technician
            //MauiProgram.ShopDB.RemoveRepairOrder(rO.RepairOrderId);


        }


    }
}
