using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaPurisima
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();

			NavigationPage.SetHasNavigationBar(this, false);

			NavigationPage.SetBackButtonTitle(this, "Atrás");
		}

		async void ForgotPassClicked(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new RecoverPassPage());
		}

		async void LoginClicked(object sender, System.EventArgs e)
		{
			await Navigation.PushModalAsync(new RootPage());
		}
	}
}