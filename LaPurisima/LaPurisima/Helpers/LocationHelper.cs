//using System;
//using Xamarin.Forms;
//using Xamarin.Forms.Maps;

//namespace LaPurisima
//{
//	public class LocationHelper
//	{

//		public Position CurrentPosition = null;
//		public Xamarin Geolocator;

//		static LocationHelper _instance;
//		public static LocationHelper Instance
//		{
//			get
//			{
//				if (_instance == null)
//				{
//					_instance = new LocationHelper();
//				}

//				return _instance;
//			}
//		}

//		private LocationHelper()
//		{
//			SetupGeoLocator();
//		}

//		void SetupGeoLocator()
//		{
//			if (Geolocator != null)
//				return;
//			Geolocator = DependencyService.Get<IGeolocator>();
//			Geolocator.DesiredAccuracy = 100;

//			Geolocator.PositionChanged += (sender, e) =>
//			{
//				CurrentPosition = e.Position;
//				System.Diagnostics.Debug.WriteLine("PositionChanged {0} {1}", e.Position.Latitude, e.Position.Longitude);
//			};

//			Geolocator.StartListening(1, 1);
//		}

		 

//		//public static double DistanceBetween(Position a, Position b)
//		//{
//		//	double d = Math.Acos(
//		//	   (Math.Sin(a.Latitude) * Math.Sin(b.Latitude)) +
//		//	   (Math.Cos(a.Latitude) * Math.Cos(b.Latitude))
//		//	   * Math.Cos(b.Longitude - a.Longitude));
//		//	return 6378137 * d;
//		//}

//	}
//}

