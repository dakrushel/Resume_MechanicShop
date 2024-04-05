using MechanicShop.Services;
using MechanicShop.Models;
using System.Collections.ObjectModel;
namespace MechanicShop.Views;


public partial class AppointmentView : ContentPage
{
	public static DateTime currentDate = DateTime.Now;
	public static string TodayDate = currentDate.ToString("yyyy-MM-dd");
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
        }
        datePicker.Date = currentDate;
        var appointments = new ObservableCollection<RepairOrder>(MauiProgram.ShopDB.GetAllRepairOrders());
        activeAppointments.ItemsSource = appointments;


    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        
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




    private void RefreshROJobs()
    {
        repairOrderJobs.ItemsSource = thisJobs;
        removeJobBtn.Text = "Remove Job";
        removeJobBtn.IsEnabled = false;
    }

    private void scheduleAppointment_Clicked(object sender, EventArgs e)
    {
        //gather information needed for appointment
        string? description = problemDescriptionEntry.Text;
        string? createDate = TodayDate;
        string? appointmentDate = datePicker.Date.ToString("yyyy-MM-dd");
        string? VIN = v?.VIN;

        if (description != null && createDate != null && appointmentDate != null && VIN != null)
        {
            RepairOrder newAppointment = new RepairOrder(description, createDate, appointmentDate, VIN);
            foreach (ServiceJob job in thisJobs)
            {
                newAppointment.AssignServiceJob(job.ServiceJobId);
            }
            OnAppearing();
        }
        
    }
}