using System;
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

			if (await Valid())
			{

				Device.BeginInvokeOnMainThread(() => ShowProgress("Validando"));



				string refe = "";
				if (!string.IsNullOrEmpty(_referencias.Text))
				{
					refe = "Referencias: " + _referencias.Text;
				}

				HelperOrdenPage.Pedido.direccion = string.Format("{0} #{1}, {2}. {3}", HelperOrdenPage.street, HelperOrdenPage.streetNumber, HelperOrdenPage.colony, refe);
				HelperOrdenPage.Pedido.detalles = new List<DetallePedido>();
				foreach (var prod in HelperOrdenPage.Pedido.productos)
				{
					HelperOrdenPage.Pedido.detalles.Add(new DetallePedido()
					{
						producto_id = prod.id,
						cantidad = prod.cantidad,
						producto = prod,
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

		}

		async Task<bool> Valid()
		{

			if (string.IsNullOrEmpty(EntryCalleProfile.Text) || EntryCalleProfile.Text.Length <= 1)
			{
				await DisplayAlert("", "Ingrese una calle", "ok");
				return false;
			}

			if (string.IsNullOrEmpty(EntryColoniaProfile.Text))
			{
				await DisplayAlert("", "Ingrese una colonia", "ok");
				return false;
			}

			if (string.IsNullOrEmpty(_referencias.Text))
			{
				await DisplayAlert("", "Ingrese unas referencias", "ok");
				return false;
			}

			if (HelperOrdenPage.Pedido.productos == null || HelperOrdenPage.Pedido.productos.Count == 0)
			{
				await DisplayAlert("","Seleccione algun producto.","Ok");
				return false;
			}


			return true;
		}

		public ConfirmOrderPage(Pedido pedido = null)
		{
			InitializeComponent();
		}

		internal void UpdateView()
		{
			//EntryNameProfile.IsEnabled = false;
			//EntryCalleProfile.IsEnabled = false;
			//EntryColoniaProfile.IsEnabled = false;
			var user = PropertiesManager.GetUserInfo();
			EntryNameProfile.Text = user.nombre;
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
			//	};+x
			if (HelperOrdenPage.Pedido != null && HelperOrdenPage.Pedido.productos != null)
			{
				HelperOrdenPage.Pedido.productos.RemoveAll(x => x.cantidad <= 0);

				_listView.ItemsSource = HelperOrdenPage.Pedido.productos;
				_listView.HeightRequest = (HelperOrdenPage.Pedido.productos.Count() * 60) + 60;
				_totalLabel.Text = string.Format("Total: ${0}", HelperOrdenPage.Pedido.productos.Sum(x => x.total));
			}
		}
	}
}