using System;
using System.Collections.Generic;

namespace LaPurisima
{

	public class Pedido
	{
		public string api_token { get; set; }
		public string latitud { get; set; }
		public string longitud { get; set; }
		public string direccion { get; set; }
		public List<Producto> detalles { get; set; }
	}
}
