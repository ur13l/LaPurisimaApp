using System;
using Realms;
using Xamarin.Forms;

namespace LaPurisima
{


	public class Producto : Realms.RealmObject
	{
		public int id { get; set; }
		public string nombre { get; set; }
		public string descripcion { get; set; }
		public int stock { get; set; }
		public int contenido { get; set; }
		public double precio { get; set; }
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
				return id;
			}
		}
		[Ignored]
		public int cantidad { get; set; }
		[Ignored]
		public double total
		{
			get
			{
				var t = (cantidad * precio);
				return t;
			}
		}
		[Ignored]
		public string totalLabel
		{
			get
			{
				var t = (cantidad * precio);
				string s = String.Format("{0:$#,##0.00}", total);
				return s;
			}
		}
		bool _isImageVisible = true;
		[Ignored]
		public bool IsImageVisible
		{
			get
			{
				return _isImageVisible;
			}
			set
			{
				_isImageVisible = value;
			}
		}
		[Ignored]
		public Color CostoColor
		{
			get
			{
				if (precio > 0)
				{
					return Color.FromHex("#848484");
				}
				else {
					return Color.Green;
				}
			}
		}

		[Ignored]
		public Color BGColor
		{
			get
			{
				if (precio > 0)
				{
					return Color.White;
				}
				else {
					return Color.FromHex("#FAFAFA");
				}
			}
		}
		#endregion
	}

	//public class Producto : Realms.RealmObject
	//{
	//	public int id { get; set; }
	//	public string nombre { get; set; }
	//	public string descripcion { get; set; }
	//	public int stock { get; set; }
	//	public int contenido { get; set; }
	//	public int precio { get; set; }
	//	public string imagen { get; set; }
	//	public string created_at { get; set; }
	//	public string updated_at { get; set; }
	//	public string deleted_at { get; set; }

	//	public override string ToString()
	//	{
	//		return string.Format("[Producto: id={0}, nombre={1}, descripcion={2}, stock={3}, contenido={4}, precio={5}, imagen={6}, created_at={7}, updated_at={8}, deleted_at={9}, producto_id={10}, cantidad={11}]", id, nombre, descripcion, stock, contenido, precio, imagen, created_at, updated_at, deleted_at, producto_id, cantidad);
	//	}

	//	#region PEDIDO
	//	[Ignored]
	//	public int producto_id
	//	{
	//		get
	//		{
	//			if (id != null)
	//				return id;

	//			return -1;
	//		}
	//	}
	//	[Ignored]
	//	public int cantidad { get; set; }
	//	[Ignored]
	//	public int total { get { return cantidad * precio; }}
	//	#endregion
	//}
}
