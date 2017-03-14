using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidHUD;
using LaPurisima;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Media;

[assembly: ExportRenderer(typeof(BasePage), typeof(LaPurisima.Droid.BasePageRenderer))]
namespace LaPurisima.Droid
{
	public class BasePageRenderer : PageRenderer
	{ 
		BasePage _basePage;

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement is BasePage)
			{
				_basePage = e.NewElement as BasePage;
				_basePage.TakePicture = TakePicture;
				_basePage.SelectFromGallery = SelectFromGallery;
			}

			var page = e.NewElement as BasePage;

			// this is a ViewGroup - so should be able to load an AXML file and FindView<>
			var activity = this.Context as Activity;

			page.ShowProgressMessage = (x) =>
			{
				AndHUD.Shared.Show(activity, x,-1,MaskType.Black);
			};

			page.HideProgressAction = () =>
			{
				AndHUD.Shared.Dismiss();

			};

			page.ShowProgressType = (t) =>
			{
				if (t == IProgressType.Done)
				{
					AndHUD.Shared.ShowSuccess(activity, LaPurisima.Localize.GetString("DoneLabel"), MaskType.Black);
					AndHUD.Shared.CurrentDialog.SetCancelable(true);
				}

				if (t == IProgressType.LogedIn)
				{
					AndHUD.Shared.ShowSuccess(activity, LaPurisima.Localize.GetString("LogedInLabel"), MaskType.Black);
					AndHUD.Shared.CurrentDialog.SetCancelable(true);
				}
			};
		}

		void SelectFromGallery()
		{
			var picker = new MediaPicker(this.Context);
			var intent = picker.GetPickPhotoUI();

			var mainActivity = this.Context as MainActivity;
			mainActivity.StartActivity(intent, OnActivityResult);
		}

		void TakePicture()
		{
			var picker = new MediaPicker(this.Context);
			if (!picker.IsCameraAvailable)
				Console.WriteLine("No camera!");
			else {
				var intent = picker.GetTakePhotoUI(new StoreCameraMediaOptions
				{
					Name = string.Format("{0}.jpg", Guid.NewGuid()),
					Directory = "Pictures"
				});

				var mainActivity = this.Context as MainActivity;
				mainActivity.StartActivity(intent, OnActivityResult);
			}
		}

		private async void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
		{
			if (resultCode == Android.App.Result.Canceled)
				return;

			var res = await data.GetMediaFileExtraAsync(this.Context);
			MediaFile media = res;
			//ShowPhoto(media);

			MemoryStream mstr = new MemoryStream();
			media.GetStream().CopyTo(mstr);

			if (_basePage != null)
			{
				_basePage.ImagePath = media.Path;
				_basePage.bytes = ResizeImageAndroid(mstr.ToArray(), 400, 400, 100);
				_basePage.Source = ImageSource.FromStream(() => new MemoryStream(_basePage.bytes));
			}

		}

		public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height, int quality)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

			float oldWidth = (float)originalImage.Width;
			float oldHeight = (float)originalImage.Height;
			float scaleFactor = 0f;

			if (oldWidth > oldHeight)
			{
				scaleFactor = width / oldWidth;
			}
			else
			{
				scaleFactor = height / oldHeight;
			}

			float newHeight = oldHeight * scaleFactor;
			float newWidth = oldWidth * scaleFactor;

			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Jpeg, quality, ms);
				return ms.ToArray();
			}
		}
	}
}