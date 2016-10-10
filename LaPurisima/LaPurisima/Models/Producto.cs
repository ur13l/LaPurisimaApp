using System;
using Realms;

namespace LaPurisima
{
	public class Producto : Realms.RealmObject
	{
		public int id { get; set; }
		public string nombre { get; set; }
		public string descripcion { get; set; }
		public int stock { get; set; }
		public int contenido { get; set; }
		public int precio { get; set; }
		public string imagen { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
		public string deleted_at { get; set; }

		public override string ToString()
		{
			return string.Format("[Producto: id={0}, nombre={1}, descripcion={2}, stock={3}, contenido={4}, precio={5}, imagen={6}, created_at={7}, updated_at={8}, deleted_at={9}, producto_id={10}, cantidad={11}]", id, nombre, descripcion, stock, contenido, precio, imagen, created_at, updated_at, deleted_at, producto_id, cantidad);
		}

		#region PEDIDO
		[Ignored]
		public int producto_id
		{
			get
			{
				if (id != null)
					return id;

				return -1;
			}
		}
		[Ignored]
		public int cantidad { get; set; }
		#endregion
	}
}
