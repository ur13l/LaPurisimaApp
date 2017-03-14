using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using LaPurisima;
using LaPurisima.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Media;

[assembly: ExportRenderer(typeof(BasePage), typeof(BasePageRenderer))]
namespace LaPurisima.iOS
{
	public class BasePageRenderer : PageRenderer
	{
		BasePage _basePage;

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}

			if (e.NewElement is BasePage)
			{
				_basePage = e.NewElement as BasePage;

				_basePage.SelectFromGallery = SelectFromGallery;
				_basePage.TakePicture = TakePicture;
			}
		}

		async void TakePicture()
		{
			var picker = new MediaPicker();
			if (picker.IsCameraAvailable)
			{
				MediaPickerController controller = picker.GetTakePhotoUI(new StoreCameraMediaOptions
				{
					Name = string.Format("{0}.jpg", Guid.NewGuid()),
					Directory = "Pictures"
				});


				// On iPad, you'll use UIPopoverController to present the controller
				PresentViewController(controller, true, null);

				await controller.GetResultAsync().ContinueWith(t =>
				{
					// We need to dismiss the controller ourselves
					DismissViewController(true, () =>
					{
						// User canceled or something went wrong
						if (t.IsCanceled || t.IsFaulted)
							return;

						// We get back a MediaFile
						MediaFile media = t.Result;
						//ShowPhoto(media);
						MemoryStream mstr = new MemoryStream();
						media.GetStream().CopyTo(mstr);

						if (_basePage != null)
						{
							_basePage.ImagePath = media.Path;
							_basePage.bytes = ResizeTheImage(mstr.ToArray(), 400, 400);
							_basePage.Source = ImageSource.FromStream(() => new MemoryStream(_basePage.bytes));
						}
					});
					// Make sure we use the UI thread to show our photo.
				}, TaskScheduler.FromCurrentSynchronizationContext());

			}
		}

		async void SelectFromGallery()
		{
			var picker = new MediaPicker();

			MediaPickerController controller = picker.GetPickPhotoUI();

			// On iPad, you'll use UIPopoverController to present the controller
			PresentViewController(controller, true, null);

			//var result = await controller.GetResultAsync();
			//controller.DismissViewController(true, null);
			//MediaFile file = result;
			//_page.ImagePath = file.Path;

			await controller.GetResultAsync().ContinueWith(t =>
				{
					// We need to dismiss the controller ourselves
					controller.DismissViewController(true, () =>
					{
						// User canceled or something went wrong
						if (t.IsCanceled || t.IsFaulted)
							return;
						MediaFile media = t.Result;
						MemoryStream mstr = new MemoryStream();
						media.GetStream().CopyTo(mstr);

						if (_basePage != null)
						{
							_basePage.ImagePath = media.Path;
							_basePage.bytes = ResizeTheImage(mstr.ToArray(), 400, 400);
							_basePage.Source = ImageSource.FromStream(() => new MemoryStream(_basePage.bytes));
						}

					});
					// Make sure we use the UI thread to show our photo.
				}, TaskScheduler.FromCurrentSynchronizationContext());
		}



		public byte[] ResizeTheImage(byte[] imageData, float width, float height)
		{
			UIImage originalImage = ImageFromByteArray(imageData);
			originalImage = MaxResizeImage(originalImage, width, height);
			UIImageOrientation orientation = originalImage.Orientation;

			return originalImage.AsJPEG().ToArray();
		}

		public static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1) return sourceImage;
			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			UIGraphics.BeginImageContext(new CGSize((nfloat)width, (nfloat)height));
			sourceImage.Draw(new CGRect(0, 0, (nfloat)width, (nfloat)height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return resultImage;
		}

		public static UIKit.UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			UIKit.UIImage image;
			try
			{
				image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}

	}
}


