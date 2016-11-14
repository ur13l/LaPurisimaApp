using System;
using System.Collections.Generic;
using Realms;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			RealmConfiguration realmConfiguration = RealmConfiguration.DefaultConfiguration;

			try
			{
				var x = Realm.GetInstance();
			}
			catch (RealmMigrationNeededException e)
			{
				try
				{
					Realm.DeleteRealm(realmConfiguration);
					//Realm file has been deleted.
					var y = Realm.GetInstance(realmConfiguration);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}




			MainPage = new NavigationPage(new LoginPage());
			//MainPage = new NavigationPage(new OrdersPage());

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
			if (PropertiesManager.GetUserInfo() != null)
			{
				var token = PropertiesManager.GetUserInfo().api_token;
				System.Diagnostics.Debug.WriteLine(token);
			}
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

