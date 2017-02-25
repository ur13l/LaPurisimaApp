using System;
using Xamarin.Forms;
using LaPurisima.Droid;
using Android.Content.PM;
using Android.Content;

[assembly: Dependency(typeof(ValidUrl))]
namespace LaPurisima.Droid
{
	public class ValidUrl : IValidUrl
	{
		public bool Valid(string url)
		{
			Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));

			var canOpenUrl = false;
			PackageManager packageManager = Forms.Context.PackageManager;
			var resolvedActivities = packageManager.QueryIntentServices(intent, 0);
			if (resolvedActivities.Count > 0)
				canOpenUrl = true;
			return canOpenUrl;
		}
	}
}