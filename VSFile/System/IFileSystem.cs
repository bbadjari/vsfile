using System.IO;

namespace VSFile.System
{
	/// <summary>
	/// File system contract.
	/// </summary>
	internal interface IFileSystem
	{
		/// <summary>
		/// Determine if directory at given path exists.
		/// </summary>
		/// <param name="path">
		/// String representing directory path.
		/// </param>
		/// <returns>
		/// True if directory at given path exists, false otherwise.
		/// </returns>
		bool DirectoryExists(string path);

		/// <summary>
		/// Determine if file at given path exists.
		/// </summary>
		/// <param name="path">
		/// String representing file path.
		/// </param>
		/// <returns>
		/// True if file at given path exists, false otherwise.
		/// </returns>
		bool FileExists(string path);

		/// <summary>
		/// Get current working directory.
		/// </summary>
		/// <returns>
		/// String representing current working directory.
		/// </returns>
		string GetCurrentDirectory();

		/// <summary>
		/// Get file paths that match given search pattern in given directory path, searching subdirectories if specified.
		/// </summary>
		/// <param name="path">
		/// String representing directory path.
		/// </param>
		/// <param name="searchPattern">
		/// String representing pattern to match against names of files in directory path.
		/// </param>
		/// <param name="searchOption">
		/// SearchOption enumeration value specifying whether to search all subdirectories for files or only current directory.
		/// </param>
		/// <returns>
		/// Array of strings representing matching file paths in given directory path.
		/// </returns>
		string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
	}
}
