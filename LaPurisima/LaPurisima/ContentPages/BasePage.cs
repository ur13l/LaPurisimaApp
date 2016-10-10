using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class BasePage : ContentPage
	{
		public Action<string> ShowProgress;
		public Action HideProgress;

		public Action<ContentPage> NextPage;
		public Action<ContentPage> PreviousPage;
		public Action<int> GoToPage;
	}
}

