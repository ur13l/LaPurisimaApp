using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class TintedImage : Image
	{
		public static readonly BindableProperty TintcolorProperty = BindableProperty.Create(
   propertyName: "TintColor",
			returnType: typeof(Color),
			declaringType: typeof(Color),
			defaultValue: Color.Black);
		 
		public Color TintColor
		{
			get { return (Color)GetValue(TintcolorProperty); }
			set
			{
				SetValue(TintcolorProperty, value);
			}
		}
		
	}
}
