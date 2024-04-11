using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using MechanicShop.Models;
using MechanicShop.Services;

namespace MechanicShop.Views;

public partial class TechView : ContentPage
{
    public static bool formValid = false;
    public static Technician? t = null;
    public static ShopSettings? shopSettings = null;
	public TechView()
	{
		InitializeComponent();
        updateShopSettings.IsEnabled = false;
	}
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        t = null;
        techName.Text = null;
        techPhone.Text = null;
        techSpecial.Text = null;
        techRate.Text = null;
        techID.Text = null;
        techHireDate.Text = null;

        RefreshTechList();
        techInfo.BindingContext = null;
        formValid = false;
        updateTech.IsEnabled = false;
        techForm.IsEnabled = false;
        techFormTitle.Text = "Technician Info:";
        newTech.IsEnabled = true;

        techOptions.IsVisible = true;
        addTech.IsVisible = false;
        updateShopSettings.IsEnabled = false;
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
    {
        techFormTitle.Text = "Technician Info:";
        newTech.IsEnabled = true;
        techForm.IsEnabled = true;
        t = e.SelectedItem as Technician;
        if (t != null)
        {
            techInfo.BindingContext = t;
            techRate.Text = t.HourlyRate.ToString("C");
        }
        updateTech.IsEnabled = false;
    }
    private void newTech_Clicked(object sender, EventArgs e)
    {
        //setup new tech form
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
        if (t != null)
        {
            bool delete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this Technician?", "Delete Technician", "Cancel");
            if (!delete)
            {
                return;
            }
            MauiProgram.ShopDB.RemoveTechnician(t.EmployeeId);
            OnAppearing();
        }
    }
    private void updateTech_Clicked(object sender, EventArgs e)
    {
        UpdateTech();
    }
    private void UpdateTech()
    {
        updateTech.IsEnabled = false;
        ValidateFormInfo();
        if (formValid && t != null)
        {
            t.Name = techName.Text;
            t.EmployeePhone = techPhone.Text;
            t.Specialization = techSpecial.Text;
            t.HourlyRate = double.Parse(techRate.Text);
            MauiProgram.ShopDB.UpdateTechnician(t);
            RefreshTechList();
        }
    }
    private async void clearTechForm_Clicked(object sender, EventArgs e)
    {
        if (techOptions.IsVisible == true && updateTech.IsEnabled == true)
        {
            bool saveChanges = await DisplayAlert("Update Technician?","Changes have been made to this technicians info. do you want to save changes?","Save Changes","Don't Save");
            if (saveChanges)
            {
                UpdateTech();
            }
        }
        if (addTech.IsVisible == true && addTech.IsEnabled == true)
        {
            bool saveNew = await DisplayAlert("Save New Tech?", "You've entered new technician information, do you want to add this Technician?", "Add New Technician", "Discard Information");
            if (saveNew)
            {
                AddTech();
            }
        }
        OnAppearing();
    }
    private void addTech_Clicked(object sender, EventArgs e)
    {
        AddTech();
    }
    private void AddTech()
    {
        ValidateFormInfo();
        if (formValid)
        {
            string name = techName.Text;
            string phone = techPhone.Text;
            string special = techSpecial.Text;
            double rate = double.Parse(techRate.Text);
            // need EmployeeId
            string date = AppointmentView.TodayDate;

            if (name != null && phone != null && special != null && date != null)
            {
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
            {
                updateTech.IsEnabled = true;
                addTech.IsEnabled = true;
                return;
            }

        }
        updateTech.IsEnabled = false;
        addTech.IsEnabled = false;
    } 
    private async void ValidateFormInfo()
    {
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
        updateShopSettings.IsEnabled = false;
        if (shopRate.Text != null && shopSupplies.Text != null && shopSettings != null)
        {
            shopSettings.ShopHourlyRate = double.Parse(shopRate.Text);
            shopSettings.ShopSupplyCost = double.Parse(shopSupplies.Text);
            MauiProgram.ShopDB.UpdateShopSettings(shopSettings);
        }
        RefreshShopSettings();
    }
    private void shopSettingsChanged()
    {
        if (shopRate.Text != null && shopSupplies.Text != null && shopSettings != null)
        {
            updateShopSettings.IsEnabled = true;
            return;
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