using System;

using Xamarin.Forms;

namespace LaPurisima
{
	public class RootPage : MasterDetailPage
	{
		public RootPage()
		{

			var drawer = new DrawerListPage();
			drawer.PageSelected += (pageType) =>
			{
				switch (pageType)
				{
					case DrawerPage.MakeOrder:
						Detail = new NavigationPage(new MakeOrderPage());
						break;
					case DrawerPage.Orders:
						Detail = new NavigationPage(new OrdersPage());
						break;
					case DrawerPage.Settings:

						break;
				}

				IsPresented = false;
			};
			Master = drawer;
			Detail = new NavigationPage(new MakeOrderPage());

			MasterBehavior = MasterBehavior.Popover;



		}
	}
}


