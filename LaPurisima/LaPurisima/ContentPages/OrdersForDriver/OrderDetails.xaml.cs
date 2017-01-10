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
					ShowProgressType(IProgressType.Done);
					await Task.Delay(600);
					HideProgress();
					//await DisplayAlert("", "Pedido actualizado correctamente", "ok");
					await Navigation.PopAsync();
				}

			}
		}

		async void Cancelar(object sender, System.EventArgs e)
		{
			var user = PropertiesManager.GetUserInfo();
			if (user != null)
			{

				_pedido.api_token = user.api_token;
				var res = await ClientLaPurisima.PostObject<Pedido>(_pedido, WEB_METHODS.CancelarPedido);
				if (ClientLaPurisima.IsErrorFalse(res))
				{
					await DisplayAlert("", "Error al actualizar el pedido", "ok");
				}
				else {
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

				_direccion.Text = _pedido.direccion;
				var productos = new List<Producto>();

				foreach (var item in _pedido.detalles)
				{
					item.producto.cantidad = item.cantidad;
					productos.Add(item.producto);
				}

				_listView.ItemsSource = productos;
				_listView.HeightRequest = (productos.Count * 60) + 60;
				_totalLabel.Text = string.Format("Total: ${0}", productos.Sum(x => x.total));


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
