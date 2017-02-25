using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class LaunchMapApp
	{
		public static void Open(Geocode geocode, TypeMap typeMap)
		{
			string request = "";

			if (String.IsNullOrEmpty(geocode.Name))
			{
				string latitude = geocode.Latitude.Replace(",", ".");
				string longitude = geocode.Longitude.Replace(",", ".");

				if (typeMap == TypeMap.Apple)
				{
					request = "maps://?saddr=&daddr=" + string.Format("{0},{1}", latitude, longitude) + @"&directionsmode=drive";
				}
				else if (typeMap == TypeMap.Google)
				{
					request = "comgooglemaps://?saddr=&daddr=" + string.Format("{0},{1}", latitude, longitude) + @"&directionsmode=drive";
				}
				else if (typeMap == TypeMap.Waze)
				{
					request = "waze://?ll=" + string.Format("{0},{1}", latitude, longitude) + @"&navigate=yes";
				}
			}
			else
			{
				var name = geocode.Name.Replace("&", "and");

				if (typeMap == TypeMap.Apple)
				{
					name = name.Replace(' ', '+');
					request = "maps://?daddr=" + name + "&directionsmode=drive";
				}
				else if (typeMap == TypeMap.Google)
				{
					name = name.Replace(' ', '+');
					request = "comgooglemaps://?daddr=" + name + "&directionsmode=drive";
				}
				else if (typeMap == TypeMap.Waze)
				{
					name = name.Replace(' ', '%');
					request = "waze://?q=" + name;
				}
			}

			Device.OpenUri(new Uri(request));
		}
	}

	public enum TypeMap
	{
		Apple,
		Google,
		Waze
	}

	public class Geocode
	{
		public string Name { get; set; }

		public string Latitude { get; set; }

		public string Longitude { get; set; }
	}
}
