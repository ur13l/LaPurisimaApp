using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace LaPurisima
{
	public partial class Profile : BasePage
	{
		public Profile()
		{
			InitializeComponent();

			InitiViews();

			Title = "Perfil";
		}

		void InitiViews()
		{
			if (PropertiesManager.GetUserInfo() != null)
			{
				EntryNameProfile.Text = PropertiesManager.GetUserInfo().nombre;
				EntryCalleProfile.Text = PropertiesManager.GetUserInfo().calle;
				EntryColoniaProfile.Text = PropertiesManager.GetUserInfo().colonia;
				if (PropertiesManager.GetUserInfo().referencia == null)
				{
					EntryBetweenProfile.Text = "";
				}
				else {
					EntryBetweenProfile.Text = PropertiesManager.GetUserInfo().referencia;
				}
				EntryCPProfile.Text = PropertiesManager.GetUserInfo().codigo_postal;
			}

			EntryNameProfile.IsEnabled = false;
			EntryCalleProfile.IsEnabled = false;
			EntryColoniaProfile.IsEnabled = false;
			EntryBetweenProfile.IsEnabled = false;
			EntryCPProfile.IsEnabled =false;
		}


		void EditClicked(object sender, System.EventArgs e)
		{
			EntryNameProfile.IsEnabled = !EntryNameProfile.IsEnabled;
			EntryCalleProfile.IsEnabled = !EntryCalleProfile.IsEnabled;
			EntryColoniaProfile.IsEnabled = !EntryColoniaProfile.IsEnabled;
			EntryBetweenProfile.IsEnabled = !EntryBetweenProfile.IsEnabled;
			EntryCPProfile.IsEnabled = !EntryCPProfile.IsEnabled;

			SaveButton.IsVisible = !SaveButton.IsVisible;

			ToolbarItems.Remove(EditButton);
		}

		async void SaveInfo(object sender, System.EventArgs e)
		{
			EditClicked(null, null);

			var user = PropertiesManager.GetUserInfo();
			user.nombre = EntryNameProfile.Text;
			user.calle = EntryCalleProfile.Text;
			user.colonia = EntryColoniaProfile.Text;
			user.referencia = EntryBetweenProfile.Text;
			user.codigo_postal = EntryCPProfile.Text;



			ToolbarItems.Add(EditButton);
			var progressDependency = DependencyService.Get<IProgress>();
			if (progressDependency != null)
				progressDependency.ShowProgress("Validando");
			if (ShowProgress != null)
				ShowProgress("Validando");
			var response = await ClientLaPurisima.UpdateUser(user);
			if (ValidateResponse(response))
			{
				await DisplayAlert("Error", Localize.GetString("ErrorMessageDoesntExist", ""), "ok");
				PropertiesManager.SaveUserInfo(user);
			}

			if (progressDependency != null)
				progressDependency.Dismiss();
			if (HideProgress != null)
				HideProgress();
			
		}


		public async void ShowErrorMessage(string label)
		{
			await DisplayAlert("Error", Localize.GetString(label, ""), "ok");
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

