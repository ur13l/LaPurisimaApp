using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using Realms;
using UIKit;

namespace LaPurisima.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			Xamarin.FormsMaps.Init();

			FFImageLoading.Forms.Touch.CachedImageRenderer.Init();
			var dummy = new FFImageLoading.Forms.Touch.CachedImageRenderer();
			var ignore = new FFImageLoading.Transformations.CircleTransformation();

			KeyboardOverlapRenderer.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}

