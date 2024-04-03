using SQLite;
using SQLiteNetExtensions.Attributes;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace MechanicShop.Models
{
    [Table("Technician")]
    public class Technician : Employee
    {
        [PrimaryKey, NotNull, ForeignKey(typeof(Employee))]
        public string EmployeeId { get; set; }
        [OneToOne]
        public Employee Employee { get; set; }
        public string Specialization { get; set; }

        public double HourlyRate { get; set; }

        public Technician(string employeeId, string name, string employeePhone, string hireDate, string specialization, double hourlyRate)
            : base(employeeId, name, employeePhone, hireDate)
        {
            this.EmployeeId = employeeId;
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
