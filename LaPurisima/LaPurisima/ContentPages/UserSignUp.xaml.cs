using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class UserSignUp : ContentPage
	{
		public UserSignUp()
		{
			InitializeComponent();

			//System.Diagnostics.Debug.WriteLine(Localize.GetString("NewUserText", ""));
		}

		async void SignUpClicked(object sender, System.EventArgs e)
		{
			if (await Validate())
			{

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


			//mail
			if (ValidateEntry(EntryEmailAlta, "VerifyMailLabel", @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
			{
				return false;
			}


			//pass
			if (!ValidateEntry(EntryPassAlta, "EnterPassLabel"))
			{
				return false;
			}

			//confirm pass
			if (!ValidateEntry(EntryPassConfAlta, "ConfirmPassLabel"))
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

			if (!ValidateEntry(EntryPassConfAlta, "ConfirmPassLabel"))
			{
				return false;
			}

			if (!ValidateEntry(EntryCalleAlta, "EnterStreetLabel"))
			{
				return false;
			}

			if (!ValidateEntry(EntryColoniaAlta, "EnterColonyLabel"))
			{
				return false;
			}

			return true;
		}

		bool ValidateEntry(Entry entry, string label, string regex = null, RegexOptions options = RegexOptions.IgnoreCase)
		{
			bool valid = true;
			if (string.IsNullOrEmpty(entry.Text))
			{
				valid = false;
				return false;
			}

			if (!string.IsNullOrEmpty(regex))
			{
				valid = !Regex.IsMatch(entry.Text, regex, options);
			}

			if (!valid)
			{
				entry.Focus();
				ShowErrorMessage(label);
			}

			return valid;
		}

		public async void ShowErrorMessage(string label)
		{
			await DisplayAlert("Error", Localize.GetString(label, ""), Localize.GetString("OkButtonLabel", ""));
		}

		//https://maps.googleapis.com/maps/api/geocode/json?language=es&latlng=21.1165064,-101.6781424

	}
}

