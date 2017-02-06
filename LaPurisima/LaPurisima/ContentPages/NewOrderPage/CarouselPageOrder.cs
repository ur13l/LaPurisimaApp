using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class CarouselPageOrder : CarouselPage
	{
		ConfirmOrderPage _confirmOrder;


		public CarouselPageOrder(RootPage rootPage)
		{

			var pagina1 = new SelectProductoPage();
			var pagina2 = new MakeOrderPage();
			_confirmOrder = new ConfirmOrderPage();

			pagina1.NextPage = NextPage;
			pagina2.NextPage = NextPage;
			_confirmOrder.NextPage = (obj)=>
			{ 
				HelperOrdenPage.city = null;
				HelperOrdenPage.colony = null;
				HelperOrdenPage.country = null;
				HelperOrdenPage.Pedido = null;
				HelperOrdenPage.postalcode = null;
				HelperOrdenPage.state = null;
				HelperOrdenPage.street = null;
				HelperOrdenPage.streetNumber = null;

				rootPage.Detail = new NavigationPage(new CarouselPageOrder(rootPage));
			};

			Children.Add(pagina1);
			Children.Add(pagina2);
			Children.Add(_confirmOrder);

			Title = Localize.GetString("product_selection", "");
		}

		protected override void OnCurrentPageChanged()
		{
			base.OnCurrentPageChanged();
			var n = Children.IndexOf(CurrentPage);
			switch (n)
			{
				case 0:
					Title = Localize.GetString("product_selection","");
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

			//if (n == 2)
				_confirmOrder.UpdateView();
		}

		void NextPage(ContentPage obj)
		{
			var n = Children.IndexOf(obj);
			if (Children.Count > n)
				CurrentPage = Children[n + 1];

		}
	}
}
