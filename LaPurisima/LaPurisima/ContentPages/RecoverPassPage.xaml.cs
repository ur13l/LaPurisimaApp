using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class RecoverPassPage : BasePage
	{
		public RecoverPassPage()
		{
			InitializeComponent();
		}



		async void ResetClicked(object sender, System.EventArgs e)
		{
			if (ValidateUI())
			{
				var progressDependency = DependencyService.Get<IProgress>();
				if (progressDependency != null)
					progressDependency.ShowProgress("Validando");
				if (ShowProgressMessage != null)
					ShowProgressMessage("Validando");
				var response = await ClientLaPurisima.ForgotEmail(EntryEmail.Text);
				if (ValidateResponse(response))
				{
					//var user = JsonConvert.DeserializeObject<User>(response);
					await Navigation.PushModalAsync(new LoginPage(EntryEmail.Text));
				}

				if (progressDependency != null)
					progressDependency.Dismiss();
				if (HideProgressAction != null)
					HideProgressAction();
			}

		}


		bool ValidateUI()
		{
			if (string.IsNullOrEmpty(EntryEmail.Text) || !Regex.IsMatch(EntryEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
			{
				ShowErrorMessage("ErrorMessageEmail");

				return false;
			}

			return true;
		}


		public async void ShowErrorMessage(string label)
		{
			await DisplayAlert("Error", Localize.GetString(label, ""),"ok");
		}



		bool ValidateResponse(string response)
		{
			if (ClientLaPurisima.IsErrorFalse(response))
			{
				ShowErrorMessage("ErrorMessageDoesntExist");
				return false;
			}
			else {
				return true;
			}
		}
	}
}


