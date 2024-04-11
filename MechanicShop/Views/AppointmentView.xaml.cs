using MechanicShop.Services;
using MechanicShop.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.Maui.Animations;

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
    public static ObservableCollection<ServiceJob> thisJobs = new ObservableCollection<ServiceJob>();

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
            apptOptions.IsVisible = false;
            scheduleAppointment.IsVisible = true;            
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
            // Widgets
            appointmentSlip.IsEnabled = false;
            appointmentLogs.IsVisible = true;
            appointmentLogs.IsEnabled = true;
            serviceJobMenu.IsVisible = false;
            
        }
        thisJobs.Clear();
        addJobBtn.IsEnabled = true;
        problemDescriptionEntry.Text = null;
        priceEstimate.Text = null;
        datePicker.Date = currentDate.AddDays(7);
        RefreshAppointments();
        RefreshSearch();

    }
    private void RefreshSearch()
    {
        apptPhone.Text = null;
        apptName.Text = null;
        searchName.IsEnabled = false;
        SearchPhone.IsEnabled = false;
        clearSearchForm.IsEnabled = false;
    }
    private void RefreshAppointments()
    {
        AppointmentViewService.refreshAppointments();
        activeAppointments.ItemsSource = AppointmentViewService.upcomingAppointments;
        expiredAppointments.ItemsSource = AppointmentViewService.expiredAppointments;
        updateAppt.IsEnabled = false;       
    }
    private void RefreshROJobs()
    {
        repairOrderJobs.ItemsSource = thisJobs;
        removeJobBtn.Text = "Remove Job";
        removeJobBtn.IsEnabled = false;
        if (thisJobs != null)
        {
            priceEstimate.Text = AppointmentViewService.CalculateEstimate(thisJobs.ToList()).ToString("C");
        }      
    }
    //======================================================================================
    //+++APPOINTMENT LISTS+++
    //======================================================================================
    private void appointments_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        appointmentSlip.IsEnabled = true;
        repairOrder = e.SelectedItem as RepairOrder;
        
        if (repairOrder != null)
        {
            // set local variables
            c = MauiProgram.ShopDB.GetACustomerByPhone(repairOrder.CustomerPhoneNumber);
            v = MauiProgram.ShopDB.GetVehicleByVIN(repairOrder.VIN);
            // bind info to appointmenmt slip
            cInformation.BindingContext = c;
            vInformation.BindingContext = v;
            problemDescriptionEntry.Text = repairOrder.RepairOrderDescription;
            datePicker.Date = DateTime.Parse(repairOrder.AppointmentDate);
            //populate list of jobs assigned to this appointment
            thisJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.
                GetServiceJobListByRepairOrderId(repairOrder.RepairOrderId));
            RefreshROJobs();
            // swap buttons
            scheduleAppointment.IsVisible = false;
            apptOptions.IsVisible = true;
            updateAppt.IsEnabled = false;
        }
    }

    //======================================================================================
    //+++SEARCH+++
    //======================================================================================
    private void apptName_TextChanged(object sender, TextChangedEventArgs e)
    {
        clearSearchForm.IsEnabled = true;
        apptName.Text = Validate.Name(e.NewTextValue);
        searchName.IsEnabled = true;
    }
    private void apptPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        clearSearchForm.IsEnabled = true;
        apptPhone.Text = Validate.Phone(e.NewTextValue);
        if (apptPhone.Text != null)
        {
            if (apptPhone.Text.Length == 12)
            {
                SearchPhone.IsEnabled = true;
                return;
            }
        }
        SearchPhone.IsEnabled = false;

    }
    private void clearSearchForm_Clicked(object sender, EventArgs e)
    {
        apptName.Text = null;
        apptPhone.Text = null;
        clearSearchForm.IsEnabled = false;
        RefreshAppointments();
    }
    private async void SearchPhone_Clicked(object sender, EventArgs e)
    {
        // Search customer by phone number
        // only accept whole phone number BC the dashes will be in the right place
        string phone = apptPhone.Text; // get ssearch value
        if(phone.Length == 12)
        {
            // Call filter method to sort new lists based on phone
            AppointmentViewService.RefreshByPhone(phone);
            activeAppointments.ItemsSource = AppointmentViewService.upcomingAppointments;
            expiredAppointments.ItemsSource = AppointmentViewService.expiredAppointments;
            int? upcomingCount = AppointmentViewService.upcomingAppointments?.Count;
            int? expCount = AppointmentViewService.expiredAppointments?.Count;
            if(upcomingCount == 0 && expCount == 0)
            {
                await DisplayAlert("No Results","No appointments match this phone number","Ok");
                RefreshAppointments();
            }
        }
    }
    private void searchName_Clicked(object sender, EventArgs e)
    {
        //TODO once method exists
    }
    //======================================================================================
    //+++APPOINTMENT SLIP+++
    //======================================================================================
    private async void clearSlip_Clicked(object sender, EventArgs e)
    {
        if (updateAppt.IsEnabled == true)
        {
            bool savechanges = await DisplayAlert("Save Changes?", "This RO has been altered, do you want to save changes before closing?", "Save Changes", "Discard Changes");
            if (savechanges)
            {
                UpdateAppointment();
            }
        }
        OnAppearing();
    }
    private void removeJobBtn_Clicked(object sender, EventArgs e)
    {
        ServiceJob? toRemove = repairOrderJobs.SelectedItem as ServiceJob;
        if (toRemove != null)
        {
            thisJobs.Remove(toRemove);
            RefreshROJobs();
        }
        removeJobBtn.IsEnabled = false;
        AppointmentDetailsChanged();
        
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
    private void scheduleAppointment_Clicked(object sender, EventArgs e)
    {
        appointmentLogs.IsVisible = true;
        serviceJobMenu.IsVisible = false;
        addJobBtn.IsEnabled = true;
        //gather information needed for appointment
        string? description = problemDescriptionEntry.Text;
        string? createDate = TodayDate;
        string? appointmentDate = datePicker.Date.ToString("yyyy-MM-dd");
        string? VIN = v?.VIN;
        if (datePicker.Date < currentDate.AddDays(-1))
        {
            NotifyDateError();
            return;
        }
        if (createDate != null && appointmentDate != null && VIN != null)
        {
            RepairOrder newAppointment = new RepairOrder(description, createDate, appointmentDate, VIN);
            foreach (ServiceJob job in thisJobs)
            {
                newAppointment.AssignServiceJob(job.ServiceJobId);
            }
            repairOrder = newAppointment;           
            appointmentLogs.IsEnabled = true;
            RefreshAppointments();           
            // swap out button set
            scheduleAppointment.IsVisible = false;
            apptOptions.IsVisible = true;
        }
    }  
    private async void deleteAppt_Clicked(object sender, EventArgs e)
    {
        if (repairOrder != null)
        {
            // confirmation message to prevent accidental deletion
            bool delete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this appointment?", "Delete Appointment", "Cancel");
            if (delete)
            {              
                MauiProgram.ShopDB.RemoveRepairOrder(repairOrder.RepairOrderId);                
                OnAppearing();
            }
        }
    }
    private void updateAppt_Clicked(object sender, EventArgs e)
    {
        UpdateAppointment();
    }
    private void UpdateAppointment()
    {
        updateAppt.IsEnabled = false;
        if (repairOrder != null)
        {
            //update problem description
            repairOrder.RepairOrderDescription = problemDescriptionEntry.Text;
            // update appointment date
            if (datePicker.Date < currentDate.AddDays(-1))
            {
                NotifyDateError();
                return;
            }
            string? appointmentDate = datePicker.Date.ToString("yyyy-MM-dd");
            if (appointmentDate != null)
            {
                repairOrder.AppointmentDate = appointmentDate;
            }
            //update list of jobs for this RO
            MauiProgram.ShopDB.RemoveRepairOrderServiceJobBridgeAttachedToRepairOrder(repairOrder.RepairOrderId);
            foreach (ServiceJob job in thisJobs)
            {
                repairOrder.AssignServiceJob(job.ServiceJobId);
            }
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
            RefreshAppointments();
        }
    }
    private async void NotifyDateError()
    {
        await DisplayAlert("Invalid Date", "Cannot scedule appointment with a past date", "Ok");
    }
    private void datePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (repairOrder != null)
        {          
            DateTime roDate = DateTime.Parse(repairOrder.AppointmentDate);
            if (roDate != datePicker.Date && datePicker.Date >= currentDate.AddDays(-1))
            {
                AppointmentDetailsChanged();
                return;
            }
        }
    }
    private void AppointmentDetailsChanged()
    {
        // Called when description, jobs, or date are changed,
        // Activates update button
        updateAppt.IsEnabled = true;      
    }
    private async void makeRO_Clicked(object sender, EventArgs e)
    {
        if (repairOrder != null && c != null && v != null)
        {
            // Set the RO ti active (classifying it as Repair Order rather and Appointment)
            repairOrder.IsActive = true;
            // Update OBJ in DB
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
            Pass.PassCustomer(c);
            Pass.PassVehicle(v);
            Pass.PassRO(repairOrder);
            // Go to REPAIR ORDER VIEW page
            await Shell.Current.GoToAsync("//OrderView");
        }
    }
    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        problemDescriptionEntry.Text = Validate.Description(e.NewTextValue);
        AppointmentDetailsChanged();
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
            thisJobs.Add(sj);
            addThisJob.IsEnabled = false;
            RefreshROJobs();
        }
        AppointmentDetailsChanged();
    }

    
}