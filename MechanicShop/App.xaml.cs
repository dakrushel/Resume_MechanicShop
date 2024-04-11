namespace MechanicShop
{
    public partial class App : Application
    {
        public static DateTime currentDate = DateTime.Now;
        public static DateTime MaxDate = currentDate.AddDays(30);
        public static string TodayDate = currentDate.ToString("yyyy-MM-dd");

        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light; // force light mode for app
            MainPage = new AppShell();

            
        }

    }
}
