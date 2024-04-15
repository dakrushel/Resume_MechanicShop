namespace MechanicShop.Models
{



    public class Constant
    {
        //Constant for DB location
        public const string DatabaseFilename = @"..\..\..\..\..\Resources\Raw\MechanicShopTest.db3";

        //Not actually a constant anymore but this is where it was when repairOrderFilename was a constant
        public static string repairOrderFilename = @"..\..\..\..\..\Resources\Raw\Invoices\";

        //FOR TESTING
        public const string TestFilename = @"..\..\..\..\..\Resources\Raw\Test.txt";

        //Creating a string to the DB location
        public static string DatabasePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFilename);
        public static string RepairOrderPath => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, repairOrderFilename);
        public static string TestPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestFilename);

    }
}
