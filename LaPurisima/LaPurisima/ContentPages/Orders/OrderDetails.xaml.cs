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
				_entregar.IsVisible = false;
				_contNombre.IsVisible = false;
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
				}

				_listView.ItemsSource = productos;
				_listView.HeightRequest = (productos.Count * 60) + 60;

				var precio = _pedido.total;
				_precioLabel.Text = string.Format("{0:C}", precio);
				_descuentoLabel.Text = string.Format("{0:C}", _pedido.total - _pedido.Total);
				var total = _pedido.Total;
				_totalLabel.Text = string.Format("{0:C}", total);

				if (_pedido.detalles_descuento !=null&& _pedido.detalles_descuento.Count == 0)
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
