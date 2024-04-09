using System.Collections.ObjectModel;
using MechanicShop.Models;
using MechanicShop.Services;

namespace MechanicShop.Views;

public partial class OrderView : ContentPage
{
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
        

        RefreshTechList();
        RefreshServiceJobs();
        RefreshROs();
        

    }
    private void RefreshTechList()
    {
        var techs = new ObservableCollection<Technician>(MauiProgram.ShopDB.GetAllTechnicians());
        techList.ItemsSource = techs;
    }
    private void RefreshServiceJobs()
    {
        var allJobs = new ObservableCollection<ServiceJob>(MauiProgram.ShopDB.GetAllServiceJobs());
        serviceJobs.ItemsSource = allJobs;
    }
    private void RefreshROs()
    {
        AppointmentViewService.refreshROs();
        unasignedROs.ItemsSource = AppointmentViewService.unassigned;
        inProgressROs.ItemsSource = AppointmentViewService.inProgress;
        
    }



    //========================================================================================================
    //-----
    //========================================================================================================

    private void unasignedROs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

    }

    private void inProgressROs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

    }

    private void assignTechPicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}