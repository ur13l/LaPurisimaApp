using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using LaPurisima;
using LaPurisima.iOS;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]
namespace LaPurisima.iOS
{
	public class CustomImageRenderer : ImageRenderer, IProviderImage
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			if (Control != null && Element != null)
			{
				CustomImage customImage = e.NewElement as CustomImage;
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

		public async void SetImageUrl(string imageUrl)
		{
			if (Control == null)
			{
				return;
			}
			if (imageUrl != null)
			{
				//Control.Image = UIImage.FromBundle(imageUrl);

				Control.Image = await LoadImage(imageUrl);
			}
			else {

			}
		}

		public async Task<UIImage> LoadImage(string imageUrl)
		{
			var httpClient = new HttpClient();

			Task<byte[]> contentsTask = httpClient.GetByteArrayAsync(imageUrl);

			// await! control returns to the caller and the task continues to run on another thread
			var contents = await contentsTask;

			// load from bytes
			return UIImage.LoadFromData(NSData.FromArray(contents));
		}

	}
}