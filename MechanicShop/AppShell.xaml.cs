using MechanicShop.Views;
namespace MechanicShop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AppointmentView), typeof(AppointmentView));
            Routing.RegisterRoute(nameof(OrderView), typeof(OrderView));


        }
        
    }
}
