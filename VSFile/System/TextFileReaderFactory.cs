namespace VSFile.System
{
	/// <summary>
	/// Text file reader factory implementation.
	/// </summary>
	internal class TextFileReaderFactory : ITextFileReaderFactory
	{
		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Create text file reader with given path.
		/// </summary>
		/// <param name="path">
		/// String representing text file path.
		/// </param>
		/// <returns>
		/// ITextFileReader instance representing text file reader.
		/// </returns>
		public ITextFileReader Create(string path)
		{
			return new TextFileReader(path);
		}
	}
}
