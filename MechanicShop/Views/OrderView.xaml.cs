using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MechanicShop.Models;
using MechanicShop.Services;
using Microsoft.Maui.Animations;
using Microsoft.Maui.ApplicationModel;
/* 
 * Order View page code behind (Written by Chloe) 
    This page handles logic for seperating lists, as well as assigning Technicians and
    closing our repair orders. You can do much of the same things as the Appointment View page,
    But this page is meant to be representative of cars that have been checked into the shop
    - When a technician is assigned, they will not show in the list of availale techs
    - Once an RO is closed, that tech will be marked as available again

    This page also handles all of the checks to make sure a repair order has everything is needs
    before you're able to close it out.
 */
namespace MechanicShop.Views;
public partial class OrderView : ContentPage
{
    // Page temp variables (for currently displayed repair order)
    public static Customer? c = null;
    public static Vehicle? v = null;
    public static Technician? t = null;
    public static RepairOrder? repairOrder = null;
    public static ObservableCollection<ServiceJob> thisJobs = new ObservableCollection<ServiceJob>();
    public OrderView()
	{
		InitializeComponent();
	}
    //========================================================================================================
    //REFRESH
    //========================================================================================================
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        //refresh components
        RefreshTechList();
        RefreshServiceJobs();
        RefreshROs();
        removeTech.IsEnabled = false;
        updateRO.IsEnabled = false;
        selectedTech.Text = ":::Not Assigned:::";    
        if (Pass.CustomerPass != null && Pass.VehiclePass != null)
        { // If receiving a customer from PASS. set up PASS view
            // page temp objects
            c = Pass.RetreiveCustomer();
            v = Pass.RetrieveVehicle();
            repairOrder = Pass.RetrieveRO();
            //Binding Contexts
            if (repairOrder != null && c != null && v != null)
            {
                cInformation.BindingContext = c;
                vInformation.BindingContext = v;
                thisJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.
                    GetServiceJobListByRepairOrderId(repairOrder.RepairOrderId));
                repairOrderJobs.ItemsSource = thisJobs;
                problemDescriptionEntry.Text = repairOrder.RepairOrderDescription;
            }  
            //Widget Visibility
            orderSlip.IsVisible = true;
            roLogs.IsVisible = false;
            serviceJobMenu.IsEnabled = true;
        }
        else
        { // Otherwise set up standard view
            // Clear all page variables
            c = null;
            v = null;
            t = null;
            repairOrder = null;
            thisJobs.Clear();
            //Binding Contexts
            cInformation.BindingContext = null;
            vInformation.BindingContext = null;
            //widget visibility
            orderSlip.IsVisible = false;
            roLogs.IsVisible = true;
            serviceJobMenu.IsEnabled = false;
        }
    }
    private void RefreshTechList()
    {
        AppointmentViewService.RefreshOpenTechnicians();
        techList.ItemsSource = AppointmentViewService.openTechnicians;
        techList.SelectedItem = null;
        assignTech.IsEnabled = false;
        assignTech.Text = "Assign Technician";
    }
    private void RefreshServiceJobs()
    {
        var allJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.GetAllServiceJobs());
        serviceJobs.ItemsSource = allJobs;
        addThisJob.IsEnabled = false;
        thisJob.BindingContext = null;
    }
    private void RefreshROs()
    {
        AppointmentViewService.refreshROs(); // refresh sorted lists in the logic layer
        unasignedROs.ItemsSource = AppointmentViewService.unassigned; // bind lists to GUI
        inProgressROs.ItemsSource = AppointmentViewService.inProgress;
    }
    //========================================================================================================
    //-----ORDER SLIP
    //========================================================================================================
    private async void DeleteRO_Clicked(object sender, EventArgs e)
    {
        if (repairOrder != null) // check that order is not null
        {
            // confirmation message to prevent accidental deletion
            bool delete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this Repair Order?", "Delete RO", "Cancel");
            if (delete)
            { // delete the order and refresh the page
                MauiProgram.ShopDB.RemoveRepairOrder(repairOrder.RepairOrderId);
                OnAppearing();
            }
        }
    }
    private void updateRO_Clicked(object sender, EventArgs e)
    {
        UpdateRo(); //One of two ways to update Order
    }
    private void UpdateRo()
    {
        updateRO.IsEnabled = false; // disable update button
        if (repairOrder != null) // ensure RO not null
        {
            repairOrder.RepairOrderDescription = problemDescriptionEntry.Text; //update problem description
            // remove and replace repair order jobs to reflect current job list
            MauiProgram.ShopDB.RemoveRepairOrderServiceJobBridgeAttachedToRepairOrder(repairOrder.RepairOrderId);
            foreach (ServiceJob job in thisJobs)
            {
                repairOrder.AssignServiceJob(job.ServiceJobId);
            }
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder); // updare RO in Database
        }
    }
    private void repairOrderJobs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        ServiceJob? sj = e.SelectedItem as ServiceJob; // get job OBJ
        if (sj != null) // ensure not null
        {
            removeJobBtn.IsEnabled = true; // enable remove button and change text for UX
            removeJobBtn.Text = $"Remove {sj.ServiceJobDescription}";
        }
    }
    private void removeJobBtn_Clicked(object sender, EventArgs e)
    {
        removeJobBtn.Text = "Remove Job"; // reset  remove button
        updateRO.IsEnabled = true; // enable update RO button
        ServiceJob? toRemove = repairOrderJobs.SelectedItem as ServiceJob;
        if (toRemove != null) // ensure job selected
        {
            thisJobs.Remove(toRemove); // remove job from list
            RefreshROJobs(); // refresh display

        }
        removeJobBtn.IsEnabled = false; // diasble remove button
    }
    private async void closeRO_Clicked(object sender, EventArgs e)
    {
        //this will delete the OBj and generate invoice. Need to make sure all info there (tech, jobs, etc)
        if (repairOrder != null)
        {
            //update problem description
            repairOrder.RepairOrderDescription = problemDescriptionEntry.Text;
            // update joblist
            MauiProgram.ShopDB.RemoveRepairOrderServiceJobBridgeAttachedToRepairOrder(repairOrder.RepairOrderId);
            foreach (ServiceJob job in thisJobs)
            {
                repairOrder.AssignServiceJob(job.ServiceJobId);
            }
            // RepairOrderHours
            if (thisJobs == null)
            {
                await DisplayAlert("Add Job", "You must add a job to close this Repair Order", "Ok");
                return;
            }
            // set finalized hours
            repairOrder.RepairOrderHours = AppointmentViewService.GetHours(thisJobs.ToList());
            // set finalized close date
            repairOrder.DateClose = AppointmentView.TodayDate;
            // update the RO object in DB
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
            //invoice the RO
            GenerateInvoice.SaveInvoiceDelRO(repairOrder);
            //Delete the RO (This happens in invoice method)
            await DisplayAlert("Invoice Created", "The Repair Order has been closed and invoiced", "Ok");
            OnAppearing(); // refresh page
        }
    }
    private async void Clear_Clicked(object sender, EventArgs e)
    {
        if (updateRO.IsEnabled == true) // check if changes were made to the repair order
        { // if so, prompt user to save changes first
            bool savechanges = await DisplayAlert("Save Changes?","This RO has been altered, do you want to save changes before closing?","Save Changes","Discard Changes");
            if (savechanges)
            {
                UpdateRo(); // save if they say they want to save
            }
        }      
        OnAppearing(); // refresh the page
    }
    private void RefreshROJobs()
    {
        repairOrderJobs.ItemsSource = thisJobs; // bind lical joblist to display
        if (thisJobs != null) // check if list is empty
        {   // if not empty, calculate estimate and display on screeen
            priceEstimate.Text = AppointmentViewService.CalculateEstimate(thisJobs.ToList()).ToString("C");
        }
        RODetailsChanged(); // allert details changed
    }
    private void RODetailsChanged()
    {
        updateRO.IsEnabled = true;// enable the "Save Changes" button
        if (repairOrder != null)
        {  // Enable checkout button if all info is present
            if (repairOrder.EmployeeId > 0 && thisJobs.Count >= 1)
            {
                closeRO.IsEnabled = true;
                return;
            }
        }
        closeRO.IsEnabled = false;
    }
    private void removeTech_Clicked(object sender, EventArgs e)
    {
        removeTech.IsEnabled = false;
        if (repairOrder != null)
        {
            repairOrder.EmployeeId = 0;
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
            selectedTech.Text = ":::Not Assigned:::";
            RefreshTechList();
            RODetailsChanged();
        }
    }
    //========================================================================================================
    //-----ORDER LOGS
    //========================================================================================================
    private void ROs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        serviceJobMenu.IsEnabled = true; // enable job menu
        orderSlip.IsVisible = true; // switch to order slip view
        roLogs.IsVisible = false;
        repairOrder = e.SelectedItem as RepairOrder; // get RepairOrder OBJ
        RefreshTechList(); // refresh tech list to get list of available techs
        if (repairOrder != null) // ensure we have an RO OBJ
        {
            // set local variables
            c = MauiProgram.ShopDB.GetACustomerByName(repairOrder.CustomerName);
            v = MauiProgram.ShopDB.GetVehicleByVIN(repairOrder.VIN);
            // bind info to appointmenmt slip
            cInformation.BindingContext = c;
            vInformation.BindingContext = v;
            problemDescriptionEntry.Text = repairOrder.RepairOrderDescription;            
            //populate list of jobs assigned to this appointment
            thisJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.
                GetServiceJobListByRepairOrderId(repairOrder.RepairOrderId));
            RefreshROJobs();
            updateRO.IsEnabled = false; // disable update button until changes are made
            if (repairOrder.EmployeeId > 0) // check if RO has a tech assigned
            { // if so, get that tech's information
                Technician? tech = MauiProgram.ShopDB.GetTechnician(repairOrder.EmployeeId);
                if (tech != null) // check that we have a tech OBJ
                {
                    selectedTech.Text = tech.Name; // bind Tech name to the repair order
                    t = tech; // set local t variable to the tech OBJ
                    removeTech.IsEnabled = true;
                }
            }
        }
    }
    //========================================================================================================
    //-----TECHNICIAN LIST
    //========================================================================================================
    private void techList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Technician? techToAssign = e.SelectedItem as Technician; // get a tech OBJ
        if (techToAssign != null && repairOrder != null) // ensure all values are present
        {
            assignTech.IsEnabled = true; // enable 'assign tech' button
            assignTech.Text = $"Assign {techToAssign.Name} to This RO?"; // Change text for UX
            t = techToAssign; // Set the page local tech variable to the selected tech OBJ
        }
    }
    private void assignTech_Clicked(object sender, EventArgs e)
    {
        assignTech.IsEnabled = false; // disable button
        if (t != null && repairOrder != null) // ensure both values are present
        {
            repairOrder.EmployeeId = t.EmployeeId; // attach the tech to the repair order
            selectedTech.Text = t.Name; // display the techs name on the Repair order
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder); // update the order in the DB
            removeTech.IsEnabled = true;
            RODetailsChanged();
        }
    }
    //========================================================================================================
    //-----SERVICE JOBS MENU
    //========================================================================================================
    private void addThisJob_Clicked(object sender, EventArgs e)
    {
        ServiceJob? sj = thisJob.BindingContext as ServiceJob; //get a job OBJ
        if (sj != null)
        {
            thisJobs.Add(sj); // add to job list
            RefreshROJobs(); // refresh display
        }
    }
    private void serviceJobs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        ServiceJob? sj = e.SelectedItem as ServiceJob; // get selected job
        if (sj != null) // ensure a job is chosen
        {
            thisJob.BindingContext = sj; // bind to display
            addThisJob.IsEnabled = true; // enable add job button
        }
    }
    //==================================================================================
    // INPUT VALIDATION
    //==================================================================================
    // For more info on how these work, see the Validation class in 'Services' folder
    private void problemDescriptionEntry_TextChanged(object sender, TextChangedEventArgs e)
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
        RODetailsChanged();
    }
    private void roPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        clearSearchForm.IsEnabled = true;
        roPhone.Text = Validate.Phone(e.NewTextValue);
        if (roPhone.Text != null)
        {
            if (roPhone.Text.Length == 12)
            {
                roPhone.TextColor = Color.FromArgb("080D10");
                SearchPhone.IsEnabled = true;
                return;
            }
        }
        roPhone.TextColor = Color.FromArgb("FF0000");
        SearchPhone.IsEnabled = false;
    }
    private void roName_TextChanged(object sender, TextChangedEventArgs e)
    {
        clearSearchForm.IsEnabled = true;
        roName.Text = Validate.Name(e.NewTextValue);
        searchName.IsEnabled = true;
    }
    //==================================================================================
    // Search Widget
    //==================================================================================
    private void RefreshSearch()
    {
        // sets all aspects of the search widget to default view
        roPhone.Text = null;
        roName.Text = null;
        searchName.IsEnabled = false;
        SearchPhone.IsEnabled = false;
        clearSearchForm.IsEnabled = false;
    }
    private void clearSearchForm_Clicked(object sender, EventArgs e)
    {
        RefreshSearch(); // reset search form
        RefreshROs(); // refresh appointments without search filters
    }
    private async void SearchPhone_Clicked(object sender, EventArgs e)
    {
        // Search appointments by customer phone number
        string phone = roPhone.Text; // get search value (inputted phone number)
        if (phone.Length == 12) // only accept a full phone number
        {
            // Call filter method to sort new lists based on phone
            AppointmentViewService.RObyPhone(phone);
            unasignedROs.ItemsSource = AppointmentViewService.unassigned; // bind lists to GUI
            inProgressROs.ItemsSource = AppointmentViewService.inProgress;
            // Check if the returned lists have any values in them
            int? unassign = AppointmentViewService.unassigned?.Count;
            int? progress = AppointmentViewService.inProgress?.Count;
            if (unassign == 0 && progress == 0)
            { // if lists have no values, Display a message that no results were found, and clear the search
                await DisplayAlert("No Results", "No Repair Orders match this phone number", "Ok");
                RefreshROs();
            }
        }
    } 
    private async void searchName_Clicked(object sender, EventArgs e)
    {
        // Search appointments by customer name
        string name = roName.Text; // get search value (name or partial name inputted)
        if (name != null && name != "") // ensure there is a value to compare
        {
            // Call filter method to compare search value to existing appointments.
            AppointmentViewService.RObyName(name);
            unasignedROs.ItemsSource = AppointmentViewService.unassigned; // bind lists to GUI
            inProgressROs.ItemsSource = AppointmentViewService.inProgress;
            // Check if the returned lists have any values in them
            int? unassign = AppointmentViewService.unassigned?.Count;
            int? progress = AppointmentViewService.inProgress?.Count;
            if (unassign == 0 && progress == 0)
            { // if lists have no values, Display a message that no results were found, and clear the search
                await DisplayAlert("No Results", "No appointments match this Name", "Ok");
                RefreshROs();
            }
        }
    }
    
}