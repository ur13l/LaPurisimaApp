using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace LaPurisima
{
	public class Localize
	{
		static readonly CultureInfo ci;

		static Localize()
		{
			ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
		}

		public static string GetString(string key, string comment)
		{
			ResourceManager temp = new ResourceManager("LaPurisima.Resources.Resources", typeof(Localize).GetTypeInfo().Assembly);

			string result = temp.GetString(key, ci);

			return result;
		}
	}
}

