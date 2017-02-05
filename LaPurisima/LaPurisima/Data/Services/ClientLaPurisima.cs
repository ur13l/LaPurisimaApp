using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms.Maps;

namespace LaPurisima
{

	public class ClientLaPurisima
	{

		public static HttpClient GetHttpClient()
		{
			HttpClient httpClient = new HttpClient();
			httpClient.Timeout = TimeSpan.FromSeconds(30);
			httpClient.DefaultRequestHeaders.ExpectContinue = true;
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));
			httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("UTF-8"));

			httpClient.BaseAddress = new Uri(Config.URL);

			return httpClient;
		}

		public static async Task<string> LoginUser(string mail, string pass)
		{
			var jsonResponse = await PostObject<User>(new User()
			{
				email = mail,
				password = pass,
			}, WEB_METHODS.Autenticate);

			if (jsonResponse == null)
			{
				return null;
			}

			return jsonResponse;
		}

		public static async Task<string> MakeOrder(Pedido _pedido)
		{
			_pedido.api_token = PropertiesManager.GetUserInfo().api_token;
			_pedido.fecha = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");
			var jsonResponse = await PostObject<Pedido>(_pedido, WEB_METHODS.PedidoNuevo);

			if (jsonResponse == null)
			{
				return null;
			}

			return jsonResponse;
		}


		/*
{
"api_token": "neMdEyQE9xz7SKaRFObZm7fY3bCcNw7TxNvTGokk5abi1SH7Lq8SEfxJOtav",
"latitud": "180",
"longitud": "130",
"direccion": "Conocida",
"detalles": [
	{
		"producto_id": 52,
		"cantidad": 1
	},
	{
		"producto_id": 62,
		"cantidad": 2
	}
]
}*/



		public static async Task<string> ForgotEmail(string mail)
		{
			var jsonResponse = await PostObject<User>(new User()
			{
				email = mail,
			}, WEB_METHODS.PassForgot);

			if (jsonResponse == null)
			{
				return null;
			}

			return jsonResponse;
		}


		public static async Task<string> UpdateUser(User user)
		{
			var jsonResponse = await PostObject<User>(new User()
			{
				id = null,
				nombre = user.nombre,
				calle = user.calle,
				codigo_postal = user.codigo_postal,
				colonia = user.colonia,
				referencia = user.referencia,
				api_token = user.api_token,
				//imagen_usuario = usser.imagen_usuario
			}, WEB_METHODS.UsuarioUpdate, true);

			if (jsonResponse == null)
			{
				return null;
			}
			return jsonResponse;
		}

		public static async Task<List<Pedido>> GetPedidos(User user)
		{
			var jsonResponse = await PostObject<User>(user, WEB_METHODS.GetPedidosUsuario, true);
			var list = new List<Pedido>();
			if (jsonResponse == null)
			{
				return null;
			}
			try
			{
				list = JsonConvert.DeserializeObject<List<Pedido>>(jsonResponse);
			}
			catch (Exception ex)
			{

			}
			return list;
		}

		public static async Task<List<Pedido>> GetPedidosRepartidor(User user)
		{
			var jsonResponse = await PostObject<User>(user, WEB_METHODS.GetPedidosRepartidor);
			var list = new List<Pedido>();
			if (jsonResponse == null)
			{
				return null;
			}
			try
			{

				list = JsonConvert.DeserializeObject<List<Pedido>>(jsonResponse);
			}
			catch (Exception ex)
			{

			}
			return list;
		}

		public static async Task<GoogleMapsLocation> GetAddresForPosition(Position position)
		{
			try
			{

				var resultModel = await GetObject<GoogleMapsLocation>(WEB_METHODS.GetLocationName, false, position.Latitude + "," + position.Longitude);

				return resultModel;
			}
			catch
			{
				return new GoogleMapsLocation();
			}
		}

		public static async Task<string> PostObject<T>(object item, WEB_METHODS method, bool igonoreIfnull = false)
		{
			try
			{
				var client = GetHttpClient();
				string json = null;

				if (igonoreIfnull)
				{

					json = JsonConvert.SerializeObject((T)item,
							Newtonsoft.Json.Formatting.None,
							new JsonSerializerSettings
							{
								NullValueHandling = NullValueHandling.Ignore
							});
				}
				else {
					json = JsonConvert.SerializeObject((T)item);
				}

				HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
				client.BaseAddress = new Uri(Config.URL);
				var response = await client.PostAsync(Config.GetURLForMethod(method), content);
				var resultString = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					
					//var result = JsonConvert.DeserializeObject<K>(resultString); 
					return resultString;
				}
				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public static async Task<T> GetObject<T>(WEB_METHODS method, bool default_url = true, string where = null, bool igonoreIfnull = false)
		{
			try
			{
				var client = GetHttpClient();

				if (default_url)
					client.BaseAddress = new Uri(Config.URL);

				var response = await client.GetAsync(Config.GetURLForMethod(method) + where);

				if (response.IsSuccessStatusCode)
				{
					var resultString = await response.Content.ReadAsStringAsync();


					if (igonoreIfnull)
					{
						return JsonConvert.DeserializeObject<T>(resultString,
								new JsonSerializerSettings
								{
									NullValueHandling = NullValueHandling.Ignore
								});
					}
					else {
						return JsonConvert.DeserializeObject<T>(resultString);
					}
				}
				return default(T);
			}
			catch (Exception ex)
			{
				return default(T);
			}
		}



		#region PARSING

		public static bool IsError(string json)
		{

			if (string.IsNullOrEmpty(json))
				return true;

			if (!json.Contains("success"))
				return false;

			try
			{
				var resp = JsonConvert.DeserializeObject<Response>(json);
				if (resp.success)
					return false;
				else return true;
			}
			catch (Exception ex)
			{
				return true;
			}

			return false;
		}

		public static WEB_ERROR GetWebError(string json)
		{
			if (json == null)
				return WEB_ERROR.ServerError;

			try
			{
				var resp = JsonConvert.DeserializeObject<Response>(json);
				if (resp.success)
				{
					return WEB_ERROR.NoError;
				}
				else {
					if (resp.error.Contains("email.exists"))
						return WEB_ERROR.EmailExists;
					else
						return WEB_ERROR.Error;

				}
			}
			catch (Exception ex)
			{
				return WEB_ERROR.ParseError;
			}
		}

		public static bool IsErrorFalse(string json)
		{
			if (!json.Contains("false"))
			{
				return false;
			}
			else {
				return true;
			}
		}

		public static bool IsGood(string jsonResponse)
		{
			return (GetWebError(jsonResponse) == WEB_ERROR.NoError);
		}

		public static string GetMessageForError(WEB_ERROR error)
		{
			string message = "Error";
			switch (error)
			{
				case WEB_ERROR.EmailExists:
					message = Localize.GetString("ErrorEmailExists");
					break;
				case WEB_ERROR.ServerError:
					message = Localize.GetString("ErrorServerResponse");
					break;
			}

			return message;
		}



		public class Response
		{
			public bool success { get; set; }
			public List<string> error { get; set; }
		}

		#endregion
	}
}

