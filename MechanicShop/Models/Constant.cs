namespace MechanicShop.Models
{



    public class Constant
    {

        public const string DatabaseFilename = @"..\..\..\..\..\Resources\Raw\MechanicShopTest.db3";

        //Not actually a constant but this is where it was when repairOrderFilename was a constant
        public static string repairOrderFilename = @"..\..\..\..\..\Resources\Raw\";


        public static string DatabasePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFilename);
        public static string RepairOrderPath => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, repairOrderFilename);

    }
}
