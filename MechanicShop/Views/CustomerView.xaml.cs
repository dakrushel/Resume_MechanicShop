using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using MechanicShop.Models;
using MechanicShop.Resources;
using Microsoft.Maui.Controls;
using MechanicShop.Services;



namespace MechanicShop.Views;

public partial class CustomerView : ContentPage
{
    
    public CustomerView()
	{
		InitializeComponent();



        var customers = new ObservableCollection<Customer>(MauiProgram.ShopDB.GetAllCustomers());
        customersCollectionView.ItemsSource = customers;

        //var makeList = new ObservableCollection<string>(MauiProgram.ShopDB.);
        
        // Get years for the yearlist picker (in the add vehicle widget)
        for (int year = 1995; year <= 2025; year++)
        {
            yearPicker.Items.Add(year.ToString());
        }
        colourPicker.ItemsSource = MauiProgram.vehicleColors;

        // get makes for the make picker in the add vehicle widget
        List<string> makes = MauiProgram.ShopDB.GetListOfMakes();
        makePicker.ItemsSource = makes;

        
    }

    //====================================================================================================
    // Page formatting upon refreshing page (or clearing a certain form)-------------------------------
    //====================================================================================================
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        ResetSearchWidget();
        ResetAddVehicleForm();
        ResetEditCustomerForm();
        ResetCustomerDisplay();
        ResetAddCustomerForm();
        
    }
    private void ResetSearchWidget()
    {
        nameEntry.Text = null;
        phoneEntry.Text = null;
        clearCustomerSearch.IsEnabled = false;
        searchCustomersBtn.IsEnabled = false;

        var customers = new ObservableCollection<Customer>(MauiProgram.ShopDB.GetAllCustomers());
        customersCollectionView.ItemsSource = customers;
    }
    private void ResetCustomerDisplay()
    {
        cNameBox.Text = null;
        cPhoneBox.Text = null;
        cEmailBox.Text = null;
        customerDisplay.IsEnabled = false;
        vehicleInformation.BindingContext = null;
        cVehicleList.ItemsSource = null;
        deleteVehicle.IsEnabled = false;
        newAppointmentBtn.IsEnabled = false;
    }
    private void ResetAddCustomerForm()
    {
        newNameBox.Text = null;
        newPhoneBox.Text = null;
        newEmailBox.Text = null;
        addCustomerForm.IsVisible = false;
        searchCustomers.IsEnabled = true;
    }
    public void ResetEditCustomerForm()
    {
        editNameBox.Text = null;
        editPhoneBox.Text = null;
        editEmailBox.Text = null;
        EditCustomerForm.IsVisible = false;
        searchCustomers.IsEnabled = true;
        customerDisplay.IsEnabled = true;
    }
    //====================================================================================================
    // Customer Search Box---------------------------------------------------------------------
    //====================================================================================================
    private void clearCustomerSearch_Clicked(object sender, EventArgs e)
    {
        // inactive unless search fields filled in
        ResetSearchWidget();
        
    }
    private void searchCustoemrsBtn_Clicked(object sender, EventArgs e)
    {
        // TODO: this method!
        // inactive unless search fields are filled in
        // Pairs down collectionview to only items matching search terms
    }
    private void addCustomer_Clicked(object sender, EventArgs e)
    {
        // Display the 'Add new customer form
        addCustomerForm.IsVisible = true;
        // pass the name and or phone number that was being searched
        newNameBox.Text = nameEntry.Text;
        newPhoneBox.Text = phoneEntry.Text;
        searchCustomers.IsEnabled = false;
    }
    private void customers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        //TODO: hightlight???
        Customer? selectedCustomer = e.SelectedItem as Customer;
        customerDisplay.IsEnabled = true;
        if (selectedCustomer != null)
        {
            cNameBox.Text = selectedCustomer.Name;
            cPhoneBox.Text = selectedCustomer.CustomerPhone;
            cEmailBox.Text = selectedCustomer.Address;
            // TODO get cars
            var vehicles = new ObservableCollection<Vehicle>(MauiProgram.ShopDB.GetCustomerVehicles(selectedCustomer.CustomerPhone));
            cVehicleList.ItemsSource = vehicles;
            vehicleInformation.BindingContext = null;
            deleteVehicle.IsEnabled = false;
            newAppointmentBtn.IsEnabled = false;
        }
        

    }
    //====================================================================================================
    // Add new customer form------------------------------------------------------------------
    //====================================================================================================
    private async void AddThisCustomerBtn_Clicked(object sender, EventArgs e)
    {
        string newCustomerName = newNameBox.Text;
        string newPhoneNumber = newPhoneBox.Text;
        string newEmail = newEmailBox.Text;
        

        // Regular expression pattern for validating email addresses
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Check if the entered text matches the pattern
        if (!Regex.IsMatch(newEmail, pattern))
        {
            // If the entered text is not a valid email address, clear the entry field
            newEmailBox.Text = "";
            await DisplayAlert("Invalid Email Address", "Please Enter a valid Email address", "Ok");
            return;
        }

        var customers = new ObservableCollection<Customer>(MauiProgram.ShopDB.GetAllCustomers());
        //customersCollectionView.ItemsSource = customers;
        foreach (Customer customer in customers)
        {
            if (customer.CustomerPhone == newPhoneNumber)
            {
                await DisplayAlert("Cannot Add Customer", "This phone number already belongs to another customers account", "Ok");
                newPhoneBox.Text = null;
                return;
            }
        }

        if (newCustomerName != null && newPhoneNumber != null && newEmail != null)
        {
            Customer newCustomer = new Customer(newCustomerName, newEmail, newPhoneNumber);
            //MauiProgram.ShopDB.AddCustomer(newCustomer);
            ResetAddCustomerForm();
            ResetSearchWidget();
        }     
    }

    private void cancelAddCustomer_Clicked(object sender, EventArgs e)
    {
        addCustomerForm.IsVisible = false;
        searchCustomers.IsEnabled = true;
    }
    //====================================================================================================
    // EDIT CUSTOMER INFORMATION FORM----------------------------------------------------------
    //====================================================================================================
    private void updateCustomerInfo_Clicked(object sender, EventArgs e)
    {
        EditCustomerForm.IsVisible = true;
        searchCustomers.IsEnabled = false;
        customerDisplay.IsEnabled = false;
        editNameBox.Text = cNameBox.Text;
        editPhoneBox.Text = cPhoneBox.Text;
        editEmailBox.Text = cEmailBox.Text;

    }
    private void cancelEditCustomer_Clicked(object sender, EventArgs e)
    {
        ResetEditCustomerForm();
    }

    private async void EditThisCustomerBtn_Clicked(object sender, EventArgs e)
    {
        var customers = new ObservableCollection<Customer>(MauiProgram.ShopDB.GetAllCustomers());
        Customer? toEdit = null;
        foreach (Customer c in customers)
        {
            if (c.CustomerPhone == editPhoneBox.Text)
            {
                toEdit = c;
                break;
            }
        }
        if (toEdit == null)
        {
            await DisplayAlert("Unable to update Customer", "Could not locate account", "Ok");
            return;
        }
        string newEmail = editEmailBox.Text;
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(newEmail, pattern))
        {
            // If the entered text is not a valid email address, clear the entry field
            editEmailBox.Text = cEmailBox.Text;
            await DisplayAlert("Invalid Email Address", "Please Enter a valid Email address", "Ok");
            return;
        }
        toEdit.Name = editNameBox.Text;
        toEdit.Address = newEmail;
        MauiProgram.ShopDB.UpdateCustomer(toEdit);
        cNameBox.Text = editNameBox.Text;
        cEmailBox.Text = editEmailBox.Text;
        ResetEditCustomerForm();
        ResetSearchWidget();
    }

    //====================================================================================================
    // ADD CUSTOMER VEHICLE FORM---------------------------------------------------------------
    //====================================================================================================
    private void addVehicle_Clicked(object sender, EventArgs e)
    {
        AddVehicleForm.IsVisible = true;
        searchCustomers.IsEnabled = false;
        customerDisplay.IsEnabled = false;

    }
    private void cancelAddVehicle_Clicked(object sender, EventArgs e)
    {
        ResetAddVehicleForm();
    }
    private void makePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddVehicleForm_Updated();
        var picker = (Picker)sender;
        if (picker.SelectedItem == null)
        {
            modelPicker.ItemsSource = null;
            modelPicker.IsEnabled = false;
            return;
        }
        string? make = picker.SelectedItem.ToString();
        if (make != null)
        {
            List<string> models = new List<string>();
            List<VehicleDatabase> vehicles = MauiProgram.ShopDB.GetAllVehicleByMake(make);
            foreach (var vehicle in vehicles)
            {
                models.Add(vehicle.VehicleModel);
            }
            modelPicker.ItemsSource = models;
            modelPicker.IsEnabled = true;
        }
    }
    public void ResetAddVehicleForm()
    {
        AddVehicleForm.IsVisible = false;
        searchCustomers.IsEnabled = true;
        customerDisplay.IsEnabled = true;
        vinEntry.Text = null;
        yearPicker.SelectedItem = null;
        colourPicker.SelectedItem = null;
        makePicker.SelectedItem = null;
        modelPicker.SelectedItem = null;
        //TODO: Reset picker selections
    }
    private async void AddThisVehicleBtn_Clicked(object sender, EventArgs e)
    {
        // gather all REQ infor for a new vehicle
        string? customerPhone = cPhoneBox.Text;
        string? newVin = vinEntry.Text;
        string? newYearSt = yearPicker.SelectedItem.ToString();
        string? newMake = makePicker.SelectedItem.ToString();
        string? newModel = modelPicker.SelectedItem.ToString();
        string? newColour = colourPicker.SelectedItem.ToString();
        // parse the year to an int
        int newYear = 0;
        if (int.TryParse(newYearSt, out int year))
        {
            newYear = year;
        }
        if (newYear == 0)
        {
            await DisplayAlert("Error", "Error adding Vehicle", "Ok");
            return;
        }
        //make new vehicle
        if (customerPhone != null && newVin != null && newYear != 0 && newMake != null && newModel != null && newColour != null)
        {
            Vehicle newVehicle = new Vehicle(newVin, newMake, newModel, newColour, newYear, customerPhone);
            MauiProgram.ShopDB.AddVehicle(newVehicle);
            var vehicles = new ObservableCollection<Vehicle>(MauiProgram.ShopDB.GetCustomerVehicles(customerPhone));
            cVehicleList.ItemsSource = vehicles;
            vehicleInformation.BindingContext = newVehicle;
            ResetAddVehicleForm();
        }
        
    }
    //====================================================================================================
    // CUSTOMER INFORMATION DISPLAY -----------------------------------------------------------
    //====================================================================================================
    private async void deleteCustomer_Clicked(object sender, EventArgs e)
    {
        bool delete = await DisplayAlert("Confirm Deletion", "Are you sure you want to delete this customer?", "Delete", "Cancel");
        if (delete)
        {
            string custPhone = cPhoneBox.Text;
            MauiProgram.ShopDB.RemoveCustomer(custPhone);
            ResetCustomerDisplay();
            var customers = new ObservableCollection<Customer>(MauiProgram.ShopDB.GetAllCustomers());
            customersCollectionView.ItemsSource = customers;
        }
    }
    private void ClearCustomerDisplay_Clicked(object sender, EventArgs e)
    {
        ResetCustomerDisplay();
        customersCollectionView.SelectedItem = null;
    }
    private void cVehicleList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Vehicle? v = e.SelectedItem as Vehicle;
        vehicleInformation.BindingContext = v;
        deleteVehicle.IsEnabled = true;
        newAppointmentBtn.IsEnabled = true;
    }
    private async void deleteVehicle_Clicked(object sender, EventArgs e)
    {
        bool delete = await DisplayAlert("Confirm Deletion", "Are you sure you want to perminantly delete this vehicle?", "Delete", "Cancel");
        if (delete)
        {
            Vehicle? toDelete = cVehicleList.SelectedItem as Vehicle;
            if (toDelete != null)
            {
                MauiProgram.ShopDB.RemoveVehicle(toDelete.VIN);
                var vehicles = new ObservableCollection<Vehicle>(MauiProgram.ShopDB.GetCustomerVehicles(cPhoneBox.Text));
                cVehicleList.ItemsSource = vehicles;
                vehicleInformation.BindingContext = null;
                deleteVehicle.IsEnabled = false;
                newAppointmentBtn.IsEnabled = false;
            }
            
        }

    }

    private async void newAppointmentBtn_Clicked(object sender, EventArgs e)
    {
        Customer? c = customersCollectionView.SelectedItem as Customer;
        Vehicle? v = cVehicleList?.SelectedItem as Vehicle;
        if(v != null && c!= null) 
        {
            Pass.PassCustomer(c);
            Pass.PassVehicle(v);
        }
        await Shell.Current.GoToAsync("//AppointmentView");

    }

    
    private void SearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if(nameEntry.Text == null && phoneEntry.Text == null)
        {
            clearCustomerSearch.IsEnabled = false;
            searchCustomersBtn.IsEnabled= false;
            return;
        }
        
        
        clearCustomerSearch.IsEnabled = true;
        searchCustomersBtn.IsEnabled = true;
    }


    



    


    //====================================================================================================
    // INPUT VALIDATIONS AND FORM BUTTON TRIGGERS
    //====================================================================================================
    private void AddCustomerForm_Updated()
    {
        if (newPhoneBox.Text == null)
        {
            AddThisCustomerBtn.IsEnabled = false;
            return;
        }
        if (newPhoneBox.Text.Length != 12 || newNameBox.Text == null || newEmailBox.Text == null)
        {
            AddThisCustomerBtn.IsEnabled = false;
            return;
        }
        if (newNameBox.Text == "" || !newEmailBox.Text.Contains('@'))
        {
            AddThisCustomerBtn.IsEnabled = false;
            return;
        }
        AddThisCustomerBtn.IsEnabled = true;
    }
    private void newEmailBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        
        AddCustomerForm_Updated();
        
    }
    private void newNameBox_TextChanged(object sender, TextChangedEventArgs e)
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
            AddCustomerForm_Updated();
        }
    }
    private void phoneEntry_TextChanged(object sender, TextChangedEventArgs e)
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
    // New customer phone number entry
    private void newPhoneBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        AddCustomerForm_Updated();
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

    //====================================================================================================
    // Triggers for Add Vehicle Button in new vehicle form
    //====================================================================================================
    private void AddVehicleForm_Updated()
    {
        if (yearPicker.SelectedItem == null || makePicker.SelectedItem == null || modelPicker.SelectedItem == null || colourPicker.SelectedItem == null || vinEntry.Text == null || vinEntry.Text.Length != 17)
        {
            AddThisVehicleBtn.IsEnabled = false;
            return;
        }

        AddThisVehicleBtn.IsEnabled = true;
    }

    private void yearPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddVehicleForm_Updated();
    }

    private void vinEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue == null || e.NewTextValue == "") { return; }
        var entry = (Entry)sender;
        // Convert the entered text to uppercase
        string newText = e.NewTextValue.ToUpper();
        // Ensure that the entered text contains only numbers and capital letters
        string validCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string filteredText = "";
        foreach (char c in newText)
        {
            if (validCharacters.Contains(c))
            {
                filteredText += c;
            }
        }
        // Update the entry's text with the filtered text
        entry.Text = filteredText;
        AddVehicleForm_Updated();

    }

    private void modelPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddVehicleForm_Updated();
    }

    private void colourPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddVehicleForm_Updated();
    }

    private void editNameBox_TextChanged(object sender, TextChangedEventArgs e)
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
            AddCustomerForm_Updated();
        }
    }
}

