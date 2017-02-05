using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using UniversalImageLoader.Core;

namespace LaPurisima.Droid
{
	[Application]
	public class LaPurisimaApp : Application, Application.IActivityLifecycleCallbacks
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

			RegisterActivityLifecycleCallbacks(this);
		}

		public override void OnTerminate()
		{
			base.OnTerminate();
			UnregisterActivityLifecycleCallbacks(this);
		}


		public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivityDestroyed(Activity activity)
		{

		}

		public void OnActivityPaused(Activity activity)
		{

		}

		public void OnActivityResumed(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{

		}

		public void OnActivityStarted(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivityStopped(Activity activity)
		{

		}


	}
}
