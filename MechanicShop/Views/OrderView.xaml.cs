using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MechanicShop.Models;
using MechanicShop.Services;

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
        updateRO.IsEnabled = false;
        selectedTech.Text = ":::Not Assigned:::";
        // If receiving a customer from PASS
        if (Pass.CustomerPass != null && Pass.VehiclePass != null)
        {
            // page temp objects
            c = Pass.RetreiveCustomer();
            v = Pass.RetrieveVehicle();
            repairOrder = Pass.RetrieveRO();


            //Binding Contexts
            cInformation.BindingContext = c;
            vInformation.BindingContext = v;
            thisJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.
                GetServiceJobListByRepairOrderId(repairOrder.RepairOrderId));
            repairOrderJobs.ItemsSource = thisJobs;
            problemDescriptionEntry.Text = repairOrder.RepairOrderDescription;

            //Widget Visibility
            orderSlip.IsVisible = true;
            roLogs.IsVisible = false;
            serviceJobMenu.IsEnabled = true;
        }
        else
        {
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
        AppointmentViewService.refreshROs();
        unasignedROs.ItemsSource = AppointmentViewService.unassigned;
        inProgressROs.ItemsSource = AppointmentViewService.inProgress;
        
    }



    //========================================================================================================
    //-----ORDER SLIP
    //========================================================================================================
    private async void DeleteRO_Clicked(object sender, EventArgs e)
    {
        if (repairOrder != null)
        {
            // confirmation message to prevent accidental deletion
            bool delete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this Repair Order?", "Delete RO", "Cancel");
            if (delete)
            {
                MauiProgram.ShopDB.RemoveRepairOrder(repairOrder.RepairOrderId);
                OnAppearing();
            }
        }
    }

    private void updateRO_Clicked(object sender, EventArgs e)
    {
        updateRO.IsEnabled = false;
        if (repairOrder != null)
        {
            //update problem description
            repairOrder.RepairOrderDescription = problemDescriptionEntry.Text;

            MauiProgram.ShopDB.RemoveRepairOrderServiceJobBridgeAttachedToRepairOrder(repairOrder.RepairOrderId);
            foreach (ServiceJob job in thisJobs)
            {
                repairOrder.AssignServiceJob(job.ServiceJobId);
            }

            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
            
        }

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
    private void removeJobBtn_Clicked(object sender, EventArgs e)
    {
        removeJobBtn.Text = "Remove Job";
        updateRO.IsEnabled = true;
        ServiceJob? toRemove = repairOrderJobs.SelectedItem as ServiceJob;
        if (toRemove != null)
        {
            thisJobs.Remove(toRemove);
            RefreshROJobs();
        }
        removeJobBtn.IsEnabled = false;
    }

    private async void closeRO_Clicked(object sender, EventArgs e)
    {
        // TODO this will delete the OBj and generate invoice. Need to make sure all info there (tech, jobs, etc)
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
            repairOrder.RepairOrderHours = AppointmentViewService.GetHours(thisJobs.ToList());
            
            repairOrder.DateClose = AppointmentView.TodayDate;
    
            // update the RO object
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);

            //invoice the RO
            GenerateInvoice.SaveInvoiceDelRO(repairOrder);
            //TODO: Delete the RO
            
        }
    }

    private void Clear_Clicked(object sender, EventArgs e)
    {

        //TODO do you want to save changes??? (ifchanged)
        OnAppearing();
    }
    private void RefreshROJobs()
    {
        repairOrderJobs.ItemsSource = thisJobs;
        //TODO
        if (thisJobs != null)
        {
            priceEstimate.Text = AppointmentViewService.CalculateEstimate(thisJobs.ToList()).ToString("C");
        }
        RODetailsChanged();
    }
    private void RODetailsChanged()
    {
        // enable the "Save CHanges" button
        updateRO.IsEnabled = true;

        // Enable checkout button is all info is present
        if (repairOrder != null)
        {
            if (repairOrder.EmployeeId > 0 && thisJobs != null)
            {
                closeRO.IsEnabled = true;
                return;
            }
        }
        closeRO.IsEnabled = false;
    }



    //========================================================================================================
    //-----ORDER LOGS
    //========================================================================================================
    private void ROs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        serviceJobMenu.IsEnabled = true;
        orderSlip.IsVisible = true;
        roLogs.IsVisible = false;
        repairOrder = e.SelectedItem as RepairOrder;
        RefreshTechList();
        if (repairOrder != null)
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

            if (repairOrder.EmployeeId != null)
            {
                Technician? tech = AppointmentViewService.GetTechByID(repairOrder.EmployeeId);
                if (tech != null)
                {
                    selectedTech.Text = tech.Name;
                }
            }

            //TODO get tech name in selectedTech
            if (repairOrder.EmployeeId != null)
            {
                t = MauiProgram.ShopDB.GetTechnician(repairOrder.EmployeeId);
            }
        }
    }

    //========================================================================================================
    //-----TECHNICIAN LIST
    //========================================================================================================
    private void techList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Technician? techToAssign = e.SelectedItem as Technician;
        if (techToAssign != null && repairOrder != null)
        {
            assignTech.IsEnabled = true;
            assignTech.Text = $"Assign {techToAssign.Name} to This RO?";
            t = techToAssign;
        }
    }
    private void assignTech_Clicked(object sender, EventArgs e)
    {
        assignTech.IsEnabled = false;
        if (t != null && repairOrder != null)
        {
            repairOrder.EmployeeId = t.EmployeeId;
            selectedTech.Text = t.Name;
            MauiProgram.ShopDB.UpdateRepairOrder(repairOrder);
        }
    }


    //========================================================================================================
    //-----SERVICE JOBS MENU
    //========================================================================================================
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
        }
    }
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

    //========================================================================================================
    //-----INPUT VALIDATION
    //========================================================================================================
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

    
}