using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class BorderlessEntry : Entry
	{
		public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
   propertyName: "MaxLength",
   returnType: typeof(int),
   declaringType: typeof(int),
   defaultValue: -1);

		public BorderlessEntry()
		{
			this.TextChanged += (sender, e) =>
			{
				if (MaxLenght > 0){
					String val = this.Text; //Get Current Text

					if (val.Length > MaxLenght)//If it is more than your character restriction
					{
						val = val.Remove(val.Length - 1);// Remove Last character 
						this.Text = val; //Set the Old value
					}
				}
			
			};
		}

		public int MaxLenght
		{
			get { return (int)GetValue(MaxLengthProperty); }
			set { SetValue(MaxLengthProperty, value);
			}
		}



	}
}

