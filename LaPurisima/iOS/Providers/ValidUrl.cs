using System;
using UIKit;
using Foundation;
using Xamarin.Forms;
using LaPurisima.iOS;

[assembly: Dependency(typeof(ValidUrl))]
namespace LaPurisima.iOS
{
	public class ValidUrl : IValidUrl
	{
		public bool Valid(string url)
		{
			var valid = UIApplication.SharedApplication.CanOpenUrl(new NSUrl(url));
			return valid;
		}
	}
}