using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaPurisima
{

	public partial class OrdersPage : BasePage
	{

		public static bool Changed = false;
		public static List<Pedido> _pedidos = null;

		ObservableCollection<Pedido> _itemsList = new ObservableCollection<Pedido>();
		Dictionary<int, Pedido> _pedidosDict = new Dictionary<int, Pedido>();


		public OrdersPage()
		{
			InitializeComponent();


			ListView.ItemsSource = _itemsList;

			Title = "Historial de Pedidos";


			InitViews();
			//Changed = true;
			GetPedidos();
			NavigationPage.SetBackButtonTitle(this, "Atrás");
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			GetPedidos(Changed);

		}

		async void InitViews()
		{

			ListView.Refreshing += (sender, e) =>
			{
				GetPedidos();
			};
		}

		async void TapItem(object sender, System.EventArgs e)
		{
			var stack = (StackLayout)sender;
			//await Task.WhenAll(new Task[]{stack.ScaleTo(1.2), stack.ScaleTo(1) });
			await stack.ScaleTo(0.9, 100, Easing.SinIn);
			await stack.ScaleTo(1, 100, Easing.SinIn);
			//stack.ScaleTo(1.1, 100, Easing.SinIn);


			if (stack.BindingContext is Pedido)
			{
				var orden = stack.BindingContext as Pedido;

				await Navigation.PushAsync(new OrderDetails(orden));
			}
		}

		async void GetPedidos(bool update = false)
		{
			//Device.BeginInvokeOnMainThread(() =>
			//{
			//	ListView.IsRefreshing = true;
			//});
			ShowProgress((update)?"Actualizando pedidos":"Obteniendo pedidos");
			//if (_pedidos == null)
			_pedidos = await ClientLaPurisima.GetPedidos(PropertiesManager.GetUserInfo());

			var user = PropertiesManager.GetUserInfo();

			if (_pedidos != null && user != null)
			{

				//solicitados, asignados o en camino que no tengan mas de 7 dias
				_pedidos = _pedidos.Where(x => x.cliente_id == user.id
										  //&& x.status <=3 
										  && ((DateTime.Now - x.fechaDateTime).TotalDays <= 7)).
								   OrderByDescending(x => x.fechaDateTime).ToList();

				_itemsList.Clear();
				foreach (var item in _pedidos)
				{

					//if (!_pedidosDict.Keys.Contains(item.id))
					//{
					_itemsList.Add(item);
					//}
				}

				Device.BeginInvokeOnMainThread(() =>
				{
					//ListView.IsRefreshing = true;
					if (ListView.ItemsSource != _itemsList)
						ListView.ItemsSource = _itemsList;
				});


				Changed = false;
			}

			HideProgress();


			//Device.BeginInvokeOnMainThread(() =>
			//{
			//	ListView.IsRefreshing = false;
			//});
		}
	}
}

