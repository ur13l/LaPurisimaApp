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
			var jsonResponse = await PostObject<User, User>(new User()
			{
				email = mail,
				password = pass,
			}, WEB_METHODS.Autenticate);

			if (jsonResponse == null){
				return null;
			}

			return jsonResponse;
		}


		public static async Task<string> ForgotEmail(string mail)
		{
			var jsonResponse = await PostObject<User, User>(new User()
			{
				email = mail,
			}, WEB_METHODS.Forgot);

			if (jsonResponse == null)
			{
				return null;
			}

			return jsonResponse;
		}





		public static async Task<string> PostObject<T,K>(object item, WEB_METHODS method)
		{
			try
			{
				var client = GetHttpClient();
				var json = JsonConvert.SerializeObject((T)item);
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

