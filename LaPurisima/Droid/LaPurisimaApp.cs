using System;
using Android.App;
using Android.Runtime;
using UniversalImageLoader.Core;

namespace LaPurisima.Droid
{
	[Application]
	public class LaPurisimaApp : Application
	{
		protected LaPurisimaApp(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}
		public override void OnCreate()
		{
			base.OnCreate();
			// Use default options
			var config = ImageLoaderConfiguration.CreateDefault(ApplicationContext);
			// Initialize ImageLoader with configuration.
			ImageLoader.Instance.Init(config);
		}
	}
}
