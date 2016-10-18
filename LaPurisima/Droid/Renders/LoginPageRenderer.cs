using System;
using Android.App;
using AndroidHUD;
using LaPurisima;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BasePage), typeof(LaPurisima.Droid.LoginPageRenderer))]
namespace LaPurisima.Droid
{
	public class LoginPageRenderer : PageRenderer
	{ 

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			var page = e.NewElement as BasePage;

			// this is a ViewGroup - so should be able to load an AXML file and FindView<>
			var activity = this.Context as Activity;

			page.ShowProgressMessage = (x) =>
			{
				AndHUD.Shared.Show(activity, x,-1,MaskType.Black);
			};

			page.HideProgressAction = () =>
			{
				AndHUD.Shared.Dismiss();

			};

			page.ShowProgressType = (t) =>
			{
				if (t == IProgressType.Done)
				{
					AndHUD.Shared.ShowSuccess(activity, LaPurisima.Localize.GetString("DoneLabel"), MaskType.Black);
					AndHUD.Shared.CurrentDialog.SetCancelable(true);
				}

				if (t == IProgressType.LogedIn)
				{
					AndHUD.Shared.ShowSuccess(activity, LaPurisima.Localize.GetString("LogedInLabel"), MaskType.Black);
					AndHUD.Shared.CurrentDialog.SetCancelable(true);
				}
			};
		}
	}
}

