namespace VSFile.System
{
	/// <summary>
	/// Text file reader factory contract.
	/// </summary>
	internal interface ITextFileReaderFactory
	{
		/// <summary>
		/// Create text file reader with given path.
		/// </summary>
		/// <param name="path">
		/// String representing text file path.
		/// </param>
		/// <returns>
		/// ITextFileReader instance representing text file reader.
		/// </returns>
		ITextFileReader Create(string path);
	}
}
