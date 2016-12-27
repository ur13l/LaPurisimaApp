using System;
using System.ComponentModel;
using AppSupervisores.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ListView), typeof(ListViewRendererImpl))]
namespace AppSupervisores.iOS
{
	public class ListViewRendererImpl : ListViewRenderer
	{
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			//Control.SeparatorInset = UIEdgeInsets.Zero;
			if (Control == null)
				return;

			Control.SeparatorInset = UIEdgeInsets.Zero;
			Control.LayoutMargins = UIEdgeInsets.Zero;

			if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
				Control.CellLayoutMarginsFollowReadableWidth = false;

			Control.ContentInset = new UIEdgeInsets(0, 0, 0, 0);

		}

	}
}

