namespace MechanicShop.Views;

public partial class CustomerView : ContentPage
{
	public CustomerView()
	{
		InitializeComponent();
	}


    private void clearCustomerSearch_Clicked(object sender, EventArgs e)
    {
        // inactive unless searh fields are filled in
        // clears search form
    }

    private void searchCustoemrsBtn_Clicked(object sender, EventArgs e)
    {
        // inactive unless searfh fields are filled in
        // Pairs down collectionview to only items matching search terms
    }

    private void deleteCustomer_Clicked(object sender, EventArgs e)
    {
        // make inactive unless vehicle is selected
        // Confirmation message to prevent accidental detetion
    }

    private void updateCustomerInfo_Clicked(object sender, EventArgs e)
    {
        // Modal view? update info or cancel
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
}