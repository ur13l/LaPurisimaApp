using Android.Gms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms;
using Android.Gms.Maps.Model;
using LaPurisima.Droid;
using LaPurisima;
using System;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace LaPurisima.Droid
{
	public class CustomMapRenderer : MapRenderer, GoogleMap.IOnInfoWindowClickListener
	{
		private bool _isDrawnDone;

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			var formsMap = (CustomMap)sender;

			//var map = ((MapView)Control).Map;

		//	map.MyLocationEnabled = true;

			if (e.PropertyName.Equals("VisibleRegion") && !_isDrawnDone)
			{
				//map.Clear();
				//map.MarkerClick += HandleMarkerClick;
				//map.MyLocationEnabled = formsMap.IsShowingUser;
				//map.SetOnInfoWindowClickListener(this);

				var formsPins = formsMap.Pins;

				foreach (var formsPin in formsPins)
				{
					var markerWithIcon = new MarkerOptions();
					markerWithIcon.SetPosition(new LatLng(formsPin.Position.Latitude, formsPin.Position.Longitude));
					markerWithIcon.SetTitle(formsPin.Label);
					markerWithIcon.SetSnippet(formsPin.Address);
					markerWithIcon.InvokeIcon(BitmapDescriptorFactory.DefaultMarker());

					//var m = map.AddMarker(markerWithIcon);
				//	m.ShowInfoWindow();
				}

				_isDrawnDone = true;

			}

		}

		void HandleMarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
		{

		}

		public void OnInfoWindowClick(Marker marker)
		{

		}
	}

}
