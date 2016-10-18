using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class LoginPage : BasePage
	{
		public LoginPage()
		{
			InitializeComponent();

			NavigationPage.SetHasNavigationBar(this, false);

			NavigationPage.SetBackButtonTitle(this, "Atrás"); 

			if (PropertiesManager.IsLogedIn())
			{
				Navigation.PushModalAsync(new RootPage());
			} 
		}

		public LoginPage(string email)
		{
			InitializeComponent();
			EntryEmail.Text = email;

		}

		async void SignUpClicked(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new UserSignUp());
		}

		async void ForgotPassClicked(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new RecoverPassPage());
		}

		bool clicked = false;

		async void LoginClicked(object sender, System.EventArgs e)
		{
			if (clicked)
				return;
			clicked = true;

			if (ValidateUI())
			{
				ShowProgress("Validando");
				var response = await ClientLaPurisima.LoginUser(EntryEmail.Text, EntryPass.Text);
				
				if (ValidateResponse(response))
				{
					var user = JsonConvert.DeserializeObject<User>(response);
					PropertiesManager.SaveUserInfo(user);
					ShowProgress(IProgressType.LogedIn);
					await Task.Delay(600);
					HideProgress();
					await Navigation.PushModalAsync(new RootPage());
				}

				HideProgress();
			}

			clicked = false;

		}

		bool ValidateResponse(string response)
		{
			if (ClientLaPurisima.IsError(response))
			{
				DisplayAlert(Localize.GetString("ErrorTitleText", ""), Localize.GetString("VerifyMailPassLabel", ""), Localize.GetString("OkButtonLabel", ""));
				return false;
			}
			else {
				return true;
			}
		}

		bool ValidateUI()
		{
			if (string.IsNullOrEmpty(EntryEmail.Text) || !Regex.IsMatch(EntryEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
			{
				DisplayAlert("Error",Localize.GetString("VerifyMailLabel", ""), Localize.GetString("OkButtonLabel", ""));

				return false;
			}


			if (string.IsNullOrEmpty(EntryPass.Text))
			{
				DisplayAlert("Error", Localize.GetString("EnterPassLabel", ""),Localize.GetString("OkButtonLabel", ""));
				return false;
			}

			return true;
		}
}
}