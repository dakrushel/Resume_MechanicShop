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

            Customer Bob = new Customer("Bob", "123 Lane", "123-456-789");
            
            MechanicShopSQLite mechanicShopDB = new MechanicShopSQLite();

            mechanicShopDB.AddCustomer(Bob);


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
