using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaPurisima
{
	public class UpdateHelper
	{
		

		public static async Task UpdateInfo()
		{
			var productos = await ClientLaPurisima.GetObject<List<Producto>>(WEB_METHODS.GetProductos);
			RealmHelper.SetProductos(productos);


			System.Diagnostics.Debug.WriteLine("updatet info");
		}
	}
}
