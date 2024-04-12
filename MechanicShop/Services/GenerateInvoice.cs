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
        public static void SaveInvoiceDelRO(RepairOrder rO) //Denver
        {
            //ALL DEM VARIABLES! Because ROs don't have them once they are in the DB
            ShopSettings shopSettings = MauiProgram.ShopDB.GetShopSettings();
            Vehicle tempVehicle = MauiProgram.ShopDB.GetVehicleByVIN(rO.VIN);
            List<ServiceJob> tempJobs = MauiProgram.ShopDB.GetServiceJobListByRepairOrderId(rO.RepairOrderId);
            Technician tech = MauiProgram.ShopDB.GetTechnician(rO.EmployeeId);
            string? techName = tech.Name;
            string workDone = null;
            double totalHours = 0;

            //First, check if there are any jobs on the RO
            if (tempJobs != null)
            {
                //If there are, list them in workDone
                foreach (ServiceJob sj in tempJobs)
                {
                    workDone += $"    {sj.ServiceJobDescription}\n    Hours.................................................. {sj.ServiceJobHours}\n\n";
                    totalHours += sj.ServiceJobHours;
                }
            }
            //Store the whole thing in a RepairOrderOutput
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
                $"{workDone}\n" +
                $"    Total Hours............................................ {totalHours}\n\n" +
                $"    Shop Rate............................................ {shopSettings.ShopHourlyRate.ToString("C")}\n\n" +
                $"    Shop Supplies........................................ {shopSettings.ShopSupplyCost.ToString("C")}\n\n" +
                $"    Amount Owing......................................... {(totalHours * shopSettings.ShopHourlyRate + shopSettings.ShopSupplyCost).ToString("C")}" +
                $"\n\n==================================================================";
            
            
            //Generate NEW txt file for each Invoice (based on Customer Name, RO ID, and Date Closed)
            Constant.repairOrderFilename += $"{rO.DateClose}_{rO.CustomerName}_{rO.RepairOrderId}.txt";

            //Write it to file
            using (StreamWriter sw = new StreamWriter(Constant.RepairOrderPath))
            {
                sw.Write(RepairOrderOutput);
                //Reset repairOrderFilename
                Constant.repairOrderFilename = @"..\..\..\..\..\Resources\Raw\Invoices\";
            }

            //Delete RO from database RepairOrder, Customer, Vehicle, ServiceJob, Technician
            MauiProgram.ShopDB.RemoveRepairOrder(rO.RepairOrderId);


        }


    }
}
