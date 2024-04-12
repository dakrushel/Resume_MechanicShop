using SQLite;
using SQLiteNetExtensions.Attributes;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace MechanicShop.Models
{
    [Table("Technician")]
    public class Technician 
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int EmployeeId { get; set; }

        public string Specialization { get; set; }

        public string Name { get; set; }

        public string EmployeePhone { get; set; }

        public string HireDate { get; set; }

        public double HourlyRate { get; set; }

        public Technician(string name, string employeePhone, string hireDate, string specialization, double hourlyRate)
        {
  
            this.Name = name;
            this.EmployeePhone = employeePhone;
            this.HireDate = hireDate;
            this.Specialization = specialization;
            this.HourlyRate = hourlyRate;

        }

        public Technician()
        { }

        //Returns true if a tech is not assigned to an active RO and false if a tech is not
        public static bool RemoveTechChecker(Technician technician)// Denver
        {
            //Start with a list of ROs
            List<RepairOrder> tempROs = MauiProgram.ShopDB.GetRepairOrdersByTechnician(technician.EmployeeId);
            //Nest we need a list of active ROs
            List<RepairOrder> activeROs = new List<RepairOrder>();
            //Check to see if tempROs even has anything in it
            if (tempROs.Count > 0)
            {
                //Then sort through and see if any of them are active
                foreach (RepairOrder r in tempROs)
                {
                    if (r.IsActive) {  activeROs.Add(r); }
                }
                //If there are active ROs return false, otherwise return true
                if (activeROs.Count > 0) { return false; } else { return true; }
            }
            //If there are no ROs, return true
            else { return true; }
        }
    }
}
