using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaPurisima
{

	public enum DrawerPage
	{
		MakeOrder,
		Orders,
		DriverOrders,
		Profile,
		Settings,
		LogOut,
	}

	public partial class DrawerListPage : ContentPage
	{
		public Action<DrawerPage> PageSelected;

		public DrawerListPage()
		{
			InitializeComponent();


			Title = "     ";

			InitViews();
		}

		void InitViews()
		{
			SetListView();

			UpdateView();

			if (PropertiesManager.IsLogedIn())
			{
				var user = PropertiesManager.GetUserInfo();

				if (user.tipo_usuario_id == 2)
				{

					_labelStatus.Text = user.status == 1 ? "En línea" : "Desconectado";

					_switchOnline.Toggled += (sender, e) =>
					{
						var isOnline = e.Value;
						RepartidorHelper.SetEstatusRepartidor(isOnline, true);

						_labelStatus.Text = e.Value ? "En línea" : "Desconectado";

					};
				}
				else {
					_statusRepartidorContainer.IsVisible = false;
				}
			}

			if (Device.OS == TargetPlatform.iOS)
			{
				Icon = "menu_icon.png";
				var x = Icon;
			}
		}

		void SetListView()
		{
			var list = new List<ItemDrawer>();

			//CONDUCTOR
			if (PropertiesManager.GetUserInfo().tipo_usuario_id == 2)
			{
				list.Add(new ItemDrawer()
				{
					Label = "Pedidos pendientes",
					Page = DrawerPage.DriverOrders,
				});

				list.Add(new ItemDrawer()
				{
					Label = "Hacer Pedido",
					Page = DrawerPage.MakeOrder,
				});

				list.Add(new ItemDrawer()
				{
					Label = "Perfil",
					Page = DrawerPage.Profile,
				});

				list.Add(new ItemDrawer()
				{
					Label = "Cerrar sesión",
					Page = DrawerPage.LogOut,
				});

			}
			else {

				list.Add(new ItemDrawer()
				{
					Label = "Hacer Pedido",
					Page = DrawerPage.MakeOrder,
				});

				list.Add(new ItemDrawer()
				{
					Label = "Pedidos",
					Page = DrawerPage.Orders,
				});

				list.Add(new ItemDrawer()
				{
					Label = "Perfil",
					Page = DrawerPage.Profile,
				});

				list.Add(new ItemDrawer()
				{
					Label = "Cerrar sesión",
					Page = DrawerPage.LogOut,
				});

			}

			UpdateView();

			if (Device.OS == TargetPlatform.iOS)
			{
				Icon = "menu_icon.png";
				var x = Icon;
			}

			ListView.ItemsSource = list;
			ListView.ItemSelected += (sender, e) =>
			{
				if (e.SelectedItem == null)
					return;
				var item = e.SelectedItem as ItemDrawer;
				if (PageSelected != null)
					PageSelected(item.Page);
			};
		}

		public void UpdateView()
		{
			if (PropertiesManager.IsLogedIn())
			{
				var user = PropertiesManager.GetUserInfo();
				_labelMail.Text = user.email;
				_labelNombre.Text = user.nombre;

				if (user.tipo_usuario_id == 2)
				{
					_switchOnline.IsToggled = user.status == 1;
				}
			}
		}

		class ItemDrawer
		{
			public string Label { get; set; }
			public DrawerPage Page { get; set; }
		}
	}
}

