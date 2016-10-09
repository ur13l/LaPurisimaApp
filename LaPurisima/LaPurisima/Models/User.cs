using System;
namespace LaPurisima
{

	public class User
	{
		public int? id { get; set; }
		public string nombre { get; set; }
		public string email { get; set; }
		public string calle { get; set; }
		public string colonia { get; set; }
		public string codigo_postal { get; set; }
		public string referencia { get; set; }
		public string imagen_usuario { get; set; }
		public int? tipo_usuario_id { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
		public string api_token { get; set; }

		public string password { get; set; }
	}
}

