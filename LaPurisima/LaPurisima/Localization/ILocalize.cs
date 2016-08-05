using System;
using System.Globalization;

namespace LaPurisima
{
	public interface ILocalize
	{
		CultureInfo GetCurrentCultureInfo();

		void SetLocale();
	}
}

