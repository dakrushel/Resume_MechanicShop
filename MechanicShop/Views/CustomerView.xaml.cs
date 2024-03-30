using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using MechanicShop.Models;
using Microsoft.Maui.Controls;



namespace MechanicShop.Views;

public partial class CustomerView : ContentPage
{
  
    public CustomerView()
	{
		InitializeComponent();



        var customers = new ObservableCollection<Customer>(MauiProgram.ShopDB.GetAllCustomers());
        customersCollectionView.ItemsSource = customers;
    }


    // Page formatting upon refreshing page (or clearing a certain form)-------------------------------
    protected override void OnAppearing()
    {
        // override initialize to get changes any time the page loads, rather than only
        // initial loading of the page. 
        base.OnAppearing();
        ResetSearchWidget();
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
    //-----------------------------------------------------------------------------------------
    // Customer Search Box---------------------------------------------------------------------
    private void clearCustomerSearch_Clicked(object sender, EventArgs e)
    {
        // inactive unless search fields filled in
        ResetSearchWidget();
    }
    private void searchCustoemrsBtn_Clicked(object sender, EventArgs e)
    {
        // TODO: this method!
        // inactive unless searfh fields are filled in
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
        }


    }
    //----------------------------------------------------------------------------------------
    // Add new customer form------------------------------------------------------------------
    private async void AddThisCustomerBtn_Clicked(object sender, EventArgs e)
    {
        //TODO null values and shit, handle it!
        string? newCustomerName = newNameBox.Text;
        string? newPhoneNumber = newPhoneBox.Text;
        string? newEmail = newEmailBox.Text;

        
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

        if (newCustomerName != null && newPhoneNumber != null & newEmail != null)
        {
            Customer newCustomer = new Customer(newCustomerName, newEmail, newPhoneNumber);
            MauiProgram.ShopDB.AddCustomer(newCustomer);
            ResetAddCustomerForm();
            ResetSearchWidget();
        }     
    }

    private void cancelAddCustomer_Clicked(object sender, EventArgs e)
    {
        addCustomerForm.IsVisible = false;
        searchCustomers.IsEnabled = true;


    }
    //-----------------------------------------------------------------------------------------
    // EDIT CUSTOMER INFORMATION FORM----------------------------------------------------------
    private void cancelEditCustomer_Clicked(object sender, EventArgs e)
    {
        ResetEditCustomerForm();
    }

    private async void EditThisCustomerBtn_Clicked(object sender, EventArgs e)
    {
        //TODO why does this not work?
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
        toEdit.Name = editNameBox.Text;
        toEdit.Address = editEmailBox.Text;
        MauiProgram.ShopDB.UpdateCustomer(toEdit);
        cNameBox.Text = editNameBox.Text;
        cEmailBox.Text = editEmailBox.Text;
        ResetEditCustomerForm();
        ResetSearchWidget();
    }
    

    //-----------------------------------------------------------------------------------------
    // CUSTOMER INFORMATION DISPLAY -----------------------------------------------------------
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
    }

    private void updateCustomerInfo_Clicked(object sender, EventArgs e)
    {
        EditCustomerForm.IsVisible = true;
        searchCustomers.IsEnabled = false;
        customerDisplay.IsEnabled = false;
        editNameBox.Text = cNameBox.Text;
        editPhoneBox.Text = cPhoneBox.Text;
        editEmailBox.Text = cEmailBox.Text;

    }
    private void addVehicle_Clicked(object sender, EventArgs e)
    {
        // Add in a popup???
        // Can we use pickers with pre loaded makes and models in a popup???
        // Look into MODAL NAVIAGATION pages!
    }

    private void editVehicle_Clicked(object sender, EventArgs e)
    {
        // make inactive unless vehicle is selected
        // Edit in a popup???
        // Give option to cancel!!!
    }

    private void deleteVehicle_Clicked(object sender, EventArgs e)
    {
        // make inactive unless vehicle is selected
        // Confirmation message to prevent accidental detetion
    }

    private void newAppointmentBtn_Clicked(object sender, EventArgs e)
    {

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

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        //TODO highlight
    }

    private void newPhoneBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // TODO: VALIDATE PHONE NUMBER ENTRY
        
       
        
        
        



    }

    
}

    