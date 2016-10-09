using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class CarouselPageOrder : CarouselPage
	{
		public CarouselPageOrder()
		{

			var pagina1 = new SelectProductoPage();
			var pagina2 = new MakeOrderPage();
			var pagina3 = new ConfirmOrderPage();


			Children.Add(pagina1);
			Children.Add(pagina2);
			Children.Add(pagina3);
		}
	}
}
