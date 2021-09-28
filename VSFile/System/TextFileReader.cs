using System.IO;

namespace VSFile.System
{
	/// <summary>
	/// Text file reader implementation.
	/// </summary>
	internal class TextFileReader : ITextFileReader
	{
		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">
		/// String representing text file path.
		/// </param>
		public TextFileReader(string path)
		{
			TextReader = new StreamReader(path);
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public void Dispose()
		{
			TextReader.Dispose();
		}

		/// <summary>
		/// Determine whether there is text to read from file; that is, end of file not reached.
		/// </summary>
		/// <returns>
		/// True if there is text to read from file, false otherwise.
		/// </returns>
		public bool HasText()
		{
			const int NoMoreCharacters = -1;

			return TextReader.Peek() != NoMoreCharacters;
		}

		/// <summary>
		/// Read current line in text file.
		/// </summary>
		/// <returns>
		/// String representing current line in text file.
		/// </returns>
		public string ReadLine()
		{
			return TextReader.ReadLine();
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get/set text reader.
		/// </summary>
		/// <value>
		/// TextReader representing text reader.
		/// </value>
		private TextReader TextReader { get; set; }
	}
}
