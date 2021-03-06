﻿using System;
using System.Collections.Generic;
using Realms;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class App : Application
	{

		public static App CurrentApp { get; set; }

		public App()
		{
			CurrentApp = this;
			InitializeComponent();

			RealmConfiguration realmConfiguration = RealmConfiguration.DefaultConfiguration;

			try
			{
				var x = Realm.GetInstance();
			}
			catch (Exception e)
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

			//MainPage = new TestPage();
			//return;

			if (PropertiesManager.IsLogedIn())
			{
				MainPage = new RootPage();
			}
			else {

				MainPage = new ShadowNavigationPage(new LoginPage());
				//MainPage = new NavigationPage(new OrdersPage());
			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
			UpdateUserStatus();
			//Test();
		}

		public async void UpdateUserStatus()
		{
			var user = PropertiesManager.GetUserInfo();
			if (user != null)
			{
				if (user.tipo_usuario_id == 2)
				{

					RepartidorHelper.SetEstatusRepartidor(online: true, updateEstatusAndLocationWeb: true);

					LocationHelper.Instance.Geolocator.PositionChanged += (sender, e) =>
					{
						try
						{

							user = PropertiesManager.GetUserInfo();


							Realm.GetInstance().Write(() =>
							{
								//user.status = 1; //activo la que tenga
								user.latitud = e.Position.Latitude;
								user.longitud = e.Position.Longitude;
							});


							RepartidorHelper.UpdateLocationStatusRepartidor(user);

							//var res = await ClientLaPurisima.PostObject<User>(user, WEB_METHODS.SetStatusRepartidor);
							//System.Diagnostics.Debug.WriteLine("RES UPDATE STATUS REPARIDOR: " + res);
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine("error updating status. " + ex.Message);
						}
					};
				}
				else {
					var x = LocationHelper.Instance.Geolocator;
				}
			}
		}

		async void Test()
		{
			//var user = await ClientLaPurisima.LoginUser("ur13l.infante@gmail.com", "123asdZXC");
			//UpdateHelper.UpdateInfo();
			//if (PropertiesManager.GetUserInfo() != null)
			//{
			//	var token = PropertiesManager.GetUserInfo().api_token;
			//	System.Diagnostics.Debug.WriteLine(token);
			//}
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