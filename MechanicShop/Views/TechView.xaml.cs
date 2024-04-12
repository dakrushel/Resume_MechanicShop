using System.Collections.ObjectModel;
using MechanicShop.Models;
using MechanicShop.Services;
/* 
 * Shop Management View page code behind (Written by Chloe) 
    This page handles operations for Creating, reading,m updating and deleting Technicians
    As well as setting shop currency rates for hourly rate and supplies

    The code handles input validation and passing changes to and from the back end
 */
namespace MechanicShop.Views;
public partial class TechView : ContentPage
{
    public static bool formValid = false;
    public static Technician? t = null;
    public static ShopSettings? shopSettings = null;
	public TechView()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        // reset local variables
        formValid = false;
        t = null;
        // reset tech form buttons and fields
        techName.Text = null;
        techPhone.Text = null;
        techSpecial.Text = null;
        techRate.Text = null;
        techID.Text = null;
        techHireDate.Text = null;
        techInfo.BindingContext = null;
        updateTech.IsEnabled = false;
        techForm.IsEnabled = false;
        techFormTitle.Text = "Technician Info:";
        newTech.IsEnabled = true;
        techOptions.IsVisible = true;
        addTech.IsVisible = false;
        updateShopSettings.IsEnabled = false;
        //Refresh Widgets
        RefreshTechList();       
        RefreshShopSettings();
    }
    private void RefreshTechList()
    {
        techList.IsEnabled = true;
        var techs = new ObservableCollection<Technician>(MauiProgram.ShopDB.GetAllTechnicians());
        techListView.ItemsSource = techs;
    }
    private void RefreshShopSettings()
    {
        shopSettings = MauiProgram.ShopDB.GetShopSettings();
        shopSupplies.Text = shopSettings.ShopSupplyCost.ToString("C");
        shopRate.Text = shopSettings.ShopHourlyRate.ToString("C");
        updateShopSettings.IsEnabled = false;
    }
    //==================================================================================
    // Technician DISPLAY LIST
    //==================================================================================
    private void techListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {   //setup the technician information form view
        techFormTitle.Text = "Technician Info:";
        newTech.IsEnabled = true;
        techForm.IsEnabled = true;
        t = e.SelectedItem as Technician;
        if (t != null)  // ensure a tech is selected
        {
            techInfo.BindingContext = t;  // display their inromation
            techRate.Text = t.HourlyRate.ToString("C");
        }
        updateTech.IsEnabled = false;
    }
    private void newTech_Clicked(object sender, EventArgs e)
    {   // Set up the new technician form
        newTech.IsEnabled = false;
        t = null;
        formValid = false;
        techInfo.BindingContext = null;
        techForm.IsEnabled = true;
        techOptions.IsVisible = false;
        addTech.IsVisible = true;
        techFormTitle.Text = "New Technician";
        techList.IsEnabled = false;
    }
    //==================================================================================
    // Technician info form
    //==================================================================================
    private async void deleteTech_Clicked(object sender, EventArgs e)
    {
        if (t != null) // ensure a tech is selected
        {   // confirmation popup that the user really wants to delete
            bool delete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this Technician?", "Delete Technician", "Cancel");
            if (!delete)
            {   // if they dont want to delete, cancel the operation
                return;
            }
            MauiProgram.ShopDB.RemoveTechnician(t.EmployeeId); // delete the technician
            OnAppearing(); // refresh page
        }
    }
    private void updateTech_Clicked(object sender, EventArgs e)
    {
        UpdateTech(); // one of two ways to update a tech
    }
    private void UpdateTech()
    {
        updateTech.IsEnabled = false; // diasable button
        ValidateFormInfo(); // validate all form informarion
        if (formValid && t != null) // double check all info is present
        {   //update the technician OBJ
            t.Name = techName.Text;
            t.EmployeePhone = techPhone.Text;
            t.Specialization = techSpecial.Text;
            t.HourlyRate = double.Parse(techRate.Text);
            MauiProgram.ShopDB.UpdateTechnician(t); // persist to DB
            RefreshTechList(); // refresh tech list display widget
        }
    }
    private async void clearTechForm_Clicked(object sender, EventArgs e)
    {
        if (techOptions.IsVisible == true && updateTech.IsEnabled == true)
        {   // if a technician was being updated but the process wasnt complete, ask to save
            bool saveChanges = await DisplayAlert("Update Technician?","Changes have been made to this technicians info. do you want to save changes?","Save Changes","Don't Save");
            if (saveChanges)
            {
                UpdateTech();
            }
        }
        if (addTech.IsVisible == true && addTech.IsEnabled == true)
        {  // same for if a tech was being added
            bool saveNew = await DisplayAlert("Save New Tech?", "You've entered new technician information, do you want to add this Technician?", "Add New Technician", "Discard Information");
            if (saveNew)
            {
                AddTech();
            }
        }
        OnAppearing(); // refresh the page
    }
    private void addTech_Clicked(object sender, EventArgs e)
    {
        AddTech(); // one of two ways to add a tech
    }
    private void AddTech()
    {
        ValidateFormInfo(); // Validate all form inforamtion
        if (formValid) // Continue only if valid (Error messages will display from called method otherwise)
        {
            string name = techName.Text; // gather all needed info
            string phone = techPhone.Text;
            string special = techSpecial.Text;
            double rate = double.Parse(techRate.Text);
            string date = AppointmentView.TodayDate;
            if (name != null && phone != null && special != null && date != null) // ensure all info valid once again
            {  // make the tech OBJ, add to Database, refresh page
                Technician newTech = new Technician(name, phone, date, special, rate);
                MauiProgram.ShopDB.AddTechnician(newTech);
                OnAppearing();
            }
        }
    }
    private void FormChanged()
    {
        if (techName.Text != null && techPhone.Text != null && techSpecial.Text != null && techRate.Text != null)
        {
            if (techName.Text.Length >= 3 && techPhone.Text.Length == 12 && techSpecial.Text.Length >= 3 && techRate.Text.Length >= 2)
            {  //enabled buttons if all information is present
                updateTech.IsEnabled = true;
                addTech.IsEnabled = true;
                return;
            }
        }
        updateTech.IsEnabled = false;
        addTech.IsEnabled = false;
    } 
    private async void ValidateFormInfo()
    {  //various error messages if a user manages to submit an incomplete form.
        if (techName.Text == null || techPhone.Text == null || techSpecial.Text == null || techRate.Text == null)
        {
            await DisplayAlert("Form Incomplete", "Please fill out all fields", "Ok");
            return;
        }
        if (techName.Text.Length < 3)
        {
            await DisplayAlert("Invalid Name", "Must be more than 2 characters", "Ok");
            return;
        }
        if (techPhone.Text.Length < 12)
        {
            await DisplayAlert("Invalid Phone Number", "Please enter a full phone number", "Ok");
            return;
        }
        if (techSpecial.Text.Length < 3)
        {
            await DisplayAlert("Invalid Specialization", "Must be more than 2 characters", "Ok");
            return;
        }
        formValid = true;
    }
    //==================================================================================
    // SHOP SETTINGS MENU
    //==================================================================================
    private void updateShopSettings_Clicked(object sender, EventArgs e)
    {
        updateShopSettings.IsEnabled = false; //disable button
        if (shopRate.Text != null && shopSupplies.Text != null && shopSettings != null)
        {  // ensure all values are not null AND are valid
            if (shopRate.Text != "" && shopSupplies.Text != "")
            { // update values to DB
                shopSettings.ShopHourlyRate = double.Parse(shopRate.Text);
                shopSettings.ShopSupplyCost = double.Parse(shopSupplies.Text);
                MauiProgram.ShopDB.UpdateShopSettings(shopSettings);
            }         
        }
        RefreshShopSettings(); // refresh widget
    }
    private void shopSettingsChanged()
    { // ensure all needed values are present and valid
        if (shopRate.Text != null && shopSupplies.Text != null && shopSettings != null)
        {  // ensure there are actual values and not jjust white space
            if (shopRate.Text != "" && shopSupplies.Text != "")
            {   // enable button
                updateShopSettings.IsEnabled = true;
                return;
            } 
        }
        updateShopSettings.IsEnabled = false;
    }
    //==================================================================================
    // INPUT VALIDATION
    //==================================================================================
    // For more info on how these work, see the Validation class in 'Services' folder
    private void techName_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
        techName.Text = Validate.Name(e.NewTextValue);
    }
    private void techPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
        techPhone.Text = Validate.Phone(e.NewTextValue);
    }
    private void techSpecial_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
        techSpecial.Text = Validate.Name(e.NewTextValue);
    }
    private void techRate_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
        techRate.Text = Validate.Currency(e.NewTextValue);
    }       
    private void shopRate_TextChanged(object sender, TextChangedEventArgs e)
    {
        shopSettingsChanged();
        shopRate.Text = Validate.Currency(e.NewTextValue);
    } 
    private void shopSupplies_TextChanged(object sender, TextChangedEventArgs e)
    {
        shopSettingsChanged();
        shopSupplies.Text = Validate.Currency(e.NewTextValue);
    }

   
}