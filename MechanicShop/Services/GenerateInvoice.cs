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

        //Ctors
        //public GenerateInvoice() { }
        //public GenerateInvoice(RepairOrder rO)
        //{
        //    //Retrieve RepairOrder (create temp RO?)
        //    RepairOrder temp = rO;
        //    this.RepairOrderOutput = 
        //        $"==================================================================\n" +
        //        $"               REPAIR ORDER {rO.RepairOrderId}\n" +
        //        $"    ----------------------------------------------------------\n" +
        //        $"    Description:\n" +
        //        $"    {rO.RepairOrderDescription}\n" +
        //        $"    ----------------------------------------------------------\n" +
        //        $"    \t\t\tDate\n\n" +
        //        $"    Created:\t     Appointment:\t   Closed:\n" +
        //        $"    {rO.DateCreated}         {rO.AppointmentDate}\t   {rO.DateClose}\n\n" +
        //        $"    ----------------------------------------------------------\n" +
        //        $"     Technician: {rO.RepairJobTechnician}\n" +
        //        $"    Employee ID: {rO.EmployeeId}\n" +
        //        $"         Job ID: {rO.ServiceJobId}\n" +
        //        $"          Hours: {rO.RepairOrderHours}\n" +
        //        $"        Vehicle: {rO.Vehicle}\r\n" +
        //        $"            VIN: {rO.VIN}\n\n";
        //}

        //Close and save method. Must output RepairOrder to a txt file and remove it from the database
        
        //TESTED
        public static void SaveInvoiceDelRO(RepairOrder rO)
        {
            Vehicle tempVehicle = MauiProgram.ShopDB.GetVehicleByVIN(rO.VIN);
            List<ServiceJob> tempJobs = rO.ListOfServiceJobs;
            Technician tempTech = new Technician();
            if (rO.EmployeeId != null)
            {
                string id = rO.EmployeeId.ToString();
                tempTech = MauiProgram.ShopDB.GetTechnician(Int32.Parse(id));
            }
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
                $"    Employee ID: {rO.EmployeeId}\t\t\tTechnician: {tempTech.Name}\n\n" +
                $"    Work done:\n" +
                $"    {workDone}\n\n\n" +
                $"    Total Hours............................................ {totalHours}\n" +
                //$"    Amount Owing.........................................{totalHours * ShopSettings.ShopHourlyRate}" +
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
