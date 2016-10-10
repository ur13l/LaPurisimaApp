using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();


			MainPage = new NavigationPage(new LoginPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts

			Test();
		}

		async void Test()
		{
			//var user = await ClientLaPurisima.LoginUser("ur13l.infante@gmail.com", "123asdZXC");
			UpdateHelper.UpdateInfo();
			var token = PropertiesManager.GetUserInfo().api_token;
			System.Diagnostics.Debug.WriteLine(token);
		}	

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

