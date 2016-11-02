using System;
namespace LaPurisima
{
	public enum WEB_METHODS
	{
		Autenticate, 
		PassForgot, 
		CrearUsuario,
		UsuarioUpdate, 
		PedidoNuevo,
		GetProductos,
		GetProductosWhere,
		GetLocationName,
		GetPedidosUsuario,
	}

	public enum WEB_ERROR
	{
		Error,
		NoError,
		EmailExists,
		ParseError,
		ServerError
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
				case WEB_METHODS.PassForgot:
					return "password/email";
				case WEB_METHODS.CrearUsuario:
					return "/usuario/create";
				case WEB_METHODS.UsuarioUpdate:
					return "usuario/update";
				case WEB_METHODS.PedidoNuevo:
					return "pedido/nuevo";
				case WEB_METHODS.GetProductos:
					return "producto/get";
				case WEB_METHODS.GetProductosWhere:
					return "producto/get?search=";
				case WEB_METHODS.GetLocationName:
					return "https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyApnytpG7fVfQV7QdoeWJ0oY561v1iliV4&language=es&latlng=";
				case WEB_METHODS.GetPedidosUsuario:
					return "https://lapurisimaweb.herokuapp.com/pedido/usuario";
				default:

					break;
			}

			return null;
		}

	}
}

