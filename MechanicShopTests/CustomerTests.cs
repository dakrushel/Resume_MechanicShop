using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicShop.Models;
namespace MechanicShopTests
{
    [TestFixture]
    public class CustomerTests
    {
        public Customer testCustomer = new Customer("Greg", "test", "test");
        [SetUp] 
        public void SetUp() 
        {
            
        }
    }
}
