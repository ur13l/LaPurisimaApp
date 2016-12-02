﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LaPurisima
{
	public partial class MakeOrderPage : BasePage
	{
		string street = null, streetNumber, colony = null, city = null, postalCode = null, state = null, country = null;
		Position START_POINT = new Position(26.9034632, -101.4199217);
		Distance START_DISTANCE = Distance.FromMiles(1.7);
		public MakeOrderPage()
		{
			InitializeComponent();

			Title = "Hacer un pedido";

			_confirmLocationBtn.Clicked += async (sender, e) =>
			{
				await Task.WhenAll(new Task[] { _confirmLocationBtn.ScaleTo(0, 250, Easing.SinIn) });

				var googleMaps = await ClientLaPurisima.GetAddresForPosition(_lastPosition);
				//System.Diagnostics.Debug.WriteLine(googleMaps);
				Traverse(googleMaps);
				UpdateView();
				SaveInfo();
			};

		/*	Map.PropertyChanged += (sender, e) =>
			{
				//System.Diagnostics.Debug.WriteLine(e.PropertyName);
				if (e.PropertyName == "VisibleRegion")
				{
					UpdatePoints(Map.VisibleRegion.Center);
				}
			};
			Map.MoveToRegion(MapSpan.FromCenterAndRadius(START_POINT, START_DISTANCE));*/
		}
		Position _lastPosition;
		async void UpdatePoints(Position position)
		{
			_lastPosition = position;
			if (_confirmLocationBtn.Scale < 1)
			{
				await Task.WhenAll(new Task[] {_confirmLocationBtn.ScaleTo(1, 250, Easing.SinIn) });
			}

			//_confirmLocationBtn.IsVisible = true;
		}

		void UpdateView()
		{

			AnimateText(_colony, colony);
			AnimateText(_number, streetNumber);
			AnimateText(_street, street);

			//await this.ColorTo(Color.FromRgb(0, 0, 0), Color.FromRgb(255, 255, 255), c => BackgroundColor = c, 5000);
			//await boxView.ColorTo(Color.Blue, Color.Red, c => boxView.Color = c, 4000);
		}

		async void AnimateText(Label label, string newText)
		{
			var color = label.TextColor;
			await Task.WhenAll(
				label.ColorTo(Color.Gray, Color.Black, c => label.TextColor = c, 200),

				label.ColorTo(Color.Black, Color.Gray, c => label.TextColor = c, 200));

			label.Text = newText;
		}

		void SaveInfo()
		{
			//HelperOrdenPage.Pedido.latitud = Map.VisibleRegion.Center.Latitude;
		//	HelperOrdenPage.Pedido.longitud = Map.VisibleRegion.Center.Longitude;
			HelperOrdenPage.street = street;
			HelperOrdenPage.city = city;
			HelperOrdenPage.streetNumber = streetNumber;
			HelperOrdenPage.country = country;
			HelperOrdenPage.postalcode = postalCode;
			HelperOrdenPage.state = state;
			HelperOrdenPage.colony = colony;
		}

		void Traverse(GoogleMapsLocation googleMaps)
		{
			if (googleMaps == null || googleMaps.results == null || googleMaps.results.Count == 0)
				return;

			street = null; streetNumber = null; colony = null; city = null; postalCode = null; state = null;
			country = null;

			foreach (var result in googleMaps.results)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("result: formatted_address: {0}, types: [{1}]", result.formatted_address, String.Join(" ", result.types)));

				int ncomp = 0;
				foreach (var addresComponent in result.address_components)
				{
					System.Diagnostics.Debug.WriteLine(string.Format(" addresComponent[{0}]: formatted_address: {1}, short_name:{2} types: [{3}]", ncomp, addresComponent.long_name, addresComponent.short_name, String.Join(" ", addresComponent.types)));


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
						else if (item == "sublocality" && colony == null)
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

		void MakeOrder(object sender, System.EventArgs e)
		{

			NextPage(this);
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

