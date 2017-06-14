using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LaPurisima;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace LaPurisima
{
	public class LocationHelper
	{

		public Position CurrentPosition = null;
		public IGeolocator Geolocator;

		static LocationHelper _instance;
		public static LocationHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LocationHelper();
				}

				return _instance;
			}
		}

		private LocationHelper()
		{
			SetupGeoLocator();
		}

		void SetupGeoLocator()
		{
			if (Geolocator != null)
				return;
			Geolocator = CrossGeolocator.Current;
			Geolocator.DesiredAccuracy = 100;

			Geolocator.PositionChanged += (sender, e) =>
			{
				CurrentPosition = e.Position;
				System.Diagnostics.Debug.WriteLine("PositionChanged {0} {1}", e.Position.Latitude, e.Position.Longitude);
			};

			Geolocator.StartListeningAsync(50, 50);
		}


		const string strGeoCodingUrl = "http://maps.googleapis.com/maps/api/geocode/json?language=es&latlng=";


		public static async Task<GoogleMapsLocation> GetLocation(double latitue, double longitude)
		{
			var list = new List<string>();

			var strResult = await fnDownloadString(strGeoCodingUrl + latitue + "," + longitude);
			if (strResult == "Exception")
			{
				return null;
			}
			return JsonConvert.DeserializeObject<GoogleMapsLocation>(strResult);
		}


		static async Task<string> fnDownloadString(string strUri)
		{
			var client = new HttpClient();
			string strResultData;
			try
			{
				strResultData = await client.GetStringAsync(new Uri(strUri));
				System.Diagnostics.Debug.WriteLine(strResultData);
			}
			catch
			{
				strResultData = "Exception";
			}
			finally
			{
				client.Dispose();
				client = null;
			}

			return strResultData;
		}




	}
}
