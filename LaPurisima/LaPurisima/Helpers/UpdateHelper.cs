using System;
using System.Collections.Generic;

namespace LaPurisima
{
	public class UpdateHelper
	{
		

		public static async void UpdateInfo()
		{
			var productos = await ClientLaPurisima.GetObject<List<Producto>>(WEB_METHODS.GetProductos);
			RealmHelper.SetProductos(productos);


			System.Diagnostics.Debug.WriteLine("updatet info");
		}
	}
}
