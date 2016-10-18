using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using LaPurisima;
using LaPurisima.Droid;
using UniversalImageLoader.Core;
//using com.refractored.monodroidtoolkit.imageloader;

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
				imageLoader = ImageLoader.Instance;

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
				var b = new DisplayImageOptions.Builder();
				b.DecodingOptions(new Android.Graphics.BitmapFactory.Options()
				{
					
					InSampleSize = 50,
				});
				imageLoader.DisplayImage(imageUrl, Control, b.Build());
			}
		}
	}

	//public static class ImageLoaderCache
	//{
	//	static ImageLoader ilLoader;

	//	public static ImageLoader GetImageLoader(CustomImageRenderer customImageRenderer)
	//	{
	//		if (ilLoader == null)
	//		{
	//			ilLoader = new ImageLoader(global::Android.App.Application.Context, 64, 40);
	//		}
	//		return ilLoader;
	//	}
	//}
}