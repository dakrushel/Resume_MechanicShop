using MechanicShop.Services;
using MechanicShop.Models;
namespace MechanicShop.Views;


public partial class AppointmentView : ContentPage
{
	public static DateTime currentDate = DateTime.Now;
	public static string TodayDate = currentDate.ToString("yyyy-MM-dd");
	public static Customer? c = null;
	public static Vehicle? v = null;

	public AppointmentView()
	{
		InitializeComponent();
		DateDisplay.Text = TodayDate;

	}
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        if (Pass.CustomerPass != null && Pass.VehiclePass != null)
        {
            appointmentSlip.IsEnabled = true;
            appointmentLogs.IsEnabled = false;
            c = Pass.RetreiveCustomer();
            cInformation.BindingContext = c;
            v = Pass.RetrieveVehicle();
            vInformation.BindingContext = v;           
        }
        else 
        { 
            cInformation.BindingContext = null;
            vInformation.BindingContext = null;
            appointmentSlip.IsEnabled = false;
            appointmentLogs.IsEnabled = true;
        }
        
        
        
    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        
    }
}