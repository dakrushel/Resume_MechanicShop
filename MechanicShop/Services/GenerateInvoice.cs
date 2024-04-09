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
            Technician tempTech = new Technician();
            if (rO.EmployeeId != null)
            {
                string id = rO.EmployeeId.ToString();
                tempTech = MauiProgram.ShopDB.GetTechnician(Int32.Parse(id));
            }
            //Write RO to txt
            RepairOrderOutput =
                $"==================================================================\n" +
                $"    REPAIR ORDER: {rO.RepairOrderId}\n" +
                $"    ----------------------------------------------------------\n" +
                $"    CUSTOMER\n\n" +
                $"    Name: {rO.CustomerName}\t\t Phone: {rO.CustomerPhoneNumber}\n\n" +
                $"    Description:\n" +
                $"    {rO.RepairOrderDescription}\n\n" +
                $"    Vehicle: {tempVehicle}\n" +
                $"    VIN: {rO.VIN}\n\n" +
                $"    ----------------------------------------------------------\n" +
                $"    DATE\n\n" +
                $"    Created:\t     Appointment:\t   Closed:\n" +
                $"    {rO.DateCreated}         {rO.AppointmentDate}\t   {rO.DateClose}\n\n" +
                $"    ----------------------------------------------------------\n" +
                $"    Employee ID: {rO.EmployeeId}\n" +
                $"     Technician: {tempTech.Name}\n" +
                $"          Hours: {rO.RepairOrderHours}\n";

            //Generate NEW txt file for each Invoice (based on Customer Name and RO ID)
            Constant.repairOrderFilename += $"{rO.RepairOrderId}_{rO.CustomerName}";

            using (StreamWriter sw = new StreamWriter(Constant.RepairOrderPath))
            {
                sw.Write(RepairOrderOutput);
            }

            //Delete RO from database RepairOrder, Customer, Vehicle, ServiceJob, Technician
            //MauiProgram.ShopDB.RemoveRepairOrder(rO.RepairOrderId);


        }


    }
}
