using System;

using Xamarin.Forms;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace LaPurisima
{

	public partial class DriverOrders : BasePage
	{

		public static List<Pedido> _pedidos = null;

		ObservableCollection<Pedido> _itemsList = new ObservableCollection<Pedido>();
		Dictionary<int, Pedido> _pedidosDict = new Dictionary<int, Pedido>();


		public DriverOrders()
		{
			InitializeComponent();


			ListView.ItemsSource = _itemsList;
			ListView.IsRefreshing = true;

			Title = "Pedidos pendientes";



		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_keepPolling = true;
			token = new CancellationToken();
			InitViews();

		}

		async void InitViews()
		{

			ListView.Refreshing += (sender, e) =>
			{
				ListView.IsRefreshing = true;
				GetPedidos();
			};

			PollWebRequest();
		}

		async void TapItem(object sender, System.EventArgs e)
		{
			var stack = (StackLayout)sender;

			await stack.ScaleTo(1.1, 100, Easing.SinIn);
			await stack.ScaleTo(1, 100, Easing.SinIn);

			if (stack.BindingContext is Pedido)
			{
				var orden = stack.BindingContext as Pedido;

				await Navigation.PushAsync(new OrderDetails(orden));
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_keepPolling = false;
			token = new CancellationToken(true);
		}

		bool _keepPolling = true;
		CancellationToken token = new CancellationToken();

		private async void PollWebRequest()
		{
			while (_keepPolling)
			{
				await GetPedidos();
				// Update the UI (because of async/await magic, this is still in the UI thread!)
				if (_keepPolling)
				{
					await Task.Delay(TimeSpan.FromSeconds(60),token);
				}
			}
		}

		async Task GetPedidos()
		{

			//TODO: MARIO AQUI!
			//el pedo es que el web, tiene que solo se puedan obtener los pedidos cuando el tipo de usuario sea 2
			_pedidos = await ClientLaPurisima.GetPedidosRepartidor(PropertiesManager.GetUserInfo());


			if (_pedidos != null)
			{

				_pedidos = _pedidos.Where(x => x.status == 2 || x.status == 3).ToList();

				System.Diagnostics.Debug.WriteLine(_pedidos.Count);

				_itemsList.Clear();
				foreach (var item in _pedidos)
				{
					//if (!_pedidosDict.Keys.Contains(item.id))
					//{



					_itemsList.Add(item);
					//}
				}

				if (_pedidos == null || _pedidos.Count == 0)
					_labelNoResutls.IsVisible = true;
				else
					_labelNoResutls.IsVisible = false;
			}
		
			ListView.IsRefreshing = false;
		}
	}
}