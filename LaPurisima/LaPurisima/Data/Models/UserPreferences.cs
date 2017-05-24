using System;
namespace LaPurisima
{
	public class UserPreferences : Realms.RealmObject
	{
		public string Street { get; set; }
		public string StreetNumber { get; set; }
		public string Neighborhood { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string References { get; set; }
	}
}
