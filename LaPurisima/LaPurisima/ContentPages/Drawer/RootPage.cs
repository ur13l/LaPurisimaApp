using System;

using Xamarin.Forms;

namespace LaPurisima
{
	public class RootPage : MasterDetailPage
	{

		DrawerListPage _drawer;

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			System.Diagnostics.Debug.WriteLine(propertyName);
		}

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
						Detail = new NavigationPage(new OrdersPage());
						break;
					case DrawerPage.DriverOrders:
						Detail = new NavigationPage(new DriverOrders());
						break;
					case DrawerPage.Profile:
						Detail = new NavigationPage(new Profile());
						break;
					case DrawerPage.Settings:
						break;
					case DrawerPage.LogOut:

						var user = PropertiesManager.GetUserInfo();
						if (user != null)
						{
							if (user.tipo_usuario_id == 2)
							{
								try
								{
									user.status = 2; //inactivo
									var res = ClientLaPurisima.PostObject<User>(user, WEB_METHODS.SetStatusRepartidor);
								}
								catch (Exception ex)
								{
									System.Diagnostics.Debug.WriteLine("error updating status. " + ex.Message);
								}
							}
						}


						PropertiesManager.LogOut();
						await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
						break;

				}

				IsPresented = false;
			};
			Master = _drawer;

			if (PropertiesManager.GetUserInfo().tipo_usuario_id == 2)
				Detail = new ShadowNavigationPage(new DriverOrders());
			else
				Detail = new ShadowNavigationPage(new CarouselPageOrder(this));

			MasterBehavior = MasterBehavior.Popover;

			IsPresentedChanged += (sender, e) =>
			{
				_drawer.UpdateView();
			};
		}

	}

	public class ShadowNavigationPage : NavigationPage
	{

		public ShadowNavigationPage(Page page):base(page)
		{
			
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Effects.Add(new ShadowEffect()
			{
				Radius = 5,
				DistanceX = 0,
				DistanceY = 0,
				Color = Color.Black
			});
		}
	}



	public class ShadowEffect : RoutingEffect
	{
		public float Radius { get; set; }

		public Color Color { get; set; }

		public float DistanceX { get; set; }

		public float DistanceY { get; set; }

		public ShadowEffect() : base("MyProject.PanelShadowEffect")
		{
		}
	}

}


