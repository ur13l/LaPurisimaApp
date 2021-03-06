﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class UserSignUp : BasePage
	{
		public UserSignUp()
		{
			InitializeComponent();

			//System.Diagnostics.Debug.WriteLine(Localize.GetString("NewUserText", ""));
			NavigationPage.SetBackButtonTitle(this, "Registro");
		}

		async void SignUpClicked(object sender, System.EventArgs e)
		{

			if (ChooseAddresPage.LastPosition == null)
			{
				await DisplayAlert("","Elige tu ubicación","OK");
				return;
			}

			Xamarin.Forms.Maps.Position position = (Xamarin.Forms.Maps.Position)ChooseAddresPage.LastPosition;

			if (await Validate())
			{
				
				var user = new User()
				{
					calle = EntryCalleAlta.Text,
					colonia = EntryColoniaAlta.Text,
					codigo_postal = EntryCPAlta.Text,
					email = EntryEmailAlta.Text,
					created_at = DateTime.Now.ToString("yy-MMM-dd ddd"),
					telefono = EntryTelAlta.Text,
					nombre = EntryNameAlta.Text,
					password = EntryPassAlta.Text,
					referencia = string.Format("{0};{1},{2}",Referencias.Text,position.Latitude,position.Longitude),
					telefono_casa = EntryTelCasaAlta.Text,
					tipo_usuario_id = 3,
				};

				ShowProgress("Validando");

				var resp = await ClientLaPurisima.PostObject<User>(user, WEB_METHODS.CrearUsuario, true);
				if (ClientLaPurisima.IsGood(resp))
				{
					//await DisplayAlert("", "Usuario Creado Correctamente", "OK");
					//ERROR 
					var response = await ClientLaPurisima.LoginUser(user.email, user.password);
					var userInfo = JsonConvert.DeserializeObject<User>(response); //response es null
					PropertiesManager.SaveUserInfo(userInfo);

					ShowProgress(IProgressType.Done);
					await Task.Delay(1000);
					HideProgress();
					await Navigation.PushModalAsync(new RootPage());
				}
				else {
					HideProgress();
					await DisplayAlert("Error", ClientLaPurisima.GetMessageForError(ClientLaPurisima.GetWebError(resp)), "OK");
				}

			}
			else {

			}


		}

		async Task<bool> Validate()
		{

			//name
			if (!ValidateEntry(EntryNameAlta, "EnterNameLabel"))
			{
				return false;
			}

			if (!ValidateEntry(EntryTelAlta, "EnterTelLabel"))
			{
				return false;
			}


			//mail
			if (!ValidateEntry(EntryEmailAlta, "VerifyMailLabel", @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
			{
				return false;
			}


			//pass
			if (!ValidateEntry(EntryPassAlta, "EnterPassLabel"))
			{
				return false;
			}

			//confirm pass
			if (!ValidateEntry(EntryPassConfAlta, "EnterPassLabel"))
			{
				return false;
			}

			//matching pass
			if (EntryPassAlta.Text != EntryPassConfAlta.Text)
			{
				EntryPassAlta.Focus();
				ShowErrorMessage("EnterMatchingPassLabel");
				return false;
			}

			if (!ValidateText(EntryCalleAlta.Text, "EnterStreetLabel"))
			{
				return false;
			}

			if (!ValidateText(EntryColoniaAlta.Text, "EnterColonyLabel"))
			{
				return false;
			}

			return true;
		}

		bool ValidateText(string text, string label, string regex = null, RegexOptions options = RegexOptions.IgnoreCase)
		{
			bool valid = true;
			if (string.IsNullOrEmpty(text))
			{
				valid = false;
			}

			if (!string.IsNullOrEmpty(regex))
			{
				valid = Regex.IsMatch(text, regex, options);
			}

			if (!valid)
			{
				//entry.Focus();
				ShowErrorMessage(label);
			}

			return valid;
		}

		bool ValidateEntry(Entry entry, string label, string regex = null, RegexOptions options = RegexOptions.IgnoreCase)
		{
			bool valid = true;
			if (string.IsNullOrEmpty(entry.Text))
			{
				valid = false;
			}

			if (!string.IsNullOrEmpty(regex))
			{
				valid = Regex.IsMatch(entry.Text, regex, options);
			}

			if (!valid)
			{
				entry.Focus();
				ShowErrorMessage(label);
			}

			return valid;
		}

		async void PickAddress(object sender, System.EventArgs e)
		{
			var p = new ChooseAddresPage(true);
			p.AddresChoosed += (ss, ee) =>
			{
				EntryCalleAlta.Text = string.Format("{0} # {1}", ChooseAddresPage.street, ChooseAddresPage.streetNumber);
				EntryColoniaAlta.Text = ChooseAddresPage.colony;
				EntryCPAlta.Text = ChooseAddresPage.postalCode;

			};
			await Navigation.PushAsync(p);
		}

		public async void ShowErrorMessage(string label)
		{
			var message = Localize.GetString(label, "");
			await DisplayAlert("Error", message, Localize.GetString("OkButtonLabel", ""));
		}

		//https://maps.googleapis.com/maps/api/geocode/json?language=es&latlng=21.1165064,-101.6781424

	}
}

