using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MechanicShop.Models;

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

        shopSettings = MauiProgram.ShopDB.GetShopSettings();
        settingsMenu.BindingContext = shopSettings;

    }
    private void RefreshTechList()
    {
        var techList = new ObservableCollection<Technician>(MauiProgram.ShopDB.GetAllTechnicians());
        techListView.ItemsSource = techList;
    }


    //==================================================================================
    // Technician info form
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
        }
        updateTech.IsEnabled = false;
    }
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

    private void clearTechForm_Clicked(object sender, EventArgs e)
    {
        OnAppearing();
    }

    private void addTech_Clicked(object sender, EventArgs e)
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

            //TODO make technician (need ID)

        }
    }
    private void FormChanged()
    {
        updateTech.IsEnabled = true;
        AddTechEnable();
    }
    private void AddTechEnable()
    {
        if (techName.Text != null && techPhone.Text != null && techSpecial.Text != null && techRate.Text != null)
        {
            addTech.IsEnabled = true;
            return;
        }
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
    // INPUT VALIDATION
    //==================================================================================
    private void techName_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
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
    private void techPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
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
    private void techSpecial_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
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
    private void techRate_TextChanged(object sender, TextChangedEventArgs e)
    {
        FormChanged();
        var entry = (Entry)sender;
        if (e.NewTextValue == null) { return; }
        // Remove non-digit characters
        var newText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        // Limit maximum length to 5 digits
        if (newText.Length > 4)
        {
            newText = newText.Substring(0, 4);
        }
        // Automatically insert decimal
        if (newText.Length > 2)
        {
            newText = newText.Insert(2, ".");           
        }
        entry.Text = newText;
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


    }

    private void updateShopSettings_Clicked(object sender, EventArgs e)
    {
        updateShopSettings.IsEnabled = false;
        if (shopRate.Text != null && shopSupplies.Text != null && shopSettings != null)
        {
            shopSettings.ShopHourlyRate = double.Parse(shopRate.Text);
            shopSettings.ShopSupplyCost = double.Parse(shopSupplies.Text);
            MauiProgram.ShopDB.UpdateShopSettings(shopSettings);
        }
        
    }

    private void shopRate_TextChanged(object sender, TextChangedEventArgs e)
    {
        shopSettingsChanged();
        var entry = (Entry)sender;
        if (e.NewTextValue == null) { return; }
        // Remove non-digit characters
        var newText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        // Limit maximum length to 5 digits
        if (newText.Length > 5)
        {
            newText = newText.Substring(0, 5);
        }
        // Automatically insert decimal
        if (newText.Length >= 3 && newText.Length <= 5 && newText.IndexOf('.') == -1)
        {
            newText = newText.Insert(newText.Length - 2, ".");
        }
        entry.Text = newText;
    }
    private void shopSettingsChanged()
    {
        if (shopRate.Text != null && shopSupplies.Text != null)
        {
            updateShopSettings.IsEnabled = true;
            return;
        }
        updateShopSettings.IsEnabled = false;
            
    }

    private void shopSupplies_TextChanged(object sender, TextChangedEventArgs e)
    {
        shopSettingsChanged();
        var entry = (Entry)sender;
        if (e.NewTextValue == null) { return; }
        // Remove non-digit characters
        var newText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        // Limit maximum length to 5 digits
        if (newText.Length > 5)
        {
            newText = newText.Substring(0, 5);
        }
        // Automatically insert decimal
        if (newText.Length >= 3 && newText.Length <= 5 && newText.IndexOf('.') == -1)
        {
            newText = newText.Insert(newText.Length - 2, ".");
        }
        entry.Text = newText;
    }

   
}