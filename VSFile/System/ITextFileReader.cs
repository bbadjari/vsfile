using System;

namespace VSFile.System
{
	/// <summary>
	/// Text file reader contract.
	/// </summary>
	internal interface ITextFileReader : IDisposable
	{
		/// <summary>
		/// Release resources.
		/// </summary>
		void Dispose();

		/// <summary>
		/// Read current line in text file.
		/// </summary>
		/// <returns>
		/// String representing current line in text file.
		/// </returns>
		string ReadLine();
	}
}
