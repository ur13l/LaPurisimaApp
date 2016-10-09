using System;
using LaPurisima;
using LaPurisima.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TintedImage), typeof(TintedImageImpl))]
namespace LaPurisima.iOS
{
	public class TintedImageImpl : ImageRenderer
	{
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control != null && ((TintedImage)this.Element) != null && ((TintedImage)this.Element).TintColor != null && Control.Image != null)
			{
				Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
				this.Control.TintColor = ((TintedImage)this.Element).TintColor.ToUIColor();
			}
		}
	}
}
