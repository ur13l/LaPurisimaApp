using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class CarouselOrder : CarouselPage
	{
		public CarouselOrder()
		{

			var pagina1 = new MakeOrderPage();
			var pagina2 = new MakeOrderPage();
			var pagina3 = new Profile();


			Children.Add(pagina1);
			Children.Add(pagina2);
			Children.Add(pagina3);
		}
	}
}
