using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaPurisima
{

	public enum DrawerPage
	{
		MakeOrder,
		Orders,
		Profile,
		Settings,
	}

	public partial class DrawerListPage : ContentPage
	{
		public Action<DrawerPage> PageSelected;

		public DrawerListPage()
		{
			InitializeComponent();


			Title = "     ";

			var list = new List<ItemDrawer>();

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


		class ItemDrawer
		{
			public string Label { get; set; }
			public DrawerPage Page { get; set; }
		}
	}
}

