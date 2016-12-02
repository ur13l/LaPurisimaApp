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

		public static List<Pedido> _pedidos = null;

		ObservableCollection<Pedido> _itemsList = new ObservableCollection<Pedido>();
		Dictionary<int, Pedido> _pedidosDict = new Dictionary<int, Pedido>();


		public OrdersPage()
		{
			InitializeComponent();

			//_itemsList.Add(new OrderItem()
			//{
			//	Street = "calle 1",
			//	Addres1 = "colonia 1",
			//	Order = "garrafones 1"
			//});

			//_itemsList.Add(new OrderItem()
			//{
			//	Street = "calle 2",
			//	Addres1 = "colonia 2",
			//	Order = "garrafones 2",
			//	Image = "icon.png",
			//});
			ListView.ItemsSource = _itemsList;
			ListView.IsRefreshing = true;

			Title = "Pedidos";


			GetPedidos();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			InitViews();

		}

		async void InitViews()
		{//await Task.WhenAll(new Task[] { _labelTitle.FadeTo(1, App.AnimationSpeed, Easing.SinIn), _txtMail.FadeTo(1, App.AnimationSpeed, Easing.SinIn), _txtPass.FadeTo(1, App.AnimationSpeed, Easing.SinIn), _container.TranslateTo(0, 0, App.AnimationSpeed, Easing.SinIn) });

			ListView.Refreshing += (sender, e) =>
			{
				ListView.IsRefreshing = true;
				GetPedidos();
			};


			await Task.Delay(300);
			//await Task.WhenAll(new Task[] { _header.ScaleTo(1, 250, Easing.SinIn) });



		}

		async void TapItem(object sender, System.EventArgs e)
		{
			var stack = (StackLayout)sender;
			//await Task.WhenAll(new Task[]{stack.ScaleTo(1.2), stack.ScaleTo(1) });
			await stack.ScaleTo(1.1,100,Easing.SinIn);
			await stack.ScaleTo(1, 100, Easing.SinIn);
		}

		async void GetPedidos()
		{
			if (_pedidos == null)
				_pedidos = await ClientLaPurisima.GetPedidos(PropertiesManager.GetUserInfo());


			if (_pedidos != null)
			{

				_pedidos = _pedidos.Where(x=>x.cliente_id == PropertiesManager.GetUserInfo().id).ToList();

				System.Diagnostics.Debug.WriteLine(_pedidos.Count);

				_itemsList.Clear();
				foreach (var item in _pedidos)
				{
					//if (!_pedidosDict.Keys.Contains(item.id))
					//{
					_itemsList.Add(item);
					//}
				}
			}


			ListView.IsRefreshing = false;
		} 
	}
}

