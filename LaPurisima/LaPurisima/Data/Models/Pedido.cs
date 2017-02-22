using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaPurisima
{



	public class Pedido
	{
		public string api_token;

		public double? latitud { get; set; }
		public double? longitud { get; set; }
		public string direccion { get; set; }


		public string direccionLabel
		{
			get
			{
				if (direccion != null)
				{
					if (direccion.Contains("Referencias:"))
						return direccion.Substring(0, direccion.IndexOf("Referencias:"));
					else {
						return direccion;
					}
				}

				return "";
			}
		}

		public string referenciasLabel
		{
			get
			{
				if (direccion != null)
				{
					if (direccion.Contains("Referencias:"))
						return direccion.Substring(direccion.IndexOf("Referencias:"), direccion.Length - direccion.IndexOf("Referencias:")).Replace("Referencias:", "");
					else {
						return "";
					}
				}

				return "";
			}
		}

		//public int status { get; set; }
		public string fecha { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
		public string deleted_at { get; set; }
		//p+ublic List<string> detalles { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public DateTime fechaDateTime
		{
			get
			{
				try
				{
					return DateTime.Parse(fecha);
				}
				catch (Exception ex)
				{

				}

				return DateTime.MinValue;
			}
		}
		public List<Producto> productos { get; set; }
		public List<DetallePedido> detalles { get; set; }
		public List<DetallesDescuento> detalles_descuento { get; set; }

		public int? status { get; set; }
		public int? total { get; set; }
		public Repartidor repartidor { get; set; }

		public int id { get; set; }
		public int id_pedido { get { return id; } }
		public int cliente_id { get; set; }
		public int? conductor_id { get; set; }

		public User cliente { get; set; }


		[Newtonsoft.Json.JsonIgnore]
		public string ProductosLabel
		{
			get
			{
				string str = "";

				if (detalles != null)
				{

					if (detalles.Count > 1)
					{
						str += string.Join("", detalles.GetRange(0, detalles.Count).Select(x => string.Format("{0} {1}  \n", x.cantidad, x.producto.nombre)));
						//str += string.Format(Environment.NewLine + "-{0}", detalles.Last().producto.nombre);
					}
					else if (detalles.Count > 0)
						str = string.Format("{0} {1} ", detalles.Last().cantidad, detalles.Last().producto.nombre);

				}


				return str;
			}
			set
			{

			}
		}

		string GetStringContenido(int contenido)
		{
			if (contenido >= 1000)
				return string.Format("{0} L", (contenido / 1000));
			else if (contenido < 1000)
				return string.Format("{0} ml", contenido);

			return string.Format("{0} ml", contenido);
		}

		[Newtonsoft.Json.JsonIgnore]
		public double Total
		{
			get
			{

				//double total = detalles.Sum(x => x.producto.precio * x.cantidad);
				//double desc_percent = 0;
				//if (detalles_descuento != null)
				//{
				//	//double descuento = detalles_descuento.Sum(x => x.descuento);

				//	var descuento = detalles_descuento.Sum(x =>
				//	{
				//		int sum = 0;

				//		//sum += ) * x.descuento.descuento;
				//		var prod = detalles.Where(y => y.producto_id == x.descuento.producto_id).Select(y => y.producto).FirstOrDefault();
				//		if (prod != null)
				//		{

				//		}
				//		else {

				//			if (x.descuento.descuento_porcentaje != 0)
				//			{

				//			}
				//			else {
				//				return x.descuento.descuento;
				//			}
				//		}

				//		return sum;
				//	});

				//	total = total - descuento;
				//}

				if (this.total == null)
					this.total = 0;

				double total = (double) this.total;
				double descuento = 0;
				foreach (var d in detalles_descuento)
				{
					//if(d.descuento.producto_id
					if ( d.descuento!=null && d.descuento.producto_id != null)
					{

						var dd = detalles.Where(x => x.producto_id == d.descuento.producto_id).FirstOrDefault();
						if (dd.producto != null)
						{
							var p = dd.producto;

							if (d.descuento.descuento_porcentaje > 0)
							{
								var n = p.precio - (((double)d.descuento.descuento_porcentaje * 100) / p.precio);
								descuento += n * dd.cantidad;
							}
							else {
								var n = d.descuento.descuento;
								descuento += n * dd.cantidad;
							}
						}

					}
					else {

						if (d.descuento != null)
						{
							if (d.descuento.descuento_porcentaje > 0)
							{
								descuento += (total * ((double)d.descuento.descuento_porcentaje / 100.0));
							}
							else {
								descuento += d.descuento.descuento;
							}
						}
					}
				}
				var t = total - descuento;

				return t;
			}

		}

		public string StatusLabel
		{
			get
			{
				switch (status)
				{
					case 1:
						return "Solicitado";
					case 2:
						return "Asignado";
					case 3:
						return "En camino";
					case 4:
						return "Entregado";
					case 5:
						return "Cancelado";
					case 6:
						return "Fallido";
					default:
						return "";
				}
			}
		}

		public Color StatusColor
		{
			get
			{
				switch (status)
				{
					case 1:
						return Color.FromHex("#777777");
					case 2:
						return Color.FromHex("#347AB7");
					case 3:
						return Color.FromHex("#347AB7");
					case 4:
						return Color.FromHex("#5CB85C");
					case 5:
						return Color.FromHex("#AF1619");
					case 6:
						return Color.FromHex("#AF1619");
					default:
						return Color.FromHex("#777777");
				}
			}
		}


	}

	//public class Pedido
	//{
	//	public string api_token { get; set; }
	//	public double? latitud { get; set; }
	//	public double? longitud { get; set; }
	//	public string direccion { get; set; }
	//	public string fecha { get; set; }




	//}
	public class DetallePedido
	{
		public int id { get; set; }
		public int producto_id { get; set; }
		public int pedido_id { get; set; }
		public int cantidad { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
		public object deleted_at { get; set; }
		public Producto producto { get; set; }
	}

	public class Descuento
	{
		public int id { get; set; }
		public int user_id { get; set; }
		public int? producto_id { get; set; }
		public int descuento { get; set; }
		public int? descuento_porcentaje { get; set; }
		public string fecha_vencimiento { get; set; }
		public int? usos_restantes { get; set; }
		public DateTime created_at { get; set; }
		public DateTime? updated_at { get; set; }
		public DateTime? deleted_at { get; set; }
	}

	public class DetallesDescuento
	{
		public int id { get; set; }
		public int pedido_id { get; set; }
		public int descuento_id { get; set; }
		public Descuento descuento { get; set; }
		public DateTime? created_at { get; set; }
		public DateTime? updated_at { get; set; }
	}

}
