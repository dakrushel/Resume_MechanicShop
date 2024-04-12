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

        public static bool RemoveTechChecker(Technician technician)
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
                    if (r.IsActive)
                }
            }
        }
    }
}
