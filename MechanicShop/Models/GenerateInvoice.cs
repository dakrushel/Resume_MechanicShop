using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class GenerateInvoice //Denver
    {
        //Getters and setters
        public string RepairOrderOutput { get; set; }

        //Ctors
        public GenerateInvoice() { }
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
        public void SaveInvoiceDelRO(RepairOrder rO)
        {
            //Write RO to txt
            this.RepairOrderOutput =
                $"==================================================================\n" +
                $"               REPAIR ORDER {rO.RepairOrderId}\n" +
                $"    ----------------------------------------------------------\n" +
                $"    Description:\n" +
                $"    {rO.RepairOrderDescription}\n" +
                $"    ----------------------------------------------------------\n" +
                $"    \t\t\tDate\n\n" +
                $"    Created:\t     Appointment:\t   Closed:\n" +
                $"    {rO.DateCreated}         {rO.AppointmentDate}\t   {rO.DateClose}\n\n" +
                $"    ----------------------------------------------------------\n" +
                $"     Technician: {rO.RepairJobTechnician}\n" +
                $"    Employee ID: {rO.EmployeeId}\n" +
                $"         Job ID: {rO.ServiceJobId}\n" +
                $"          Hours: {rO.RepairOrderHours}\n" +
                $"        Vehicle: {rO.Vehicle}\r\n" +
                $"            VIN: {rO.VIN}\n\n";

            using (StreamWriter sw = new StreamWriter(Constant.RepairOrderPath))
            {
                sw.Write(this.RepairOrderOutput); 
            }

            //Delete RO from database RepairOrder, Customer, Vehicle, ServiceJob, Technician
            MauiProgram.ShopDB.RemoveRepairOrder(rO.RepairOrderId.ToString());
        }
    }
}
