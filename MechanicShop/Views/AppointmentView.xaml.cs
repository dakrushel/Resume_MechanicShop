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
        problemDescriptionEntry.Text = null;
        priceEstimate.Text = null;
        datePicker.Date = currentDate.AddDays(7);
       

        RefreshAppointments();


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
            c = MauiProgram.ShopDB.GetACustomerByName(repairOrder.CustomerName);
            v = MauiProgram.ShopDB.GetVehicleByVIN(repairOrder.VIN);
            // bind info to appointmenmt slip
            cInformation.BindingContext = c;
            vInformation.BindingContext = v;
            problemDescriptionEntry.Text = repairOrder.RepairOrderDescription;
            datePicker.Date = DateTime.Parse(repairOrder.AppointmentDate);
            //initialize reschedule button
            
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
    private void clearSearchForm_Clicked(object sender, EventArgs e)
    {
        apptName.Text = null;
        apptPhone.Text = null;
    }

    private void findAppointment_Clicked(object sender, EventArgs e)
    {
        //TODO
    }
    //======================================================================================
    //+++APPOINTMENT SLIP+++
    //======================================================================================
    
    private void removeJobBtn_Clicked(object sender, EventArgs e)
    {
        updateAppt.IsEnabled = true;
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
                //TODO
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
        updateAppt.IsEnabled = true;
        
    }
    private async void makeRO_Clicked(object sender, EventArgs e)
    {
        if (repairOrder != null && c != null && v != null)
        {
            repairOrder.IsActive = true;
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
            Pass.PassCustomer(c);
            Pass.PassVehicle(v);
            Pass.PassRO(repairOrder);
            await Shell.Current.GoToAsync("//OrderView");
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
        updateAppt.IsEnabled = true;
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

    
    //====================================================================================================
    // INPUT VALIDATORS
    //====================================================================================================
    private void apptName_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        if (e.NewTextValue == null || e.NewTextValue == "") { return; }
        // Regular expression pattern to allow only alphabetic characters, space, hyphen, apostrophe, and period
        string pattern = @"^[a-zA-ZÀ-ÿ\s'\-\.\,]+$";

        // Check if the entered text matches the pattern
        if (!Regex.IsMatch(e.NewTextValue, pattern))
        {
            // If the entered text contains disallowed characters, remove them
            var newText = Regex.Replace(e.NewTextValue, @"[^a-zA-ZÀ-ÿ\s'\-\.\,]", "");

            // Update the entry's text with the sanitized text
            entry.Text = newText;
        }
    }
    private void apptPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        if (e.NewTextValue == null) { return; }
        // Remove non-digit characters
        var newText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        // Limit maximum length to 10 digits
        if (newText.Length > 10)
        {
            newText = newText.Substring(0, 10);
        }
        // Automatically insert dashes
        if (newText.Length > 3)
        {
            newText = newText.Insert(3, "-");
            if (newText.Length > 7)
            {
                newText = newText.Insert(7, "-");
            }
        }
        entry.Text = newText;
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
        updateAppt.IsEnabled = true;
    }

    


    

    
}