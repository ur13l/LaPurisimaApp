using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class OrderDetails : BasePage
	{

		Pedido _pedido;

		public OrderDetails(Pedido pedido)
		{
			InitializeComponent();
			_pedido = pedido;

			if (pedido != null)
				Title = "Orden " + _pedido.id;
			InitViews();

			var user = PropertiesManager.GetUserInfo();
			if (user.tipo_usuario_id != 2)
			{
				_navigatie.IsVisible = false;
				_entregar.IsVisible = false;
				_contNombre.IsVisible = false;
			}

		}

		async void ComoLlegar(object sender, System.EventArgs e)
		{

			if (Device.OS == TargetPlatform.Android)
			{
				//var str = string.Format("google.navigation:q={0},{1}&mode=d", _pedido.latitud, _pedido.longitud, _pedido.direccionLabel);
				var str = string.Format("geo:0,0?q={0},{1} (Pedido {3})", _pedido.latitud, _pedido.longitud, _pedido.direccionLabel, _pedido.id);
				Device.OpenUri(new Uri(str));
				return;
			}

			List<string> buttons = new List<string>();

			if (DependencyService.Get<IValidUrl>().Valid(@"maps://"))
			{
				buttons.Add("Apple Maps");
			}

			if (Device.OS == TargetPlatform.Android || DependencyService.Get<IValidUrl>().Valid(@"comgooglemaps://"))
			{
				buttons.Add("Google Maps");
			}

			if (DependencyService.Get<IValidUrl>().Valid(@"waze://"))
			{
				buttons.Add("Waze");
			}

			if (buttons.Count > 1)
			{
				string action = await DisplayActionSheet("Abrir en...", "Cancelar", null, buttons.ToArray());


				if (action == "Apple Maps")
				{
					LaunchMapApp.Open(new Geocode { Latitude = _pedido.latitud.ToString(), Longitude = _pedido.longitud.ToString() }, TypeMap.Apple);
				}
				else if (action == "Google Maps")
				{
					LaunchMapApp.Open(new Geocode { Latitude = _pedido.latitud.ToString(), Longitude = _pedido.longitud.ToString() }, TypeMap.Google);
				}
				else if (action == "Waze")
				{
					LaunchMapApp.Open(new Geocode { Latitude = _pedido.latitud.ToString(), Longitude = _pedido.longitud.ToString() }, TypeMap.Waze);
				}
			}
			else if (buttons.Count == 1)
			{
				string action = buttons[0];
				if (action == "Apple Maps")
				{
					LaunchMapApp.Open(new Geocode { Latitude = _pedido.latitud.ToString(), Longitude = _pedido.longitud.ToString() }, TypeMap.Apple);
				}
				else if (action == "Google Maps")
				{
					LaunchMapApp.Open(new Geocode { Latitude = _pedido.latitud.ToString(), Longitude = _pedido.longitud.ToString() }, TypeMap.Google);
				}
				else if (action == "Waze")
				{
					LaunchMapApp.Open(new Geocode { Latitude = _pedido.latitud.ToString(), Longitude = _pedido.longitud.ToString() }, TypeMap.Waze);
				}
			}
		}

		async void Entregar(object sender, System.EventArgs e)
		{
			OrdersPage.Changed = true;
			var user = PropertiesManager.GetUserInfo();
			if (user != null)
			{
				ShowProgress("Actualizando");
				_pedido.api_token = user.api_token;
				var res = await ClientLaPurisima.PostObject<Pedido>(_pedido, WEB_METHODS.FinalizarPedido);
				if (ClientLaPurisima.IsErrorFalse(res))
				{
					HideProgress();
				}
				else {
					HideProgress();
					ShowProgress(IProgressType.Done);
					await Task.Delay(600);
					HideProgress();
					//await DisplayAlert("", "Pedido actualizado correctamente", "ok");
					await Navigation.PopAsync();
				}

			}
		}

		async void Cancelar(object sender, System.EventArgs e)
		{
			OrdersPage.Changed = true;
			var user = PropertiesManager.GetUserInfo();
			if (user != null)
			{
				ShowProgress("Cancelando");
				_pedido.api_token = user.api_token;
				var res = await ClientLaPurisima.PostObject<Pedido>(_pedido, WEB_METHODS.CancelarPedido);
				if (ClientLaPurisima.IsErrorFalse(res))
				{
					HideProgress();
					await DisplayAlert("", "Error al cancelar el pedido", "ok");
				}
				else {
					ShowProgress(IProgressType.Done);
					await Task.Delay(600);
					HideProgress();
					await DisplayAlert("", "Pedido cancelado correctamente", "ok");
					await Navigation.PopAsync();
				}
			}
		}

		void InitViews()
		{
			_nombreCliente.IsEnabled = false;
			_direccion.IsEnabled = false;

			_listView.ItemSelected += (sender, e) =>
			{
				if (e.SelectedItem == null)
					return;
				_listView.SelectedItem = null;
			};

			if (_pedido != null)
			{
				if (_pedido.cliente != null)
				{
					_nombreCliente.Text = _pedido.cliente.nombre;
				}

				_direccion.Text = _pedido.direccionLabel;
				_referencias.Text = _pedido.referenciasLabel;
				var productos = new List<Producto>();

				foreach (var item in _pedido.detalles)
				{
					item.producto.cantidad = item.cantidad;
					productos.Add(item.producto);

					try
					{
						var des = _pedido.detalles_descuento.Where(x => (x.descuento != null) && x.descuento.producto_id == item.producto.id).FirstOrDefault();

						double descuento = 0;


						var p = item.producto;

						if (des.descuento.descuento_porcentaje > 0)
						{
							var n = p.precio * ((double)des.descuento.descuento_porcentaje / 100);
							descuento += n;
						}
						else {
							var n = des.descuento.descuento;
							if(n!=null)
								descuento += (int)n;
						}


						for (int n = 0; n < des.cantidad; n++)
						{

							productos.Add(new Producto()
							{
								precio = -descuento,
								imagen = null,
								nombre = "DESCUENTO: "+item.producto.nombre,
								cantidad = 1,
								IsImageVisible = false,
							});
						}


					}
					catch
					{

					}


				}

				_listView.ItemsSource = productos;
				_listView.HeightRequest = (productos.Count * 60) + 160;

				var precio = _pedido.total;
				_precioLabel.Text = string.Format("{0:C}", precio);
				_descuentoLabel.Text = string.Format("{0:C}", _pedido.total - _pedido.Total);
				var total = _pedido.Total;
				_totalLabel.Text = string.Format("{0:C}", total);

				if (_pedido.detalles_descuento != null && _pedido.detalles_descuento.Count == 0)
				{
					_descuento1.IsVisible = false;
					_descuento2.IsVisible = false;
					_precioLbl.Text = "Total:";
				}


				if (_pedido.status == 1 || _pedido.status == 2 || _pedido.status == 3)
				{
					_cancelar.IsVisible = true;
				}
				else {
					_cancelar.IsVisible = false;
				}
			}
		}
	}
}
