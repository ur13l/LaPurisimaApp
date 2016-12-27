﻿using System.ComponentModel;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using LaPurisima.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using View = Android.Views.View;

//[assembly: ExportRenderer(typeof(Entry), typeof(MaterialEntryRenderer_Droid))]

namespace LaPurisima.Droid
{
	public class MaterialEntryRenderer_Droid : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Entry, View>
	{
		private TextInputLayout _nativeView;

		private TextInputLayout NativeView
		{
			get { return _nativeView ?? (_nativeView = InitializeNativeView()); }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				var ctrl = CreateNativeControl();
				SetNativeControl(ctrl);

				SetText();
				SetHintText();
				SetBackgroundColor();
				SetTextColor();
				SetIsPassword();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
			{
				SetHintText();
			}

			if (e.PropertyName == Entry.TextColorProperty.PropertyName)
			{
				SetTextColor();
			}

			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
			{
				SetBackgroundColor();
			}

			if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
			{
				SetIsPassword();
			}

			if (e.PropertyName == Entry.TextProperty.PropertyName)
			{
				SetText();
			}

			if (e.PropertyName == Entry.IsEnabledProperty.PropertyName)
			{
				NativeView.EditText.Enabled = Element.IsEnabled;
			}

		}

		private void EditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
		{
			Element.Text = textChangedEventArgs.Text.ToString();
			NativeView.EditText.SetSelection((Element.Text==null)?0:Element.Text.Length);
		}

		private void SetText()
		{
			NativeView.EditText.Text = Element.Text;
		}

		private void SetIsPassword()
		{
			NativeView.EditText.InputType = Element.IsPassword
				? InputTypes.TextVariationPassword | InputTypes.ClassText
				: NativeView.EditText.InputType;
		}

		public void SetBackgroundColor()
		{
			NativeView.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
		}

		private void SetHintText()
		{
			NativeView.Hint = Element.Placeholder;
		}

		private void SetTextColor()
		{
			if (Element.TextColor == Color.Default)
			{
				NativeView.EditText.SetTextColor(NativeView.EditText.TextColors);
			}
			else
			{
				NativeView.EditText.SetTextColor(Element.TextColor.ToAndroid());
			}
		}

		private TextInputLayout InitializeNativeView()
		{
			var view = FindViewById<TextInputLayout>(Resource.Id.textInputLayout);
			view.EditText.TextChanged += EditTextOnTextChanged;
			return view;
		}

		protected override View CreateNativeControl()
		{

			var view = LayoutInflater.From(Context).Inflate(Resource.Layout.TextInputLayout, null);;
			if (Element != null)
			{
				((TextInputLayout)view).EditText.Enabled = Element.IsEnabled;
			}

			return view;
		}
	}
}