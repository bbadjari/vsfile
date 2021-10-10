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
using System.Diagnostics;
using System.IO;
using System.Linq;
using VSFile.Project;
using VSFile.Properties;
using VSFile.Source;
using VSFile.System;

namespace VSFile
{
	/// <summary>
	/// Manages multiple Visual Studio files of varying type.
	/// </summary>
	public class VisualStudioFiles
	{
		/// <summary>
		/// Supported file extensions.
		/// </summary>
		private static readonly string[] SupportedExtensions = new string[]
		{
			BasicProjectFile.ProjectFileExtension,
			BasicSourceFile.SourceFileExtension,
			CSharpProjectFile.ProjectFileExtension,
			CSharpSourceFile.SourceFileExtension,
			FSharpProjectFile.ProjectFileExtension,
			FSharpSourceFile.SourceFileExtension,
			SolutionFile.SolutionFileExtension
		};

		////////////////////////////////////////////////////////////////////////

		private List<BasicProjectFile> basicProjectFiles;

		private List<BasicSourceFile> basicSourceFiles;

		private List<CSharpProjectFile> cSharpProjectFiles;

		private List<CSharpSourceFile> cSharpSourceFiles;

		private List<FSharpProjectFile> fSharpProjectFiles;

		private List<FSharpSourceFile> fSharpSourceFiles;

		private List<SolutionFile> solutionFiles;

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor for specifying file paths.
		/// </summary>
		/// <param name="filePaths">
		/// Enumerable collection of strings representing file paths.
		/// </param>
		public VisualStudioFiles(IEnumerable<string> filePaths)
			: this(filePaths, false)
		{
		}

		/// <summary>
		/// Constructor for specifying recursive search option.
		/// </summary>
		/// <param name="filePaths">
		/// Enumerable collection of strings representing file paths.
		/// </param>
		/// <param name="recursiveSearch">
		/// True if all subdirectories in given file paths are also to be
		/// searched, false otherwise.
		/// </param>
		public VisualStudioFiles(IEnumerable<string> filePaths, bool recursiveSearch)
			: this(filePaths, recursiveSearch, new FileSystem())
		{
		}

		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="filePaths">
		/// Enumerable collection of strings representing file paths.
		/// </param>
		/// <param name="recursiveSearch">
		/// True if all subdirectories in given file paths are also to be
		/// searched, false otherwise.
		/// </param>
		/// <param name="fileSystem">
		/// IFileSystem instance representing file system.
		/// </param>
		internal VisualStudioFiles(IEnumerable<string> filePaths, bool recursiveSearch, IFileSystem fileSystem)
		{
			if (fileSystem == null)
				throw new ArgumentNullException(nameof(fileSystem));

			basicProjectFiles = new List<BasicProjectFile>();
			basicSourceFiles = new List<BasicSourceFile>();
			cSharpProjectFiles = new List<CSharpProjectFile>();
			cSharpSourceFiles = new List<CSharpSourceFile>();
			fSharpProjectFiles = new List<FSharpProjectFile>();
			fSharpSourceFiles = new List<FSharpSourceFile>();
			FileSearchOption = recursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			FileSystem = fileSystem;
			solutionFiles = new List<SolutionFile>();

			Initialize(filePaths);
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Initialize Visual Studio files at given paths.
		/// </summary>
		/// <param name="filePaths">
		/// Enumerable collection of strings representing file paths.
		/// </param>
		private void Initialize(IEnumerable<string> filePaths)
		{
			if (filePaths == null)
				throw new ArgumentNullException(nameof(filePaths));

			foreach (string filePath in filePaths)
				Initialize(filePath);
		}

		/// <summary>
		/// Initialize file at given path.
		/// </summary>
		/// <param name="filePath">
		/// String representing file path.
		/// </param>
		private void Initialize(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
				throw new ArgumentException(ExceptionMessages.InvalidFilePath);

			string directoryPath = Path.GetDirectoryName(filePath);

			// Use current directory if no directory information in file path.
			if (string.IsNullOrWhiteSpace(directoryPath))
				directoryPath = FileSystem.GetCurrentDirectory();

			// Skip if any wildcard characters in directory portion of file path.
			if (Wildcard.HasWildcard(directoryPath))
				return;

			string fileName = Path.GetFileName(filePath);

			// Resolve any wildcard characters in file portion of file path.
			if (Wildcard.HasWildcard(fileName))
			{
				string[] filePaths = FileSystem.GetFiles(directoryPath, fileName, FileSearchOption);

				Initialize(filePaths);

				return;
			}

			string fileExtension = Path.GetExtension(filePath).ToLowerInvariant();

			// Skip unsupported file extensions.
			if (!IsSupportedExtension(fileExtension))
				return;

			// File path contains no wildcard characters.
			Initialize(filePath, fileExtension);
		}

		/// <summary>
		/// Initialize file at given path with given supported extension.
		/// </summary>
		/// <param name="filePath">
		/// String representing file path.
		/// </param>
		/// <param name="fileExtension">
		/// String representing supported file extension.
		/// </param>
		private void Initialize(string filePath, string fileExtension)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(filePath), "Invalid file path.");
			Debug.Assert(!string.IsNullOrWhiteSpace(fileExtension), "Invalid file extension.");

			if (!FileSystem.FileExists(filePath))
				throw new FileNotFoundException(string.Format(ExceptionMessages.FileNotFound, filePath));

			switch (fileExtension)
			{
				case BasicProjectFile.ProjectFileExtension:
					basicProjectFiles.Add(new BasicProjectFile(filePath));

					break;
				case BasicSourceFile.SourceFileExtension:
					basicSourceFiles.Add(new BasicSourceFile(filePath));

					break;
				case CSharpProjectFile.ProjectFileExtension:
					cSharpProjectFiles.Add(new CSharpProjectFile(filePath));

					break;
				case CSharpSourceFile.SourceFileExtension:
					cSharpSourceFiles.Add(new CSharpSourceFile(filePath));

					break;
				case FSharpProjectFile.ProjectFileExtension:
					fSharpProjectFiles.Add(new FSharpProjectFile(filePath));

					break;
				case FSharpSourceFile.SourceFileExtension:
					fSharpSourceFiles.Add(new FSharpSourceFile(filePath));

					break;
				case SolutionFile.SolutionFileExtension:
					solutionFiles.Add(new SolutionFile(filePath));

					break;
			}
		}

		/// <summary>
		/// Determine if given file extension is supported.
		/// </summary>
		/// <param name="fileExtension">
		/// String representing file extension.
		/// </param>
		/// <returns>
		/// True if file extension is supported, false otherwise.
		/// </returns>
		private static bool IsSupportedExtension(string fileExtension)
		{
			if (string.IsNullOrWhiteSpace(fileExtension))
				return false;

			return SupportedExtensions.Any(extension => fileExtension.Equals(extension, StringComparison.CurrentCultureIgnoreCase));
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get initialized Visual Basic project files.
		/// </summary>
		/// <value>
		/// Enumerable collection of BasicProjectFile objects representing
		/// initialized Visual Basic project files.
		/// </value>
		public IEnumerable<BasicProjectFile> BasicProjectFiles
		{
			get { return basicProjectFiles; }
		}

		/// <summary>
		/// Get initialized Visual Basic source files.
		/// </summary>
		/// <value>
		/// Enumerable collection of BasicSourceFile objects representing
		/// initialized Visual Basic source files.
		/// </value>
		public IEnumerable<BasicSourceFile> BasicSourceFiles
		{
			get { return basicSourceFiles; }
		}

		/// <summary>
		/// Get initialized Visual C# project files.
		/// </summary>
		/// <value>
		/// Enumerable collection of CSharpProjectFile objects representing
		/// initialized Visual C# project files.
		/// </value>
		public IEnumerable<CSharpProjectFile> CSharpProjectFiles
		{
			get { return cSharpProjectFiles; }
		}

		/// <summary>
		/// Get initialized Visual C# source files.
		/// </summary>
		/// <value>
		/// Enumerable collection of CSharpSourceFile objects representing
		/// initialized Visual C# source files.
		/// </value>
		public IEnumerable<CSharpSourceFile> CSharpSourceFiles
		{
			get { return cSharpSourceFiles; }
		}

		/// <summary>
		/// Get initialized Visual F# project files.
		/// </summary>
		/// <value>
		/// Enumerable collection of FSharpProjectFile objects representing
		/// initialized Visual F# project files.
		/// </value>
		public IEnumerable<FSharpProjectFile> FSharpProjectFiles
		{
			get { return fSharpProjectFiles; }
		}

		/// <summary>
		/// Get initialized Visual F# source files.
		/// </summary>
		/// <value>
		/// Enumerable collection of FSharpSourceFile objects representing
		/// initialized Visual F# source files.
		/// </value>
		public IEnumerable<FSharpSourceFile> FSharpSourceFiles
		{
			get { return fSharpSourceFiles; }
		}

		/// <summary>
		/// Get initialized Visual Studio solution files.
		/// </summary>
		/// <value>
		/// Enumerable collection of SolutionFile objects representing
		/// initialized Visual Studio solution files.
		/// </value>
		public IEnumerable<SolutionFile> SolutionFiles
		{
			get { return solutionFiles; }
		}

		/// <summary>
		/// Get/set option to use when searching for files.
		/// </summary>
		/// <value>
		/// SearchOption enumeration value specifying whether to search all
		/// subdirectories for files or only current directory.
		/// </value>
		private SearchOption FileSearchOption { get; set; }

		/// <summary>
		/// Get/set file system.
		/// </summary>
		/// <value>
		/// IFileSystem instance representing file system.
		/// </value>
		private IFileSystem FileSystem { get; set; }
	}
}
