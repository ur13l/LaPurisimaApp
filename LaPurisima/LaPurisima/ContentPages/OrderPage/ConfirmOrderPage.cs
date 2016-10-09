using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaPurisima
{
	public partial class ConfirmOrderPage : ContentPage
	{
		Pedido _pedido;

		public ConfirmOrderPage(Pedido pedido = null)
		{
			InitializeComponent();

			if(pedido == null)
			_pedido = new Pedido()
			{
				direccion = "conocida",
				detalles = new List<Producto>()
				{
					new Producto(){
						nombre = "p1",
						cantidad = 3,
							precio = 40,
					},
					new Producto(){
						nombre = "p2",
						cantidad = 1,
						precio = 40,
					},
				},
			};
		}
	}
}
