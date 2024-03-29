using MechanicShop.Models;
using Microsoft.Extensions.Logging;

namespace MechanicShop
{
    public static class MauiProgram
    {
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

            Vehicle TestCar = new Vehicle("123T", "Test", "Model", "Purple", 1994, "124-555-8888");
            MechanicShopSQLite mechanicShopDB = new MechanicShopSQLite();

            mechanicShopDB.AddVehicle(TestCar);



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
