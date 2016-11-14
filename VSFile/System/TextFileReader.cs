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
