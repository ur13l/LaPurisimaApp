using System;
using Realms;

namespace LaPurisima
{
	public class RepartidorHelper
	{

		public static void SetEstatusRepartidor(bool online,bool updateEstatusAndLocationWeb=true)
		{

			var user = PropertiesManager.GetUserInfo();
			if (user != null)
			{
				if (user.tipo_usuario_id == 2)
				{

					try
					{
						//Guarda en realm
						Realm.GetInstance().Write(() =>
						{
							user.status = online ? 1 : 2; //1 activo , 2 inactivo
						});

						if (updateEstatusAndLocationWeb)
						{
							UpdateLocationStatusRepartidor(user);
						}
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine("error updating status. " + ex.Message);
					}
				}

			}

		}

		public static async void UpdateLocationStatusRepartidor(User user)
		{

			try
			{
				var res = await ClientLaPurisima.PostObject<User>(user, WEB_METHODS.SetStatusRepartidor);
				System.Diagnostics.Debug.WriteLine("RES UPDATE STATUS REPARIDOR: " + res);

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("error updating status. " + ex.Message);
			}
		}
	}
}
