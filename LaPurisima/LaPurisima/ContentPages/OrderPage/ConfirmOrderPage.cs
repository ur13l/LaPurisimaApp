using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaPurisima
{
	public partial class ConfirmOrderPage : BasePage
	{
		Pedido _pedido;


		async void Pedir(object sender, System.EventArgs e)
		{
			var progressDependency = DependencyService.Get<IProgress>();
			if (progressDependency != null)
				progressDependency.ShowProgress(Localize.GetString("LoadingText", ""));
			if (ShowProgress != null)
				ShowProgress("Validando");
			var response = await ClientLaPurisima.AskOrder(_pedido);
			DisplayAlert(Localize.GetString("ErrorTitleText", ""), Localize.GetString("VerifyMailPassLabel", ""), Localize.GetString("OkButtonLabel", ""));
			if (progressDependency != null)
				progressDependency.Dismiss();
			if (HideProgress != null)
				HideProgress();

		}




		public ConfirmOrderPage(Pedido pedido = null)
		{
			InitializeComponent();
			EntryNameProfile.IsEnabled = false;
			EntryCalleProfile.IsEnabled = false;
			EntryColoniaProfile.IsEnabled = false;
			var user = PropertiesManager.GetUserInfo();
			EntryNameProfile.Text = user.nombre;
			EntryCalleProfile.Text = user.calle;
			EntryColoniaProfile.Text = user.colonia;
			if (pedido == null)
				_pedido = new Pedido()
				{
					api_token = user.api_token,
					longitud = "100.22",
					latitud = "-2.242",
					direccion = "conocida",
					detalles = new List<Producto>()
				{
					new Producto(){
						nombre = "Garrafón Grande",
						cantidad = 2,
						precio = 80,
					},
					new Producto(){
						nombre = "Garrafón Chico",
						cantidad = 1,
						precio = 10,
					},
				},
				};
			ListView.ItemsSource = _pedido.detalles;
		}
	}
}



