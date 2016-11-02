using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class BasePage : ContentPage
	{
		public Action<string> ShowProgressMessage;
		public Action<IProgressType> ShowProgressType;
		public Action HideProgressAction;

		public Action<ContentPage> NextPage;
		public Action<ContentPage> PreviousPage;
		public Action<int> GoToPage;

		IProgress progressDependency;

		protected BasePage()
		{
			var progressDependency = DependencyService.Get<IProgress>();
		}

		public void ShowProgress(string message)
		{
			if (progressDependency != null)
				progressDependency.ShowProgress(message);
			if (ShowProgressMessage != null)
				ShowProgressMessage(message);
		}

		public void ShowProgress(IProgressType type)
		{
			progressDependency = DependencyService.Get<IProgress>();
			if (progressDependency != null)
				progressDependency.ShowProgress(type);
			if (ShowProgressType != null)
				ShowProgressType(type);
		}

		public void HideProgress()
		{
			if (progressDependency != null)
				progressDependency.Dismiss();
			if (HideProgressAction != null)
				HideProgressAction();
		}
	}
}

