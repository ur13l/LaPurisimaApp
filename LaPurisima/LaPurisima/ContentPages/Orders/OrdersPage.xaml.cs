﻿using System;
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

	
			ListView.ItemsSource = _itemsList;
			ListView.IsRefreshing = true;

			Title = "HISTORICO DE PEDIDOS";


			InitViews();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			GetPedidos();

		}

		async void InitViews()
		{
			
			ListView.Refreshing += (sender, e) =>
			{
				ListView.IsRefreshing = true;
				GetPedidos();
			};
		}

		async void TapItem(object sender, System.EventArgs e)
		{
			var stack = (StackLayout)sender;

			await stack.ScaleTo(1.1,100,Easing.SinIn);
			await stack.ScaleTo(1, 100, Easing.SinIn);


			if (stack.BindingContext is Pedido)
			{
				var orden = stack.BindingContext as Pedido;

				await Navigation.PushAsync(new OrderDetails(orden));
			}
		}

		async void GetPedidos()
		{
			//if (_pedidos == null)
				_pedidos = await ClientLaPurisima.GetPedidos(PropertiesManager.GetUserInfo());

			var user = PropertiesManager.GetUserInfo();

			if (_pedidos != null && user != null)
			{

				_pedidos = _pedidos.Where(x=>x.cliente_id == user.id).ToList();

				System.Diagnostics.Debug.WriteLine(_pedidos.Count);

				_itemsList.Clear();
				foreach (var item in _pedidos)
				{
					
					//if (!_pedidosDict.Keys.Contains(item.id))
					//{
					_itemsList.Add(item);
					//}
				}

				if (ListView.ItemsSource != _itemsList)
					ListView.ItemsSource = _itemsList;
			}


			ListView.IsRefreshing = false;
		} 
	}
}
