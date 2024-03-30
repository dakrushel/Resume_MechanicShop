using SQLite;
using System.ComponentModel.DataAnnotations;

namespace MechanicShop.Models
{

    public class Employee
    {
        //PK IS ID

        [PrimaryKey, NotNull]
        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public string EmployeePhone { get; set; }

        public string HireDate { get; set; }

        public Employee(string employeeId, string name, string phone, string HireDate) 
        {
            this.EmployeeId = employeeId;
            this.Name = name;
            this.EmployeePhone = phone;
            this.HireDate = HireDate;
        }

        public Employee() { }
    }
}
