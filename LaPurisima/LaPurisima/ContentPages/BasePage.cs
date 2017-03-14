using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class BasePage : ContentPage
	{
		public Action<string> ShowProgressMessage;
		public Action<IProgressType> ShowProgressType;
		public Action HideProgressAction;

		public Action<ContentPage> NextPage;
		public Action<ContentPage> PreviousPage;
		public Action<int> GoToPage;

		IProgress progressDependency;

		protected BasePage()
		{
			progressDependency = DependencyService.Get<IProgress>();
		}

		public void ShowProgress(string message)
		{
			if (progressDependency != null)
				progressDependency.ShowProgress(message);
			if (ShowProgressMessage != null)
				ShowProgressMessage(message);
		}

		public void ShowProgress(IProgressType type)
		{
			progressDependency = DependencyService.Get<IProgress>();
			if (progressDependency != null)
				progressDependency.ShowProgress(type);
			if (ShowProgressType != null)
				ShowProgressType(type);
		}

		public void HideProgress()
		{
			if (progressDependency != null)
				progressDependency.Dismiss();
			if (HideProgressAction != null)
				HideProgressAction();
		}



		#region PICTURES


		public string imagePath = null;
		public Action TakePicture, SelectFromGallery;

		public bool imageChanged = false;

		public string ImagePath
		{
			get
			{
				return imagePath;
			}

			set
			{
				imagePath = value;
				imageChanged = true;
				//if (_imageView != null)
				//	Device.BeginInvokeOnMainThread(() =>
				//	{
				//		_imageView.Source = imagePath;
				//	});
			}
		}

		public object LastView;
		public Action ImageSourceChanged;

		ImageSource _source;
		public ImageSource Source
		{
			set
			{
				imageChanged = true;
				_source = value;

				if (ImageSourceChanged != null)
					ImageSourceChanged();
			}
			get
			{
				return _source;
			}
		}

		byte[] _bytes;

		public byte[] bytes
		{
			get
			{
				return _bytes;
			}

			set
			{
				_bytes = value;

				Base64Image = Convert.ToBase64String(_bytes);
			}
		}

		public string Base64Image { get; set; }



		public async void TakePictureActionSheet(object imageView = null)
		{
			LastView = imageView;
			var n = await DisplayActionSheet("Elige una imagen", "cancelar", null, new string[] { "Cámara", "Galería" });
			switch (n)
			{
				case "Cámara":
					if (TakePicture != null)
					{
						TakePicture();
					}
					break;
				case "Galería":
					if (SelectFromGallery != null)
					{
						SelectFromGallery();
					}
					break;
			}
		}


		#endregion




	}
}

