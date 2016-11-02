using System;
using System.Collections.Generic;
using System.Linq;
using Realms;

namespace LaPurisima
{
	public class RealmHelper
	{
		#region PRODUCTOS

		public static void SetProductos(List<Producto> productos)
		{
			var realm = Realm.GetInstance();

			realm.Write(() =>
			{
				realm.RemoveAll<Producto>();

				if (productos != null)
					foreach (var producto in productos)
					{
						var productoRealm = realm.CreateObject<Producto>();
						productoRealm.id = producto.id;
						productoRealm.nombre = producto.nombre;
						productoRealm.descripcion = producto.descripcion;
						productoRealm.stock = producto.stock;
						productoRealm.contenido = producto.contenido;
						productoRealm.precio = producto.precio;
						productoRealm.imagen = producto.imagen;
						productoRealm.created_at = producto.created_at;
						productoRealm.updated_at = producto.updated_at;
						productoRealm.deleted_at = producto.deleted_at;
					}
			});
		}

		public static List<Producto> GetProductos()
		{
			var realm = Realm.GetInstance();

			var productos = realm.All<Producto>().ToList();

			return productos;
		}

		#endregion
	}
}
