using System;

namespace VSFile.System
{
	/// <summary>
	/// Text file reader contract.
	/// </summary>
	internal interface ITextFileReader : IDisposable
	{
		/// <summary>
		/// Determine whether there is text to read from file; that is, end of file not reached.
		/// </summary>
		/// <returns>
		/// True if there is text to read from file, false otherwise.
		/// </returns>
		bool HasText();

		/// <summary>
		/// Read current line in text file.
		/// </summary>
		/// <returns>
		/// String representing current line in text file.
		/// </returns>
		string ReadLine();
	}
}
