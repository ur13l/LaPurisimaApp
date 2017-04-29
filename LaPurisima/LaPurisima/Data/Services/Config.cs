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
		GetPedidosRepartidor,
		GetPedidosSolicitados,
		SetStatusRepartidor,
		CancelarPedido,
		FinalizarPedido,
		GetAddressCoordenates,
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
		//public static string URL = "http://10.0.7.42/LaPurisimaWeb/public/";
		//public static string URL = "http://localhost/lapurisimaweb/public/";
		public static string URL = "https://lapurisimaweb.herokuapp.com/";

		public static string GetURLForMethod(WEB_METHODS method)
		{

			switch (method)
			{
				case WEB_METHODS.Autenticate:
					return "/usuario/authenticate";
				case WEB_METHODS.PassForgot:
					return "password/email";
				case WEB_METHODS.CrearUsuario:
					return "usuario/create";
				case WEB_METHODS.UsuarioUpdate:
					return "usuario/update";
				case WEB_METHODS.PedidoNuevo:
					return "pedido/nuevo";
				case WEB_METHODS.GetProductos:
					return "producto/get";
				case WEB_METHODS.GetProductosWhere:
					return "producto/get?search=";
				case WEB_METHODS.GetLocationName:
					return "https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyApnytpG7fVfQV7QdoeWJ0oY561v1iliV4&language=es-ES&latlng=";
				case WEB_METHODS.GetPedidosUsuario:
					return "pedido/usuario";
				case WEB_METHODS.GetPedidosRepartidor:
					return "pedido/repartidor";
				case WEB_METHODS.GetPedidosSolicitados:
					return "pedido/solicitados";
				case WEB_METHODS.SetStatusRepartidor:
					return "repartidor/status";
				case WEB_METHODS.CancelarPedido:
					return "pedido/cancelar";
				case WEB_METHODS.FinalizarPedido:
					return "pedido/finalizar";
				case WEB_METHODS.GetAddressCoordenates:
					return "http://maps.google.com/maps/api/geocode/json?address=";
				default:

					break;
			}

			return null;
		}

	}
}
