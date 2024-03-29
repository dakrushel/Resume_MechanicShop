namespace MechanicShop.Models
{



    public class Constant
    {

        public const string DatabaseFilename = @"..\..\..\..\..\Resources\Raw\MechanicShopTest.db3";

        public static string DatabasePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFilename);
    }
}
