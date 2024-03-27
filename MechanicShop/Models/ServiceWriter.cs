using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicShop.Models
{
    public class ServiceWriter : Employee
    {

        private int Salary { get; set; }

        public ServiceWriter()
        {
            //YOU DO THIS
        }

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
