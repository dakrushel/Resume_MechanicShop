using MechanicShop.Models;
using Microsoft.Extensions.Logging;

namespace MechanicShop
{
    public static class MauiProgram
    {
        public static MechanicShopSQLite ShopDB;
        public static List<int> yearList = new List<int>();
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
            Customer Bob = new Customer("Bob", "123 Lane", "123-456-7890");
            Customer Bill = new Customer("Bill", "123 Lane", "124-555-8888");
            Customer Jane = new Customer("Jane", "124 Avenue", "999-000-9999");

            ServiceJob test = new ServiceJob("12A", "Oil Change", 0.5);

            test.ServiceJobDescription = "Tire Change"; 

            ShopDB = new MechanicShopSQLite();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
