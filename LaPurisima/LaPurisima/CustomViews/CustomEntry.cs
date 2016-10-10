using System;
using Xamarin.Forms;

namespace LaPurisima
{
	public class CustomEntry : Entry
	{
		public delegate void LoadEventHandler();

		public event LoadEventHandler LoadEvent;

		public void Load()
		{
			if (LoadEvent != null)
			{
				LoadEvent();
			}
		}

		public bool DoneKey { get; set; }

		public bool Autocapitalization { get; set; }

		public bool Correction { get; set; }

		public Action NextPressed = delegate
		{

		};
	}
}