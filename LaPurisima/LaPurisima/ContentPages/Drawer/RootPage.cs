using System;

using Xamarin.Forms;

namespace LaPurisima
{
	public class RootPage : MasterDetailPage
	{

		DrawerListPage _drawer;

		public RootPage()
		{

			_drawer = new DrawerListPage();
			_drawer.PageSelected += async (pageType) =>
			{
				switch (pageType)
				{
					case DrawerPage.MakeOrder:
						Detail = new NavigationPage(new CarouselPageOrder(this));
						break;
					case DrawerPage.Orders:
						//Detail = new NavigationPage(new OrdersPage());
						Detail = new NavigationPage(new CarouselOrdering(this));
						break;
					case DrawerPage.Profile:      
						Detail = new NavigationPage(new Profile());
						break;
					case DrawerPage.Settings:
						break;
					case DrawerPage.LogOut:
						PropertiesManager.LogOut();
						await Navigation.PopModalAsync();
						break;
						
				}

				IsPresented = false;
			};
			Master = _drawer;
			Detail = new NavigationPage(new CarouselPageOrder(this));

			MasterBehavior = MasterBehavior.Popover;

			IsPresentedChanged += (sender, e) =>
			{
				_drawer.UpdateView();
			};
		}

	}
}


