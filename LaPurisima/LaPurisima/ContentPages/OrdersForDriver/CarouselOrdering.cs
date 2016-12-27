using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class CarouselOrdering : CarouselPage
	{


		public CarouselOrdering(RootPage rootPage)
		{

			var pagina1 = new OrdersPage();
			var pagina2 = new MakeOrderPage();

			pagina1.NextPage = NextPage;
			pagina2.NextPage = NextPage;

			Children.Add(pagina1);
			Children.Add(pagina2);

			Title = Localize.GetString("product_selection", "");
		}

		protected override void OnCurrentPageChanged()
		{
			base.OnCurrentPageChanged();
			var n = Children.IndexOf(CurrentPage);
			switch (n)
			{
				case 0:
					Title = Localize.GetString("orders","");
					break;
				case 1:
					Title = Localize.GetString("location_selection", "");
					break;
				case 2:
					Title = Localize.GetString("confirm_order", "");
					break;
				default:
					Title = Localize.GetString("product_selection", "");
					break;
			}
		}

		void NextPage(ContentPage obj)
		{
			var n = Children.IndexOf(obj);
			if (Children.Count > n)
				CurrentPage = Children[n + 1];

		}
	}
}
