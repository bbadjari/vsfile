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
using System.Collections.Generic;
using System.IO;
using VSFile.Properties;
using VSFile.Source;
using VSFile.System;

namespace VSFile.Project
{
	/// <summary>
	/// Represents an ASP.NET web site directory.
	/// </summary>
	public class WebSiteDirectory
	{
		/// <summary>
		/// Supported web site file extensions.
		/// </summary>
		private static readonly string[] SupportedExtensions = new string[]
		{
			BasicSourceFile.SourceFileExtension,
			CSharpSourceFile.SourceFileExtension,
		};

		////////////////////////////////////////////////////////////////////////

		private List<BasicSourceFile> basicSourceFiles;

		private List<CSharpSourceFile> cSharpSourceFiles;

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">
		/// String representing web site name.
		/// </param>
		/// <param name="directoryPath">
		/// String representing path to directory containing web site files.
		/// </param>
		public WebSiteDirectory(string name, string directoryPath)
			: this(name, directoryPath, new FileSystem())
		{
		}

		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="name">
		/// String representing web site name.
		/// </param>
		/// <param name="directoryPath">
		/// String representing path to directory containing web site files.
		/// </param>
		/// <param name="fileSystem">
		/// IFileSystem instance representing file system.
		/// </param>
		internal WebSiteDirectory(string name, string directoryPath, IFileSystem fileSystem)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException(ExceptionMessages.InvalidName);

			if (string.IsNullOrWhiteSpace(directoryPath))
				throw new ArgumentException(ExceptionMessages.InvalidDirectoryPath);

			if (fileSystem == null)
				throw new ArgumentNullException("fileSystem");

			basicSourceFiles = new List<BasicSourceFile>();
			cSharpSourceFiles = new List<CSharpSourceFile>();
			DirectoryPath = directoryPath;
			FileSystem = fileSystem;
			Name = name;
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Load web site files.
		/// </summary>
		public void Load()
		{
			CheckDirectoryPath();

			ClearFiles();

			LoadFiles();
		}

		/// <summary>
		/// Throw exception if directory path is invalid.
		/// </summary>
		private void CheckDirectoryPath()
		{
			if (!FileSystem.DirectoryExists(DirectoryPath))
				throw new DirectoryNotFoundException(string.Format(ExceptionMessages.DirectoryNotFound, DirectoryPath));
		}

		/// <summary>
		/// Clear web site files.
		/// </summary>
		private void ClearFiles()
		{
			basicSourceFiles.Clear();
			cSharpSourceFiles.Clear();
		}

		/// <summary>
		/// Load web site files.
		/// </summary>
		private void LoadFiles()
		{
			List<string> filePaths = new List<string>();

			foreach (string extension in SupportedExtensions)
			{
				string filePath = Path.Combine(DirectoryPath, Wildcard.AddAsterisk(extension));

				filePaths.Add(filePath);
			}

			VisualStudioFiles files = new VisualStudioFiles(filePaths, true, FileSystem);

			basicSourceFiles.AddRange(files.BasicSourceFiles);
			cSharpSourceFiles.AddRange(files.CSharpSourceFiles);
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get Visual Basic source files contained in this web site.
		/// </summary>
		/// <value>
		/// Enumerable collection of BasicSourceFile objects representing
		/// Visual Basic source files contained in this web site.
		/// </value>
		public IEnumerable<BasicSourceFile> BasicSourceFiles
		{
			get { return basicSourceFiles; }
		}

		/// <summary>
		/// Get Visual C# source files contained in this web site.
		/// </summary>
		/// <value>
		/// Enumerable collection of CSharpSourceFile objects representing
		/// Visual C# source files contained in this web site.
		/// </value>
		public IEnumerable<CSharpSourceFile> CSharpSourceFiles
		{
			get { return cSharpSourceFiles; }
		}

		/// <summary>
		/// Get directory path.
		/// </summary>
		/// <value>
		/// String representing path to directory containing web site files.
		/// </value>
		public string DirectoryPath { get; private set; }

		/// <summary>
		/// Get web site name.
		/// </summary>
		/// <value>
		/// String representing web site name.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		/// Get/set file system.
		/// </summary>
		/// <value>
		/// IFileSystem instance representing file system.
		/// </value>
		private IFileSystem FileSystem { get; set; }
	}
}
