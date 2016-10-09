using System;
namespace LaPurisima
{
	public enum WEB_METHODS
	{
		Autenticate,Forgot,Update
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
				case WEB_METHODS.Forgot:
					return "password/email";
				case WEB_METHODS.Update:
					return "usuario/update";	
				default:
					
					break;
			}

			return null;
		}

	}
}

