using MechanicShop.Services;
using MechanicShop.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.Maui.Animations;

namespace MechanicShop.Views;

/* 
 * Appointment View page code behind (Written by Chloe) 
    This code hanldles information gathering and display,
    Receiving info from Custoemr View,
    Passing info to Repair Order View,
    Button presses, list item selections etc
    ensuring correctly related objects are displayed together
    Input validation
 */

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
    }
    //======================================================================================
    // PAGE SETUP AND REFRESH
    //======================================================================================
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        // clear page list of jobs
        thisJobs.Clear();
        // Set up page depending on if its being passed information from CustomerView or not
        if (Pass.CustomerPass != null && Pass.VehiclePass != null)
        { // If receiving information, set the appointment slip and disable logs
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
        { // Otherwise set up standard view
            // page temp objects
            cInformation.BindingContext = null;
            vInformation.BindingContext = null;
            // Widgets
            appointmentSlip.IsEnabled = false;
            appointmentLogs.IsVisible = true;
            appointmentLogs.IsEnabled = true;
            serviceJobMenu.IsVisible = false;        
        }
        // Refresh these items regardless of setup type.
        RefreshAppointmentSlip();
        RefreshAppointments();
        RefreshSearch();
        // populate jobs in service job menu
        var allJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.GetAllServiceJobs());
        serviceJobs.ItemsSource = allJobs;
    }
    private void RefreshAppointmentSlip()
    {
        // set appointment slip back to default view
        addJobBtn.IsEnabled = true;
        problemDescriptionEntry.Text = null;
        priceEstimate.Text = null;
        datePicker.Date = currentDate.AddDays(7);
    }
    private void RefreshSearch()
    {
        // sets all aspects of the search widget to default view
        apptPhone.Text = null;
        apptName.Text = null;
        searchName.IsEnabled = false;
        SearchPhone.IsEnabled = false;
        clearSearchForm.IsEnabled = false;
    }
    private void RefreshAppointments()
    {
        // Refresh Appointment List views, Calls a method that sorts the appointments based on date.
        AppointmentViewService.refreshAppointments();
        activeAppointments.ItemsSource = AppointmentViewService.upcomingAppointments;
        expiredAppointments.ItemsSource = AppointmentViewService.expiredAppointments;
        updateAppt.IsEnabled = false;       
    }
    private void RefreshROJobs()
    {
        // Refreshes the list of jobs specifically attached to a certain order and  displays in the
        // appointment slip widget.
        repairOrderJobs.ItemsSource = thisJobs;
        removeJobBtn.Text = "Remove Job";
        removeJobBtn.IsEnabled = false;
        if (thisJobs != null)
        {
            // if the list has any jobs, this method calculates the crice estimate based on jobs in the list * shop rate
            priceEstimate.Text = AppointmentViewService.CalculateEstimate(thisJobs.ToList()).ToString("C");
        }      
    }
    //======================================================================================
    //+++APPOINTMENT LISTS+++
    //======================================================================================
    private void appointments_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {       
        appointmentSlip.IsEnabled = true; // Enable appointment slip
        repairOrder = e.SelectedItem as RepairOrder; // Set page repair order variable   
        if (repairOrder != null) // ensure an order is set
        {
            // set other local page variables based on repairOrder information (Foreign Keys)
            c = MauiProgram.ShopDB.GetACustomerByPhone(repairOrder.CustomerPhoneNumber);
            v = MauiProgram.ShopDB.GetVehicleByVIN(repairOrder.VIN);
            // bind all relevant info to appointmenmt slip
            cInformation.BindingContext = c;
            vInformation.BindingContext = v;
            problemDescriptionEntry.Text = repairOrder.RepairOrderDescription;
            datePicker.Date = DateTime.Parse(repairOrder.AppointmentDate);
            //populate list of jobs assigned to this appointment
            thisJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.
                GetServiceJobListByRepairOrderId(repairOrder.RepairOrderId));
            RefreshROJobs(); // Call refresh to populate list in GUI
            // swap buttons
            scheduleAppointment.IsVisible = false;
            apptOptions.IsVisible = true;
            updateAppt.IsEnabled = false;
        }
    }
    //======================================================================================
    //+++SEARCH+++
    //======================================================================================
    private void clearSearchForm_Clicked(object sender, EventArgs e)
    {
        RefreshSearch(); // reset search form
        RefreshAppointments(); // refresh appointments without search filters
    }
    private async void SearchPhone_Clicked(object sender, EventArgs e)
    {
        // Search appointments by customer phone number
        string phone = apptPhone.Text; // get search value (inputted phone number)
        if(phone.Length == 12) // only accept a full phone number
        {
            // Call filter method to sort new lists based on phone
            AppointmentViewService.RefreshByPhone(phone);
            activeAppointments.ItemsSource = AppointmentViewService.upcomingAppointments;
            expiredAppointments.ItemsSource = AppointmentViewService.expiredAppointments;
            // Check if the returned lists have any values in them
            int? upcomingCount = AppointmentViewService.upcomingAppointments?.Count;
            int? expCount = AppointmentViewService.expiredAppointments?.Count;
            if(upcomingCount == 0 && expCount == 0)
            { // if lists have no values, Display a message that no results were found, and clear the search
                await DisplayAlert("No Results","No appointments match this phone number","Ok");
                RefreshAppointments();
            }
        }
    }
    private async void searchName_Clicked(object sender, EventArgs e)
    {
        // Search appointments by customer name
        string name = apptName.Text; // get search value (name or partial name inputted)
        if (name != null && name != "") // ensure there is a value to compare
        {
            // Call filter method to compare search value to existing appointments.
            AppointmentViewService.RefreshByName(name);
            activeAppointments.ItemsSource = AppointmentViewService.upcomingAppointments;
            expiredAppointments.ItemsSource = AppointmentViewService.expiredAppointments;
            // Check if the returned lists have any values in them
            int? upcomingCount = AppointmentViewService.upcomingAppointments?.Count;
            int? expCount = AppointmentViewService.expiredAppointments?.Count;
            if (upcomingCount == 0 && expCount == 0)
            { // if lists have no values, Display a message that no results were found, and clear the search
                await DisplayAlert("No Results", "No appointments match this Name", "Ok");
                RefreshAppointments();
            }
        }
    }
    //======================================================================================
    //+++APPOINTMENT SLIP+++
    //======================================================================================
    private async void clearSlip_Clicked(object sender, EventArgs e)
    {
        if (updateAppt.IsEnabled == true)
        {
            // if changes have been made, prompt to save changes       
            bool savechanges = await DisplayAlert("Save Changes?", "This RO has been altered, do you want to save changes before closing?", "Save Changes", "Discard Changes");
            if (savechanges)
            { // save changes if the user says to save
                UpdateAppointment();
            }
        }
        OnAppearing(); // refresh the page and all the widgets
    }
    private void removeJobBtn_Clicked(object sender, EventArgs e)
    {
        ServiceJob? toRemove = repairOrderJobs.SelectedItem as ServiceJob; // get the job to remove OBJ
        if (toRemove != null) // ensure a job is selected
        {
            thisJobs.Remove(toRemove); // remove the serviceJob from the local RO jobs list
            RefreshROJobs(); // refresh local RO jobs list in GUI
        }
        AppointmentDetailsChanged(); // alert page to wake up the 'Update Details' button  
    }
    private void addJobBtn_Clicked(object sender, EventArgs e)
    {
        appointmentLogs.IsVisible = false; // switch to job menu view
        serviceJobMenu.IsVisible = true;     
        addJobBtn.IsEnabled = false;
    }
    private void repairOrderJobs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        ServiceJob? sj = e.SelectedItem as ServiceJob; // get selected job OBJ
        if (sj != null) // ensure selection is not null
        {
            removeJobBtn.IsEnabled = true; // enable remove button and change text to alert which job
            removeJobBtn.Text = $"Remove {sj.ServiceJobDescription}";
        }      
    }   
    private void scheduleAppointment_Clicked(object sender, EventArgs e)
    {
        appointmentLogs.IsVisible = true; // swap views
        serviceJobMenu.IsVisible = false;
        addJobBtn.IsEnabled = true;
        //gather information needed for appointment
        string? description = problemDescriptionEntry.Text;
        string? createDate = TodayDate;
        string? appointmentDate = datePicker.Date.ToString("yyyy-MM-dd");
        string? VIN = v?.VIN;
        if (datePicker.Date < currentDate.AddDays(-1))
        { // error if date is passed
            NotifyDateError();
            return;
        }
        if (createDate != null && appointmentDate != null && VIN != null) // ensure all needed info is present
        {
            // create a new repaiororder object
            RepairOrder newAppointment = new RepairOrder(description, createDate, appointmentDate, VIN);
            foreach (ServiceJob job in thisJobs)
            { // assign all jobs currebntly in the list to the new order
                newAppointment.AssignServiceJob(job.ServiceJobId);
            }
            repairOrder = newAppointment; // set new order to local page repairOrder variable        
            appointmentLogs.IsEnabled = true;
            RefreshAppointments();      // enable and refresh logs (To add the new job)     
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
                OnAppearing(); // delete Ro and refresh page
            }
        }
    }
    private void updateAppt_Clicked(object sender, EventArgs e)
    {
        UpdateAppointment(); // one of two ways to call updateAppointment();
    }
    private void UpdateAppointment()
    {
        updateAppt.IsEnabled = false; //disable update button
        if (repairOrder != null) // ensure there is a repairOrder available to update
        {       
            repairOrder.RepairOrderDescription = problemDescriptionEntry.Text; //update problem description          
            if (datePicker.Date < currentDate.AddDays(-1)) // Check for valid date (Greater than current date)
            {
                NotifyDateError(); // Error and return if invalid
                return;
            }
            string? appointmentDate = datePicker.Date.ToString("yyyy-MM-dd");
            if (appointmentDate != null)
            { // set date if valid
                repairOrder.AppointmentDate = appointmentDate;
            }
            //update list of jobs for this RO
            MauiProgram.ShopDB.RemoveRepairOrderServiceJobBridgeAttachedToRepairOrder(repairOrder.RepairOrderId);
            foreach (ServiceJob job in thisJobs)
            {
                repairOrder.AssignServiceJob(job.ServiceJobId);
            }
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder); // update the Database object
            RefreshAppointments(); // refresh appointments lists
        }
    }
    private async void NotifyDateError()
    { // called when an invalid date is processed
        await DisplayAlert("Invalid Date", "Cannot scedule appointment with a past date", "Ok");
    }
    private void datePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (repairOrder != null)
        {          
            DateTime roDate = DateTime.Parse(repairOrder.AppointmentDate);
            if (roDate != datePicker.Date && datePicker.Date >= currentDate.AddDays(-1))
            { // if new date is valid, notify appointment details changed
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
        if (repairOrder != null && c != null && v != null) // ensure necessary variables are present
        {
            // Set the RO to active (classifying it as Repair Order rather and Appointment)
            repairOrder.IsActive = true;
            // Update OBJ in DB
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
            Pass.PassCustomer(c);
            Pass.PassVehicle(v); // pass info to Order View Page
            Pass.PassRO(repairOrder);
            // Go to REPAIR ORDER VIEW page
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
            // bind job to display labels & enable add button
            thisJob.BindingContext = sj;
            addThisJob.IsEnabled = true;
        }
    }
    private void closeServiceJobMenu_Clicked(object sender, EventArgs e)
    {
        // Reset to standard view
        appointmentLogs.IsVisible = true;
        serviceJobMenu.IsVisible = false;
        addJobBtn.IsEnabled = true;
    }
    private void addThisJob_Clicked(object sender, EventArgs e)
    {
        ServiceJob? sj = thisJob.BindingContext as ServiceJob; // get job OBJ
        if (sj != null) // ensure an OBJ is selected
        {
            thisJobs.Add(sj); // add to local list of Ro jobs
            addThisJob.IsEnabled = false; // diasble button
            RefreshROJobs(); // refresh local Ro job list
        }
        AppointmentDetailsChanged(); // notify details changed (for update)
    }
    //==================================================================================
    // INPUT VALIDATION
    //==================================================================================
    // For more info on how these work, see the Validation class in 'Services' folder
    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        problemDescriptionEntry.Text = Validate.Description(e.NewTextValue);
        AppointmentDetailsChanged();
    }
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
                apptPhone.TextColor = Color.FromArgb("080D10");
                SearchPhone.IsEnabled = true;
                return;
            }
        }
        apptPhone.TextColor = Color.FromArgb("FF0000");
        SearchPhone.IsEnabled = false;
    }

}