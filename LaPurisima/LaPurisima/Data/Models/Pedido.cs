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

				double n = detalles.Sum(x => x.total);

				return n;
			}
			set
			{

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
		public double precio { get; set; }
		public int cantidad { get; set; }
		public double total { get { return cantidad * precio; } }
		public Producto producto { get; set; }

	}
}
