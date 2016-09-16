using System;
namespace LaPurisima
{
	public enum WEB_METHODS
	{
		Autenticate,
	}

	public class Config
	{
		public static string URL = "https://lapurisimaweb.herokuapp.com/";


		public static string GetURLForMethod(WEB_METHODS method)
		{

			switch (method)
			{
				case WEB_METHODS.Autenticate:
					return "usuario/authenticate";
				default:
					
					break;
			}

			return null;
		}

	}
}

