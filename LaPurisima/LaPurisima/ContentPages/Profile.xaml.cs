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


			ImageSourceChanged = () =>
			{
				if (LastView is FFImageLoading.Forms.CachedImage)
					(LastView as FFImageLoading.Forms.CachedImage).Source = Source;

				_imageView.Source = Source;

				//await PostLastFoto();
			};



			Title = "Perfil";
		}


		async void ChangePicture(object sender, EventArgs e)
		{
			if(SaveButton.IsVisible)
				TakePictureActionSheet(_imageView);
		}

		void InitiViews()
		{
			var user = PropertiesManager.GetUserInfo();
			if (user != null)
			{
				EntryNameProfile.Text = user.nombre;
				EntryCalleProfile.Text = user.calle;
				EntryColoniaProfile.Text = user.colonia;
				if (user.referencia == null)
				{
					EntryBetweenProfile.Text = "";
				}
				else {
					EntryBetweenProfile.Text = user.referencia;
				}
				EntryCPProfile.Text = user.codigo_postal;

				if(user.imagen_usuario!=null)
					_imageView.Source = user.Image;
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

			if (Base64Image != null)
				user.imagen_usuario = Base64Image;


			ToolbarItems.Add(EditButton);

			ShowProgress("Validando");
			var response = await ClientLaPurisima.UpdateUser(user);
			if (ValidateResponse(response))
			{
				//await DisplayAlert("Error", Localize.GetString("ErrorMessageDoesntExist", ""), "ok");
				PropertiesManager.SaveUserInfo(user);
			}


			ShowProgress(IProgressType.Done);
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