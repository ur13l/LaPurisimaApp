using System;
namespace LaPurisima
{
	public enum WEB_METHODS
	{
		Autenticate, Forgot, Update, GetProductos,
		GetProductosWhere
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
				case WEB_METHODS.GetProductos:
					return "producto/get";
				case WEB_METHODS.GetProductosWhere:
					return "producto/get?search=";
				default:

					break;
			}

			return null;
		}

	}
}

