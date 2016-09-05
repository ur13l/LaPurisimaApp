using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class BasePage : ContentPage
	{
		public Action<string> ShowProgress;
		public Action HideProgress;
	}
}

