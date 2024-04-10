using MechanicShop.Models;
using MechanicShop.Resources;
using MechanicShop.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace MechanicShop
{
    public static class MauiProgram
    {
        public static MechanicShopSQLite ShopDB;
        public static DateTime currentDate = DateTime.Now;
        public static string dateString = currentDate.ToString("yyyy-MM-dd");

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //FOR TESTING
            //Customer Bob = new Customer("Bob", "123 Lane", "123-456-7890");
            //Customer Bill = new Customer("Bill", "123 Lane", "124-555-8888");
            //Customer Jane = new Customer("Jane", "124 Avenue", "999-000-9999");


            ShopDB = new MechanicShopSQLite();

            ////FOR TESTING
            void PrintToTxt(string s)
            {
                using (StreamWriter sw = new StreamWriter(Constant.TestPath))
                {
                    sw.Write(s);
                }
            }
            //Generate Invoice
            //Customer testCustomer = ShopDB.GetACustomerByName("lor");
            //List<RepairOrder> appointments = ShopDB.GetRepairOrdersByVIN(testCustomer.CustomerVehicles[0].VIN);
            //Vehicle testVehicle = ShopDB.GetVehicleByVIN(appointments[0].VIN);
            //RepairOrder tempRO = appointments[0];
            //GenerateInvoice.SaveInvoiceDelRO(tempRO);
            //RemoveCustomerChecker
            //PrintToTxt($"RemoveCustomerChecker: {Customer.RemoveCustomerChecker("555-555-5555")}");
            //PrintToTxt($"RemoveVehicleChecker: {Vehicle.RemoveVehicleChecker(testVehicle)}");


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

         public static List<string> vehicleColors = new List<string>
        {
            "Black",
            "Blue",
            "Gray",
            "Green",
            "Gold",
            "Maroon",
            "Red",
            "Silver",
            "White",
            "Yellow"                     
        };

    }
}
