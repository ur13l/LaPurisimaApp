using System;
using System.Linq;
using System.Collections.Generic;

namespace LaPurisima
{

	public class Pedido
	{
		public string api_token { get; set; }
		public double? latitud { get; set; }
		public double? longitud { get; set; }
		public string direccion { get; set; }
		public string fecha { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public DateTime fechaDateTime
		{
			get
			{
				return DateTime.Parse(fecha);
			}
		}
		public List<Producto> productos { get; set; }
		public List<DetallePedido> detalles { get; set; }

		public int? status { get; set; }
		public int? total { get; set; }
		public Repartidor repartidor { get; set; }

		public int id { get; set; }
		public int cliente_id { get; set; }
		public int? conductor_id { get; set; }

		public string created_at { get; set; }
		public string updated_at { get; set; }
		public string deleted_at { get; set; }

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
						str += string.Join(Environment.NewLine, detalles.GetRange(0, detalles.Count).Select(x => string.Format(Environment.NewLine + "-({0}) {1} {2}", x.cantidad, x.producto.nombre, GetStringContenido(x.producto.contenido))));
						//str += string.Format(Environment.NewLine + "-{0}", detalles.Last().producto.nombre);
					}
					else if (detalles.Count > 0)
						str = string.Format(Environment.NewLine + "-({0}) {1} {2}", detalles.Last().cantidad, detalles.Last().producto.nombre, detalles.Last().producto.contenido);

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

				double n = detalles.Sum(x => x.producto.precio);

				return n;
			}
			set
			{

			}
		}


	}

	public class DetallePedido
	{
		public int id { get; set; }
		public int producto_id { get; set; }
		public int precio { get; set; }
		public int cantidad { get; set; }
		public int total { get { return cantidad * precio; } }
		public Producto producto { get; set; }

	}
}
