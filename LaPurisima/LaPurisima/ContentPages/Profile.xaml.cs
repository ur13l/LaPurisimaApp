using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaPurisima
{
	public partial class Profile : ContentPage
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
				//EntryNameProfile.Text = PropertiesManager.GetUserInfo().;
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

		void SaveInfo(object sender, System.EventArgs e)
		{
			EditClicked(null, null);

			ToolbarItems.Add(EditButton);
		}
	}
}

