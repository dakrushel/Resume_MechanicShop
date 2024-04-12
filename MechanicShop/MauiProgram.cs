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

            ////FOR MAUR TESTING
            //void PrintToTxt(string s)
            //{
            //    using (StreamWriter sw = new StreamWriter(Constant.TestPath))
            //    {
            //        sw.Write(s);
            //    }
            //}
            ////1 for Test Testy (assigned) and 4 for Tobor Human-Man (unassigned)
            //Technician testTech = ShopDB.GetTechnician(4);
            //PrintToTxt(Technician.RemoveTechChecker(testTech).ToString());


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
