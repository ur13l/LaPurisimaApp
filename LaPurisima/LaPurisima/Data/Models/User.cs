using System;
using System.IO;
using Newtonsoft.Json;

namespace LaPurisima
{

	public class User : Realms.RealmObject
	{
		public int? id { get; set; }
		public string nombre { get; set; }
		public string email { get; set; }
		public string calle { get; set; }
		public string colonia { get; set; }
		public string codigo_postal { get; set; }
		public string referencia { get; set; }
		public string imagen_usuario { get; set; }
		public string telefono { get; set; }
		public string telefono_casa { get; set; }
		public int? tipo_usuario_id { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
		public string api_token { get; set; }

		public string password { get; set; }

		public int status { get; set; }
		public double latitud { get; set; }
		public double longitud { get; set; }


		[Realms.Ignored]
		[JsonIgnore]
		public Xamarin.Forms.ImageSource Image
		{
			get
			{
				if (imagen_usuario != null)
				{
					return Xamarin.Forms.ImageSource.FromStream(
						() => new MemoryStream(Convert.FromBase64String(imagen_usuario)));
				}
				return null;
			}
		}
	}
}

