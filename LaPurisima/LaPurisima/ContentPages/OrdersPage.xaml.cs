using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaPurisima
{
	public partial class OrdersPage : ContentPage
	{
		public OrdersPage()
		{
			InitializeComponent();
			var list = new List<OrderItem>();
			list.Add(new OrderItem()
			{
				Street = "calle 1",
				Addres1 = "colonia 1",
				Order = "garrafones 1"
			});

			list.Add(new OrderItem()
			{
				Street = "calle 2",
				Addres1 = "colonia 2",
				Order = "garrafones 2",
				Image = "icon.png",
			});
			ListView.ItemsSource = list;

			Title = "Pedidos";
		}


		class OrderItem
		{
			public string Addres1 { get; set; }
			public string Street { get; set; }
			public string Order { get; set; }
			string image = "logo.png";

			public string Image
			{
				get
				{
					return image;
				}

				set
				{
					image = value;
				}
			}
		}
	}
}

