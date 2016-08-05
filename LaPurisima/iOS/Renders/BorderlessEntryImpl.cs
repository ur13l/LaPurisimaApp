using System;
using LaPurisima;
using LaPurisima.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryImpl))]
namespace LaPurisima.iOS
{
	public class BorderlessEntryImpl : EntryRenderer
	{

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control != null)
			{
				Control.BorderStyle = UIKit.UITextBorderStyle.None;
			}
		}


	}
}

