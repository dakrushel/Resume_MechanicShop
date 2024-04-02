namespace MechanicShop.Views;

public partial class AppointmentView : ContentPage
{
	public static DateTime currentDate = DateTime.Now;
	public static string TodayDate = currentDate.ToString("yyyy-MM-dd");
	public AppointmentView()
	{
		InitializeComponent();
		DateDisplay.Text = TodayDate;
	}
}