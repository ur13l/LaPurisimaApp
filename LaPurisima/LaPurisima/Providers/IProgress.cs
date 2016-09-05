using System;
namespace LaPurisima
{
	public interface IProgress
	{
		void ShowProgress(string text);

		void Dismiss();
	}
}

