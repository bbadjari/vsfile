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
		const char WindowsDirectorySeparatorChar = '\\';

		////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Path to directory containing Visual Studio file.
		/// </summary>
		string m_directoryPath;

		/// <summary>
		/// File extension of Visual Studio file.
		/// </summary>
		string m_fileExtension;

		/// <summary>
		/// Name of Visual Studio file.
		/// </summary>
		string m_fileName;

		/// <summary>
		/// Name of Visual Studio file with no file extension.
		/// </summary>
		string m_fileNameNoExtension;

		/// <summary>
		/// File path to Visual Studio file.
		/// </summary>
		string m_filePath;

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
		{
			if (string.IsNullOrEmpty(fileExtension) || string.IsNullOrEmpty(filePath))
				throw new ArgumentException();

			// Ensure platform-specific directory separator character used in file path.
			filePath = filePath.Replace(WindowsDirectorySeparatorChar, Path.DirectorySeparatorChar);

			m_directoryPath = Path.GetDirectoryName(filePath);

			// Use current directory if no directory information in file path.
			if (string.IsNullOrEmpty(m_directoryPath))
				m_directoryPath = Directory.GetCurrentDirectory();

			m_fileExtension = fileExtension;
			m_fileName = Path.GetFileName(filePath);
			m_fileNameNoExtension = Path.GetFileNameWithoutExtension(filePath);
			m_filePath = filePath;
		}

		////////////////////////////////////////////////////////////////////////
		// Public Methods

		/// <summary>
		/// Load file.
		/// </summary>
		public void Load()
		{
			CheckFilePath();

			CheckFileExtension();

			ReadFile();
		}

		////////////////////////////////////////////////////////////////////////
		// Protected Methods

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

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Throw exception if file extension is invalid.
		/// </summary>
		void CheckFileExtension()
		{
			string extension = Path.GetExtension(FilePath);

			if (!extension.Equals(FileExtension, StringComparison.OrdinalIgnoreCase))
				throw new IOException("Invalid file extension.");
		}

		/// <summary>
		/// Throw exception if file path is invalid.
		/// </summary>
		void CheckFilePath()
		{
			if (!File.Exists(FilePath))
				throw new FileNotFoundException("File not found at path: " + FilePath);
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get path to directory containing file.
		/// </summary>
		/// <value>
		/// String representing path to directory containing this Visual Studio file.
		/// </value>
		public string DirectoryPath
		{
			get { return m_directoryPath; }
		}

		/// <summary>
		/// Get file extension.
		/// </summary>
		/// <value>
		/// String representing file extension of this Visual Studio file.
		/// </value>
		public string FileExtension
		{
			get { return m_fileExtension; }
		}

		/// <summary>
		/// Get file name.
		/// </summary>
		/// <value>
		/// String representing file name of this Visual Studio file.
		/// </value>
		public string FileName
		{
			get { return m_fileName; }
		}

		/// <summary>
		/// Get file name with no file extension.
		/// </summary>
		/// <value>
		/// String representing file name of this Visual Studio file with no file extension.
		/// </value>
		public string FileNameNoExtension
		{
			get { return m_fileNameNoExtension; }
		}

		/// <summary>
		/// Get file path.
		/// </summary>
		/// <value>
		/// String representing file path to this Visual Studio file.
		/// </value>
		public string FilePath
		{
			get { return m_filePath; }
		}
	}
}
