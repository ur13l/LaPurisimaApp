using Xamarin.Forms;

namespace LaPurisima
{
	public interface IProviderImage
	{
		void SetImageUrl(string imageUrl);
	}

	public class CustomImage : Image
	{
		public static readonly BindableProperty ImageUrlProperty =
			BindableProperty.Create(
				propertyName: "ImageUrl",
				returnType: typeof(string),
				declaringType: typeof(CustomImage),
				defaultValue: null);

		public string ImageUrl
		{
			get { return (string)GetValue(ImageUrlProperty); }
			set
			{
				SetValue(ImageUrlProperty, value);
			}
		}
	}
}