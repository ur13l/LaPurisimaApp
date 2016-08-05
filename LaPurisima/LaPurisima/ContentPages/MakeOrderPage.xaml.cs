using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LaPurisima
{
	public partial class MakeOrderPage : ContentPage
	{
		public MakeOrderPage()
		{
			InitializeComponent();

			Title = "Hacer un pedido";


			Map.MoveToRegion(
	MapSpan.FromCenterAndRadius(new Position(26.9034632, -101.4199217), Distance.FromMiles(1.7)));
		}
	}
}

