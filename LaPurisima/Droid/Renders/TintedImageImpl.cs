using System;
using LaPurisima;
using LaPurisima.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(TintedImage), typeof(TintedImageImpl))]
namespace LaPurisima.Droid
{
	public class TintedImageImpl : ImageRenderer
	{
		
		 protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (this.Control != null)
			{
				Control.SetColorFilter(((TintedImage)this.Element).TintColor.ToAndroid());
			}
		}
	}
}
