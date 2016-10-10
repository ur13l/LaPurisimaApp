using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

			if (jsonResponse == null){
				return null;
			}

			return jsonResponse;
		}


		public static async Task<string> AskOrder(Pedido _pedido)
		{
			var jsonResponse = await PostObject<Pedido>(new Pedido()
			{
				api_token = _pedido.api_token,
				latitud = _pedido.latitud,
				longitud = _pedido.longitud,
				direccion = _pedido.direccion,
				detalles = _pedido.detalles,
			}, WEB_METHODS.Ask);

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
			}, WEB_METHODS.Forgot);

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
			}, WEB_METHODS.Update,true);

			if (jsonResponse == null)
			{
				return null;
			}
			return jsonResponse;
		}

		public static async Task<string> PostObject<T>(object item, WEB_METHODS method,bool igonoreIfnull = false)
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

				if (response.IsSuccessStatusCode)
				{
					var resultString = await response.Content.ReadAsStringAsync();
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

		public static bool IsError(string json)
		{
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



		public class Response
		{
			public bool success { get; set; }
			public List<string> error { get; set; }
		}
	}
}

