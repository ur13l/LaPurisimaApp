using System;
using Xamarin.Forms;
using UIKit;
using LaPurisima.iOS;
using BigTed;

[assembly: Dependency (typeof(ProgressView))]
namespace LaPurisima.iOS
{
	public class ProgressView : IProgress
	{
		bool show = false;

		public void ShowProgress(string text)
		{
			if (!show)
			{
				show = true;
				BTProgressHUD.ForceiOS6LookAndFeel = true;
				BTProgressHUD.Show(text, -1, ProgressHUD.MaskType.Black);
			}
		}

		public void Dismiss()
		{
			show = false;
			BTProgressHUD.Dismiss();
		}

		public void ShowProgress(IProgressType type)
		{
			if (type == IProgressType.Done)
			{
				BTProgressHUD.ShowSuccessWithStatus(LaPurisima.Localize.GetString("DoneLabel"),1000);
			}
		}
	}
}