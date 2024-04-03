namespace MechanicShop
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light; // force light mode for app
            MainPage = new AppShell();
        }
    }
}
