﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class ConfirmOrderPage : BasePage
	{

		async void Pedir(object sender, System.EventArgs e)
		{

			ShowProgress("Validando");

			HelperOrdenPage.Pedido.direccion = string.Format("{0} #{1}, {2}", HelperOrdenPage.street, HelperOrdenPage.streetNumber, HelperOrdenPage.colony);
			HelperOrdenPage.Pedido.detalles = new List<DetallePedido>();
			foreach (var prod in HelperOrdenPage.Pedido.productos)
			{
				HelperOrdenPage.Pedido.detalles.Add(new DetallePedido()
				{
					producto_id = prod.id,
					cantidad = prod.cantidad,
					precio = prod.precio
				});
			}
			var response = await ClientLaPurisima.MakeOrder(HelperOrdenPage.Pedido);



			if (response != null)
			{
				OrdersPage._pedidos = null;
				ShowProgress(IProgressType.Done);
				await Task.Delay(800);
				HideProgress();
			}
			else {
				HideProgress();
				var result = await DisplayAlert(Localize.GetString("ErrorTitleText", ""), Localize.GetString("ErrorMakingOrder", ""), Localize.GetString("OkButtonLabel", ""), Localize.GetString("MakeOrderAgain", ""));
				if (!result)
					Pedir(null, null);
				return;
			}

			HideProgress();

			if (NextPage != null)
				NextPage(this);
		}




		public ConfirmOrderPage(Pedido pedido = null)
		{
			InitializeComponent();
		}

		internal void UpdateView()
		{
			EntryNameProfile.IsEnabled = false;
			EntryCalleProfile.IsEnabled = false;
			EntryColoniaProfile.IsEnabled = false;
			var user = PropertiesManager.GetUserInfo();
			EntryNameProfile.Text = user.nombre;
<<<<<<< HEAD
			EntryCalleProfile.Text = user.calle;
			EntryColoniaProfile.Text = user.colonia;
			if (pedido == null)
				_pedido = new Pedido()
				{
					api_token = user.api_token,
					longitud = "100.22",
					latitud = "-2.242",
					direccion = "conocida",
					detalles = new List<Producto>()
				{
					new Producto(){
						nombre = "Garrafón Grande",
						cantidad = 2,
						precio = 80,
					},
					new Producto(){
						nombre = "Garrafón Chico",
						cantidad = 1,
						precio = 10,
					},
				},
				};
			ListView.ItemsSource = _pedido.detalles;
=======
			EntryCalleProfile.Text = string.Format("{0} #{1}", HelperOrdenPage.street, HelperOrdenPage.streetNumber);
			//EntryColoniaProfile.Text = string.Format("{0} {1},{2}", HelperOrdenPage.colony, HelperOrdenPage.city,HelperOrdenPage.state);

			EntryColoniaProfile.Text = string.Format("{0}", HelperOrdenPage.colony);

			//if (HelperOrdenPage.Pedido == null)
			//	_pedido = new Pedido()
			//	{
			//		api_token = user.api_token,
			//		longitud = "100.22",
			//		latitud = "-2.242",
			//		direccion = "conocida",
			//		detalles = new List<Producto>()
			//	{
			//		new Producto(){
			//			nombre = "p1",
			//			cantidad = 3,
			//			precio = 40,
			//		},
			//		new Producto(){
			//			nombre = "p2",
			//			cantidad = 1,
			//			precio = 40,
			//		},
			//	},
			//	};+

			HelperOrdenPage.Pedido.productos.RemoveAll(x => x.cantidad <= 0);

			_listView.ItemsSource = HelperOrdenPage.Pedido.productos;
			_listView.HeightRequest = (HelperOrdenPage.Pedido.productos.Count() * 60) + 60;
			_totalLabel.Text = string.Format("Total: ${0}", HelperOrdenPage.Pedido.productos.Sum(x => x.total));
>>>>>>> 4c3dc91b74cf26526d1acda474aae613743d5d0e
		}
	}
}


