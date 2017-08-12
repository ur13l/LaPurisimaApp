using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LaPurisima
{
	public partial class ChooseAddresPage : BasePage
	{

		public event EventHandler AddresChoosed;

		public static string street = null, streetNumber, colony = null, city = null, postalCode = null, state = null, country = null;

		Position START_POINT = new Position(26.9034632, -101.4199217);
		Distance START_DISTANCE = Distance.FromKilometers(0.2);

		bool chooseAddress;

		public ChooseAddresPage(bool onlyChooseAddress = false)
		{
			chooseAddress = onlyChooseAddress;
			InitializeComponent();


			if (chooseAddress)
				Title = "Elige tu dirección";
			else
				Title = "Hacer un pedido";

			//_confirmLocationBtn.Clicked += (sender, e) =>
			//{
			//	_confirmLocationBtn.IsVisible = false;
			//	//Device.BeginInvokeOnMainThread(async () =>
			//	//{
			//	//	await Task.WhenAll(new Task[] { _confirmLocationBtn.FadeTo(0, 250, Easing.SinIn), _confirmLocationBtn.TranslateTo(0, 50) });
			//	//});

			//	//var googleMaps = await ClientLaPurisima.GetAddresForPosition(_lastPosition);
			//	////System.Diagnostics.Debug.WriteLine(googleMaps);
			//	//Traverse(googleMaps);
			//	UpdateView();
			//	SaveInfo(new Position(-1,-1));
			//};


			Map.PropertyChanged += async (sender, e) =>
				{
					//System.Diagnostics.Debug.WriteLine(e.PropertyName);
					if (e.PropertyName == "VisibleRegion")
					{
						//onetime
						UpdatePoints(Map.VisibleRegion.Center);
						//everytime


						Device.BeginInvokeOnMainThread(() =>
						{
							_progress.IsVisible = true;
						});

						var googleMaps = await ClientLaPurisima.GetAddresForPosition(Map.VisibleRegion.Center);
						//System.Diagnostics.Debug.WriteLine(googleMaps);
						Traverse(googleMaps);

						Device.BeginInvokeOnMainThread(() =>
					   {
						   _progress.IsVisible = false;
					   });

						if (chooseAddress)
							_nextBTN.Text = "Confirmar dirección";
						else
							_nextBTN.Text = "Confirmar ubicacion";

						UpdateView();
						//SaveInfo();
					}
				};


			var pref = RealmHelper.GetUsersPref();
			if (pref != null)
			{
				Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(pref.Latitude, pref.Longitude), START_DISTANCE));
			}
			else {
				Map.MoveToRegion(MapSpan.FromCenterAndRadius(START_POINT, START_DISTANCE));
			}



			_street.Completed += (sender, e) =>
		  {
			  SearchByAddress();

		  };

			_number.Completed += (sender, e) =>
		   {

			   SearchByAddress();
		   };

			if (Device.OS == TargetPlatform.Android)
			{
				_location.IsVisible = false;
			}

		}


		bool isFirstTime = true;
		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (isFirstTime)
				GetLocation();

			isFirstTime = false;

			Device.BeginInvokeOnMainThread(() =>
			{
				_nextBTN.Text = "Continuar";
			});
		}

		async void GetLocation()
		{

			var info = PropertiesManager.GetUserInfo();
			if (info != null)
			{

				if (info.referencia != null && info.referencia.Contains(";"))
				{
					try
					{
						var str = info.referencia.Split(';')[1];
						var arr = str.Split(',');
						var lat = double.Parse(arr[0]);
						var lon = double.Parse(arr[1]);
						START_POINT = new Position(lat, lon);

						Map.MoveToRegion(MapSpan.FromCenterAndRadius(START_POINT, START_DISTANCE));

						var response = await ClientLaPurisima.GetAddresForPosition(START_POINT);
						//System.Diagnostics.Debug.WriteLine(googleMaps);
						Traverse(response);

						if (!chooseAddress)
							SaveInfo();

						return;
					}
					catch (Exception ex)
					{

					}
				}
			}


			CenterInUser(null,null);
		}

		async void CenterInUser(object sender, EventArgs args)
		{
			if (LocationHelper.Instance.CurrentPosition != null)
			{
				START_POINT = new Position(LocationHelper.Instance.CurrentPosition.Latitude, LocationHelper.Instance.CurrentPosition.Longitude);
			}
			else {
				var l = await LocationHelper.Instance.Geolocator.GetPositionAsync(10000);
				START_POINT = new Position(l.Latitude, l.Longitude);
			}

			Map.MoveToRegion(MapSpan.FromCenterAndRadius(START_POINT, START_DISTANCE));

			var googleMaps = await ClientLaPurisima.GetAddresForPosition(START_POINT);
			//System.Diagnostics.Debug.WriteLine(googleMaps);
			Traverse(googleMaps);

			if (!chooseAddress)
				SaveInfo();
		}

		async void SearchByAddress()
		{

			try
			{

				var resultModel = await ClientLaPurisima.GetObject<GoogleMapsLocation>(WEB_METHODS.GetAddressCoordenates, false, string.Format("{0}+{1}+,{2},+{3}+{4}", _number.Text, String.Join("+", _street.Text.Split(' ')), _colony.Text, city, state));
				Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(resultModel.results[0].geometry.location.lat, resultModel.results[0].geometry.location.lng), START_DISTANCE));
				Traverse(resultModel);
				UpdateView();
			}
			catch
			{
				_error.IsVisible = true;
				await Task.Delay(300);
				_error.IsVisible = false;
			}


		}

		public static Position? LastPosition;
		async void UpdatePoints(Position position)
		{
			LastPosition = position;
			//if (_confirmLocationBtn.Opacity < 1)
			//{
			//	Device.BeginInvokeOnMainThread(async () =>
			//	{
			//		await Task.WhenAll(new Task[] { _confirmLocationBtn.FadeTo(1, 250, Easing.SinIn), _confirmLocationBtn.TranslateTo(0, 0) });
			//	});

			//}

			//_confirmLocationBtn.IsVisible = true;
		}

		void UpdateView()
		{

			AnimateText(_colony, colony);
			AnimateText(_number, streetNumber);
			AnimateText(_street, street);
		}

		async void AnimateText(Entry label, string newText)
		{
			var color = label.TextColor;
			await Task.WhenAll(
				label.ColorTo(Color.Gray, Color.Black, c => label.TextColor = c, 200),

				label.ColorTo(Color.Black, Color.Gray, c => label.TextColor = c, 200));

			label.Text = newText;
		}

		void SaveInfo()
		{

			if (HelperOrdenPage.Pedido == null && PropertiesManager.GetUserInfo() != null)
				HelperOrdenPage.Pedido = new Pedido()
				{
					api_token = PropertiesManager.GetUserInfo().api_token,
					cliente_id = (int)PropertiesManager.GetUserInfo().id,
					productos = new List<Producto>()
					{

					},
				};

			//if (p.Latitude != -1)
			//{
			//	HelperOrdenPage.Pedido.latitud = p.Latitude;
			//	HelperOrdenPage.Pedido.longitud = p.Longitude;
			//}

			if (Map.VisibleRegion != null && Map.VisibleRegion.Center != null)
			{

				HelperOrdenPage.Pedido.latitud = Map.VisibleRegion.Center.Latitude;
				HelperOrdenPage.Pedido.longitud = Map.VisibleRegion.Center.Longitude;
			}
			//HelperOrdenPage.street = street;
			//HelperOrdenPage.city = city;
			//HelperOrdenPage.streetNumber = streetNumber;
			HelperOrdenPage.street = _street.Text;
			HelperOrdenPage.city = city;
			HelperOrdenPage.streetNumber = _number.Text;
			HelperOrdenPage.country = country;
			HelperOrdenPage.postalcode = postalCode;
			HelperOrdenPage.state = state;
			HelperOrdenPage.colony = _colony.Text;
		}

		void Traverse(GoogleMapsLocation googleMaps)
		{
			if (googleMaps == null || googleMaps.results == null || googleMaps.results.Count == 0)
				return;

			street = null; streetNumber = null; colony = null; city = null; postalCode = null; state = null;
			country = null;

			foreach (var result in googleMaps.results)
			{
				//System.Diagnostics.Debug.WriteLine(string.Format("result: formatted_address: {0}, types: [{1}]", result.formatted_address, String.Join(" ", result.types)));

				int ncomp = 0;
				foreach (var addresComponent in result.address_components)
				{
					//System.Diagnostics.Debug.WriteLine(string.Format(" addresComponent[{0}]: formatted_address: {1}, short_name:{2} types: [{3}]", ncomp, addresComponent.long_name, addresComponent.short_name, String.Join(" ", addresComponent.types)));


					foreach (var item in addresComponent.types)
					{
						//System.Diagnostics.Debug.WriteLine(string.Format("{0} {1} {2} {3}", item, addresComponent.long_name, addresComponent.short_name, "types: " + String.Join("; ", addresComponent.types)));
						//System.Diagnostics.Debug.WriteLine(string.Format("{0}", result.formatted_address));

						if (item == "route" && street == null)
						{
							street = addresComponent.long_name;
						}
						else if (item == "route" && city == null)
						{
							city = addresComponent.long_name;
						}
						else if (item == "street_number" && streetNumber == null)
						{
							streetNumber = addresComponent.long_name;
						}
						else if ((item == "sublocality" || item == "neighborhood") && colony == null)
						{
							colony = addresComponent.long_name;
						}
						else if (item == "political" && state == null)
						{
							state = addresComponent.long_name;
						}
						else if (item == "country" && country == null)
						{
							country = addresComponent.short_name;
						}
						else if (item == "postal_code" && postalCode == null)
						{
							postalCode = addresComponent.long_name;
						}
					}
					ncomp++;
				}

			}
		}

		async void MakeOrder(object sender, System.EventArgs e)
		{

			if (chooseAddress)
			{
				if (AddresChoosed != null)
				{
					AddresChoosed(this, null);
				}
				await Navigation.PopAsync();
			}
			else {
				if (NextPage != null)
				{
					SaveInfo();
					NextPage(this);
				}
			}

		}
	}

	public static class ViewExtensions
	{
		public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
		{
			Func<double, Color> transform = (t) =>
			  Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
							 fromColor.G + t * (toColor.G - fromColor.G),
							 fromColor.B + t * (toColor.B - fromColor.B),
							 fromColor.A + t * (toColor.A - fromColor.A));
			return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
		}

		public static void CancelAnimation(this VisualElement self)
		{
			self.AbortAnimation("ColorTo");
		}

		static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
		{
			easing = easing ?? Easing.Linear;
			var taskCompletionSource = new TaskCompletionSource<bool>();

			element.Animate<Color>(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
			return taskCompletionSource.Task;
		}
	}
}

