using System;
using AndroidHUD;
using LaPurisima.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProgressView))]
namespace LaPurisima.Droid
{
	public class ProgressView : IProgress
	{
		public void ShowProgress(string text)
		{
			AndHUD.Shared.Show(Forms.Context, text, -1, MaskType.Black);

		}

		public void Dismiss()
		{
			AndHUD.Shared.Dismiss(Forms.Context);
		}

		public void ShowProgress(IProgressType type)
		{
			if (type == IProgressType.Done)
			{
				AndHUD.Shared.ShowSuccessWithStatus(Forms.Context, LaPurisima.Localize.GetString("DoneLabel"), MaskType.Black, TimeSpan.FromSeconds(1));
			}
			else if (type == IProgressType.LogedIn)
			{
				AndHUD.Shared.ShowSuccess(Forms.Context,null, MaskType.Black, TimeSpan.FromSeconds(1));
			}
		}
	}
}