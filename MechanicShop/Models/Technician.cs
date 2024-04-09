using SQLite;
using SQLiteNetExtensions.Attributes;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace MechanicShop.Models
{
    [Table("Technician")]
    public class Technician 
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public string EmployeeId { get; set; }

        public string Specialization { get; set; }

        public string Name { get; set; }

        public string EmployeePhone { get; set; }

        public string HireDate { get; set; }

        public double HourlyRate { get; set; }

        public Technician(string name, string employeePhone, string hireDate, string specialization, double hourlyRate)
        {
            //this.EmployeeId = employeeId;
            this.Name = name;
            this.EmployeePhone = employeePhone;
            this.HireDate = hireDate;
            this.Specialization = specialization;
            this.HourlyRate = hourlyRate;

        }

        public Technician()
        { }


    }
}
