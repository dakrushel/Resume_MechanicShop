namespace MechanicShop.Models
{



    public class Constant
    {

        public const string DatabaseFilename = @"..\..\..\..\..\Resources\Raw\MechanicShopTest.db3";
        public const string RepairOrderFilename = @"..\..\..\..\..\Resources\Raw\RepairOrder.txt";

        public static string DatabasePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFilename);
        public static string RepairOrderPath => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RepairOrderFilename);
    }
}
