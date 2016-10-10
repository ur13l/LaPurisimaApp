using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using LaPurisima;
using LaPurisima.Droid;
using com.refractored.monodroidtoolkit.imageloader;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]
namespace LaPurisima.Droid
{
	public class CustomImageRenderer : ImageRenderer
	{
		ImageLoader imageLoader;

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			if (Control != null && Element != null)
			{
				CustomImage customImage = e.NewElement as CustomImage;
				imageLoader = ImageLoaderCache.GetImageLoader(this);
				SetImageUrl(customImage.ImageUrl);
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == "ImageUrl")
			{
				CustomImage customImage = Element as CustomImage;
				SetImageUrl(customImage.ImageUrl);
			}
		}

		public void SetImageUrl(string imageUrl)
		{
			if (Control == null)
			{
				return;
			}
			if (imageUrl != null)
			{
				imageLoader.DisplayImage(imageUrl, Control, -1);
			}
		}
	}

	public static class ImageLoaderCache
	{
		static ImageLoader ilLoader;

		public static ImageLoader GetImageLoader(CustomImageRenderer customImageRenderer)
		{
			if (ilLoader == null)
			{
				ilLoader = new ImageLoader(global::Android.App.Application.Context, 300, 40);
			}
			return ilLoader;
		}
	}
}