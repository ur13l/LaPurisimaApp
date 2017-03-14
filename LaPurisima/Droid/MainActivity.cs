using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;

namespace LaPurisima.Droid
{
	[Activity(Label = "LaPurisima.Droid", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}

		private Action<int, Android.App.Result, Intent> _resultCallback;

		public void StartActivity(Intent intent, Action<int, Android.App.Result, Intent> resultCallback)
		{
			_resultCallback = resultCallback;

			StartActivityForResult(intent, 0);
		}

		protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (_resultCallback != null)
			{
				_resultCallback(requestCode, resultCode, data);
				_resultCallback = null;
			}
		} 

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}