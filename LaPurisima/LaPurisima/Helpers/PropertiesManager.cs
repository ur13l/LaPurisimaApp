using System;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LaPurisima
{
	public class PropertiesManager
	{

		public static bool IsLogedIn()
		{
			//if (Application.Current.Properties.ContainsKey(USER_INFO_KEY))
			//{
			//	return true;
			//}
			//else {
			//	return false;
			//}

			var realm = Realms.Realm.GetInstance();

			var n = realm.All<User>().ToList();
			if (n.Count > 0)
			{
				return true;
			}


			return false;
		}

		public static async void SaveUserInfo(User user)
		{
			//Application.Current.Properties[USER_INFO_KEY] = JsonConvert.SerializeObject(user);
			//await Application.Current.SavePropertiesAsync();
			var realm = Realms.Realm.GetInstance();

			realm.Write(() =>
			{
				var n = realm.All<User>().ToList();
				if (n.Count > 0)
				{
					var x = n.Last();
					x.api_token = user.api_token;
					x.calle = user.calle;
					x.codigo_postal = user.codigo_postal;
					x.colonia = user.colonia;
					x.email = user.email;
					x.id = user.id;
					x.imagen_usuario = user.imagen_usuario;
					x.latitud = user.latitud;
					x.longitud = user.longitud;
					x.nombre = user.nombre;
					x.password = user.password;
					x.status = user.status;
					x.telefono = user.telefono;
					x.telefono_casa = user.telefono_casa;
					x.tipo_usuario_id = user.tipo_usuario_id;

				}
				else {
					var x = realm.CreateObject<User>();
					x.api_token = user.api_token;
					x.calle = user.calle;
					x.codigo_postal = user.codigo_postal;
					x.colonia = user.colonia;
					x.email = user.email;
					x.id = user.id;
					x.imagen_usuario = user.imagen_usuario;
					x.latitud = user.latitud;
					x.longitud = user.longitud;
					x.nombre = user.nombre;
					x.password = user.password;
					x.status = user.status;
					x.telefono = user.telefono;
					x.telefono_casa = user.telefono_casa;
					x.tipo_usuario_id = user.tipo_usuario_id;
				}
			});
		}

		public static User GetUserInfo()
		{
			//if (IsLogedIn())
			//{
			//	var userJson = Application.Current.Properties[USER_INFO_KEY].ToString();
			//	return JsonConvert.DeserializeObject<Usuario>(userJson);
			//}
			var realm = Realms.Realm.GetInstance();

			var n = realm.All<User>().ToList();
			if (n.Count > 0)
				return n.Last();

			return null;
		}


		public static async void LogOut()
		{
			//Application.Current.Properties.Remove(USER_INFO_KEY);
			//await Application.Current.SavePropertiesAsync();

			var realm = Realms.Realm.GetInstance();

			realm.Write(() =>
			{
				realm.RemoveAll<User>();
				//n.Last().IsLoggedIn = false;
			});


		}
	}
}