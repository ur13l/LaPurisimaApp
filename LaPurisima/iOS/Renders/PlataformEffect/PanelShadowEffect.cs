using System;
using System.Linq;
using CoreGraphics;
using LaPurisima.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("MyProject")]
[assembly: ExportEffect(typeof(PanelShadowEffect), "PanelShadowEffect")]
namespace LaPurisima.iOS
{
	public class PanelShadowEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);
				if (effect == null)
				{
					return;
				}

				var control = Control;
				if (control == null)
				{
					var renderer = Platform.GetRenderer((VisualElement)Element);
					control = renderer.ViewController.View;
				}

				control.Layer.ShadowRadius = 1.5f;
				//control.Layer.CornerRadius = effect.Radius;
				control.Layer.ShadowColor = effect.Color.ToCGColor();
				control.Layer.ShadowOffset = new CGSize(effect.DistanceX, effect.DistanceY);
				control.Layer.ShadowOpacity = 0.8f;
				//control.Layer.ShadowOpacity = 0.8f;
				
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: {0}", ex.Message);
			}
		}

		protected override void OnDetached()
		{
		}
	}
}