////////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2011 Bernard Badjari
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using VSFile.Properties;
using VSFile.System;

namespace VSFile
{
	/// <summary>
	/// Represents a Visual Studio file.
	/// </summary>
	public abstract class VisualStudioFile
	{
		/// <summary>
		/// Directory separator character on Windows platform.
		/// </summary>
		private const char WindowsDirectorySeparatorChar = '\\';

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="fileExtension">
		/// String representing file extension of Visual Studio file.
		/// </param>
		/// <param name="filePath">
		/// String representing file path to Visual Studio file.
		/// </param>
		/// <param name="fileSystem">
		/// IFileSystem instance representing file system.
		/// </param>
		internal VisualStudioFile(string fileExtension, string filePath, IFileSystem fileSystem)
		{
			if (string.IsNullOrWhiteSpace(fileExtension))
				throw new ArgumentException(ExceptionMessages.InvalidFileExtension);

			if (string.IsNullOrWhiteSpace(filePath))
				throw new ArgumentException(ExceptionMessages.InvalidFilePath);

			if (fileSystem == null)
				throw new ArgumentNullException(nameof(fileSystem));

			// Ensure platform-specific directory separator character used in file path.
			filePath = filePath.Replace(WindowsDirectorySeparatorChar, Path.DirectorySeparatorChar);

			DirectoryPath = Path.GetDirectoryName(filePath);

			// Use current directory if no directory information in file path.
			if (string.IsNullOrWhiteSpace(DirectoryPath))
				DirectoryPath = fileSystem.GetCurrentDirectory();

			FileExtension = fileExtension;
			FileName = Path.GetFileName(filePath);
			FileNameNoExtension = Path.GetFileNameWithoutExtension(filePath);
			FilePath = filePath;
			FileSystem = fileSystem;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fileExtension">
		/// String representing file extension of Visual Studio file.
		/// </param>
		/// <param name="filePath">
		/// String representing file path to Visual Studio file.
		/// </param>
		protected VisualStudioFile(string fileExtension, string filePath)
			: this(fileExtension, filePath, new FileSystem())
		{
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Load file.
		/// </summary>
		public void Load()
		{
			CheckFilePath();

			CheckFileExtension();

			ReadFile();
		}

		/// <summary>
		/// Get full path combining directory path and given relative path.
		/// </summary>
		/// <param name="relativePath">
		/// String representing relative path.
		/// </param>
		/// <returns>
		/// String representing full path combining directory path and given relative path.
		/// </returns>
		protected string GetFullPath(string relativePath)
		{
			return Path.Combine(DirectoryPath, relativePath);
		}

		/// <summary>
		/// Read file.
		/// </summary>
		protected abstract void ReadFile();

		/// <summary>
		/// Throw exception if file extension is invalid.
		/// </summary>
		private void CheckFileExtension()
		{
			string extension = Path.GetExtension(FilePath);

			if (!extension.Equals(FileExtension, StringComparison.OrdinalIgnoreCase))
				throw new IOException(ExceptionMessages.InvalidFileExtension);
		}

		/// <summary>
		/// Throw exception if file path is invalid.
		/// </summary>
		private void CheckFilePath()
		{
			if (!FileSystem.FileExists(FilePath))
				throw new FileNotFoundException(string.Format(ExceptionMessages.FileNotFound, FilePath));
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get path to directory containing file.
		/// </summary>
		/// <value>
		/// String representing path to directory containing this Visual Studio file.
		/// </value>
		public string DirectoryPath { get; private set; }

		/// <summary>
		/// Get file extension.
		/// </summary>
		/// <value>
		/// String representing file extension of this Visual Studio file.
		/// </value>
		public string FileExtension { get; private set; }

		/// <summary>
		/// Get file name.
		/// </summary>
		/// <value>
		/// String representing file name of this Visual Studio file.
		/// </value>
		public string FileName { get; private set; }

		/// <summary>
		/// Get file name with no file extension.
		/// </summary>
		/// <value>
		/// String representing file name of this Visual Studio file with no file extension.
		/// </value>
		public string FileNameNoExtension { get; private set; }

		/// <summary>
		/// Get file path.
		/// </summary>
		/// <value>
		/// String representing file path to this Visual Studio file.
		/// </value>
		public string FilePath { get; private set; }

		/// <summary>
		/// Get/set file system.
		/// </summary>
		/// <value>
		/// IFileSystem instance representing file system.
		/// </value>
		private IFileSystem FileSystem { get; set; }
	}
}
