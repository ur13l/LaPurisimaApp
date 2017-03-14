using System;
using System.Globalization;
using Xamarin.Forms;

namespace LaPurisima
{
	public interface IValidUrl
	{
		bool Valid(string url);
	}
}