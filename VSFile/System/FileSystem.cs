using System.IO;

namespace VSFile.System
{
	/// <summary>
	/// File system implementation.
	/// </summary>
	internal class FileSystem : IFileSystem
	{
		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Determine if directory at given path exists.
		/// </summary>
		/// <param name="path">
		/// String representing directory path.
		/// </param>
		/// <returns>
		/// True if directory at given path exists, false otherwise.
		/// </returns>
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		/// <summary>
		/// Determine if file at given path exists.
		/// </summary>
		/// <param name="path">
		/// String representing file path.
		/// </param>
		/// <returns>
		/// True if file at given path exists, false otherwise.
		/// </returns>
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		/// <summary>
		/// Get current working directory.
		/// </summary>
		/// <returns>
		/// String representing current working directory.
		/// </returns>
		public string GetCurrentDirectory()
		{
			return Directory.GetCurrentDirectory();
		}

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
		public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.GetFiles(path, searchPattern, searchOption);
		}
	}
}
