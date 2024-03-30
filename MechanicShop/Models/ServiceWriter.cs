using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class ServiceWriter : Employee
    {
       
        public double Salary { get; set; }
        


        public ServiceWriter(string employeeId, string name, string phone, string hireDate, double salary) 
            : base(employeeId, name, phone, hireDate) 
        {
            this.EmployeeId = employeeId;
            this.Name = name;
            this.EmployeePhone = phone;
            this.HireDate = hireDate;
            this.Salary = salary;
        }

        public ServiceWriter()
        { }
        public void GenerateAppointment()
        {
            // creates a new appointment not positive we need this, may be handled front end
        }

        public void GenerateInvoice()
        {
            // closes RO IF closable
            // must be active and filled out!!!
            // need a way to get the invoice to the gui
            // possily a printable TXT
        }
    }
}
