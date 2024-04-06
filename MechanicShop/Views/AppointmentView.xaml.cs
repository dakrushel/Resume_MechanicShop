using MechanicShop.Services;
using MechanicShop.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MechanicShop.Views;


public partial class AppointmentView : ContentPage
{
    //Date variables
	public static DateTime currentDate = DateTime.Now;
    public static DateTime MaxDate = currentDate.AddDays(30);
	public static string TodayDate = currentDate.ToString("yyyy-MM-dd");

    // Page temp variables (for currently displayed repair order)
	public static Customer? c = null;
	public static Vehicle? v = null;
    public static RepairOrder? repairOrder = null;   
    public  static ObservableCollection<ServiceJob> thisJobs = new ObservableCollection<ServiceJob>();

	public AppointmentView()
	{
		InitializeComponent();
		DateDisplay.Text = TodayDate;

        var allJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.GetAllServiceJobs());
        serviceJobs.ItemsSource = allJobs;
        
    }
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        if (Pass.CustomerPass != null && Pass.VehiclePass != null)
        {
            //widgets
            appointmentSlip.IsEnabled = true;
            appointmentLogs.IsEnabled = false;

            // page temp objects
            c = Pass.RetreiveCustomer();         
            v = Pass.RetrieveVehicle();

            //Binding Contexts
            cInformation.BindingContext = c;
            vInformation.BindingContext = v;           
        }
        else 
        { 
            // page temp objects
            cInformation.BindingContext = null;
            vInformation.BindingContext = null;
            thisJobs.Clear();

            // Widgets
            appointmentSlip.IsEnabled = false;
            appointmentLogs.IsVisible = true;
            appointmentLogs.IsEnabled = true;
            serviceJobMenu.IsVisible = false;
            problemDescriptionEntry.Text = null;
        }
        datePicker.Date = currentDate.AddDays(7);

        //var appointments = new ObservableCollection<RepairOrder>(MauiProgram.ShopDB.GetAllRepairOrders());       
        //activeAppointments.ItemsSource = appointments;
        AppointmentViewService.refreshAppointments();
        activeAppointments.ItemsSource = AppointmentViewService.upcomingAppointments;
        expiredAppointments.ItemsSource = AppointmentViewService.expiredAppointments;


    }
    private void RefreshROJobs()
    {
        repairOrderJobs.ItemsSource = thisJobs;
        removeJobBtn.Text = "Remove Job";
        removeJobBtn.IsEnabled = false;
    }


    //======================================================================================
    //+++APPOINTMENT SLIP+++
    //======================================================================================

    private void removeJobBtn_Clicked(object sender, EventArgs e)
    {
        ServiceJob? toRemove = repairOrderJobs.SelectedItem as ServiceJob;
        if (toRemove != null)
        {
            thisJobs.Remove(toRemove);
            RefreshROJobs();
        }     
        removeJobBtn.IsEnabled = false;
    }

    private void addJobBtn_Clicked(object sender, EventArgs e)
    {
        appointmentLogs.IsVisible = false;
        serviceJobMenu.IsVisible = true;
        var allJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.GetAllServiceJobs());
        serviceJobs.ItemsSource = allJobs;
        addJobBtn.IsEnabled = false;
    }
    private void repairOrderJobs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        ServiceJob? sj = e.SelectedItem as ServiceJob;
        if (sj != null)
        {
            removeJobBtn.IsEnabled = true;
            removeJobBtn.Text = $"Remove {sj.ServiceJobDescription}";
        }
        
    }
    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        var editor = (Editor)sender;
        if (string.IsNullOrEmpty(e.NewTextValue)) { return; }
        // Regular expression pattern to match disallowed characters
        string pattern = @"[^a-zA-ZÀ-ÿ\s'\-.,\d]";

        // Check if the entered text contains disallowed characters
        if (Regex.IsMatch(e.NewTextValue, pattern))
        {
            // If disallowed characters are found, remove them
            var newText = Regex.Replace(e.NewTextValue, pattern, "");

            // Update the entry's text with the sanitized text
            editor.Text = newText;
        }
    }
    private void scheduleAppointment_Clicked(object sender, EventArgs e)
    {
        //gather information needed for appointment
        string? description = problemDescriptionEntry.Text;
        string? createDate = TodayDate;
        string? appointmentDate = datePicker.Date.ToString("yyyy-MM-dd");
        string? VIN = v?.VIN;
        string? customerName = c?.Name;

        if (createDate != null && appointmentDate != null && VIN != null && customerName != null)
        {
            RepairOrder newAppointment = new RepairOrder(description, createDate, appointmentDate, VIN, customerName);
            foreach (ServiceJob job in thisJobs)
            {
                newAppointment.AssignServiceJob(job.ServiceJobId);
            }
            OnAppearing();
        }
    }
    //======================================================================================
    //+++Service Job Menu+++
    //======================================================================================
    private void serviceJobs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        ServiceJob? sj = e.SelectedItem as ServiceJob;
        if (sj != null)
        {
            thisJob.BindingContext = sj;
            if (thisJobs.Contains(sj))
            {
                addThisJob.IsEnabled = false;
                return;
            }
            addThisJob.IsEnabled = true;
        }
    }
    private void closeServiceJobMenu_Clicked(object sender, EventArgs e)
    {
        appointmentLogs.IsVisible = true;
        serviceJobMenu.IsVisible = false;
        addJobBtn.IsEnabled = true;
    }
    private void addThisJob_Clicked(object sender, EventArgs e)
    {
        ServiceJob? sj = thisJob.BindingContext as ServiceJob;
        if (sj != null)
        {
            if (thisJobs.Contains(sj))
            {
                return;
            }
            thisJobs.Add(sj);
            addThisJob.IsEnabled = false;
            RefreshROJobs();
        }
        
    }

    private void appointments_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        appointmentSlip.IsEnabled = true;

    }
}