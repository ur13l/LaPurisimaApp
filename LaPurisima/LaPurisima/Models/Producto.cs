using System;
namespace LaPurisima
{
	public class Producto
	{
		public int id { get; set; }
		public string nombre { get; set; }
		public int stock { get; set; }
		public int contenido { get; set; }
		public int precio { get; set; }
		public string imagen { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
		public object deleted_at { get; set; }

		#region PEDIDO
		public int producto_id
		{
			get
			{
				if (id != null)
					return id;

				return -1;
			}
		}
		public int cantidad { get; set; }
		#endregion
	}
}
