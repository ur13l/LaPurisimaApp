using System;
using System.Collections.Generic;

namespace LaPurisima
{
	public class HelperOrdenPage
	{
		public static string streetNumber;
		public static string country;

		public static string street { get; set; }
		public static string colony { get; set; }
		public static string city { get; set; }
		public static string state { get; set; }
		public static string postalcode { get; set; }

		public static double latitud { get; set; }
		public static double longitud { get; set; }

		public static Pedido Pedido { get; set; }


		public static Pedido StockRepartidor { get; set; }
	}
}
