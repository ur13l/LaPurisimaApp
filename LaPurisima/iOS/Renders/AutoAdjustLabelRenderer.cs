using System;
using LaPurisima;
using LaPurisima.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof (AutoAdjustLabel), typeof (AutoAdjustLabelRenderer))]
namespace LaPurisima.iOS
{
	public class AutoAdjustLabelRenderer : LabelRenderer
	{
		// Override the OnElementChanged method so
		// we can tweak this renderer post-initial setup
		protected override void OnElementChanged (
			ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);

			var label = Control as UILabel;
			if (label != null) {
				label.AdjustsFontSizeToFitWidth = true;
				label.Lines = 1;
				label.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
				label.LineBreakMode = UILineBreakMode.Clip;
			}
		}
	}
}