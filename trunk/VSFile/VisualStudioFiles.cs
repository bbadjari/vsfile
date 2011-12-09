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
using VSFile.Project;
using VSFile.Source;

namespace VSFile
{
	/// <summary>
	/// Manages multiple Visual Studio files of varying type.
	/// </summary>
	public class VisualStudioFiles
	{
		/// <summary>
		/// Wildcard characters contained in file paths.
		/// </summary>
		static class Wildcard
		{
			/// <summary>
			/// Zero or more characters.
			/// </summary>
			public const string Asterisk = "*";

			/// <summary>
			/// Zero or one character.
			/// </summary>
			public const string Question = "?";
		}

		/// <summary>
		/// Supported file extensions.
		/// </summary>
		static readonly string[] SupportedExtensions = new string[]
		{
			BasicProjectFile.ProjectFileExtension,
			BasicSourceFile.SourceFileExtension,
			CSharpProjectFile.ProjectFileExtension,
			CSharpSourceFile.SourceFileExtension,
			SolutionFile.SolutionFileExtension
		};

		/// <summary>
		/// Wildcard characters contained in file paths.
		/// </summary>
		static readonly string[] Wildcards = new string[]
		{
			Wildcard.Asterisk,
			Wildcard.Question
		};

		////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initialized Visual Basic project files.
		/// </summary>
		List<BasicProjectFile> m_basicProjectFiles;

		/// <summary>
		/// Initialized Visual Basic source files.
		/// </summary>
		List<BasicSourceFile> m_basicSourceFiles;

		/// <summary>
		/// Initialized Visual C# project files.
		/// </summary>
		List<CSharpProjectFile> m_cSharpProjectFiles;

		/// <summary>
		/// Initialized Visual C# source files.
		/// </summary>
		List<CSharpSourceFile> m_cSharpSourceFiles;

		/// <summary>
		/// Option to use when searching for files.
		/// </summary>
		SearchOption m_fileSearchOption;

		/// <summary>
		/// Initialized Visual Studio solution files.
		/// </summary>
		List<SolutionFile> m_solutionFiles;

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor for specifying file paths.
		/// </summary>
		/// <param name="filePaths">
		/// Array of strings representing file paths.
		/// </param>
		public VisualStudioFiles(string[] filePaths)
			: this(filePaths, false)
		{
		}

		/// <summary>
		/// Constructor for specifying recursive search option.
		/// </summary>
		/// <param name="filePaths">
		/// Array of strings representing file paths.
		/// </param>
		/// <param name="recursiveSearch">
		/// True if all subdirectories in given file paths are also to be
		/// searched, false otherwise.
		/// </param>
		public VisualStudioFiles(string[] filePaths, bool recursiveSearch)
		{
			m_basicProjectFiles = new List<BasicProjectFile>();
			m_basicSourceFiles = new List<BasicSourceFile>();
			m_cSharpProjectFiles = new List<CSharpProjectFile>();
			m_cSharpSourceFiles = new List<CSharpSourceFile>();
			m_fileSearchOption = recursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			m_solutionFiles = new List<SolutionFile>();

			Initialize(filePaths);
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Determine if given file path contains any wildcard characters.
		/// </summary>
		/// <param name="filePath">
		/// String representing file path.
		/// </param>
		/// <returns>
		/// True if file path contains any wildcard characters, false otherwise.
		/// </returns>
		static bool HasWildcard(string filePath)
		{
			if (!string.IsNullOrEmpty(filePath))
			{
				foreach (string wildcard in Wildcards)
				{
					if (filePath.Contains(wildcard))
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Initialize Visual Studio files at given paths.
		/// </summary>
		/// <param name="filePaths">
		/// Array of strings representing file paths.
		/// </param>
		void Initialize(string[] filePaths)
		{
			if (filePaths == null)
				throw new ArgumentNullException();
			
			foreach (string filePath in filePaths)
				Initialize(filePath);
		}

		/// <summary>
		/// Initialize file at given path.
		/// </summary>
		/// <param name="filePath">
		/// String representing file path.
		/// </param>
		void Initialize(string filePath)
		{
			Debug.Assert(!string.IsNullOrEmpty(filePath), "Invalid file path.");

			string directoryPath = Path.GetDirectoryName(filePath);

			// Skip if any wildcard characters in directory portion of file path.
			if (HasWildcard(directoryPath))
				return;

			string fileName = Path.GetFileName(filePath);

			// Resolve any wildcard characters in file portion of file path.
			if (HasWildcard(fileName))
			{
				string[] filePaths = Directory.GetFiles(directoryPath, fileName, FileSearchOption);

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
		void Initialize(string filePath, string fileExtension)
		{
			Debug.Assert(!string.IsNullOrEmpty(filePath), "Invalid file path.");
			Debug.Assert(!string.IsNullOrEmpty(fileExtension), "Invalid file extension.");

			switch (fileExtension)
			{
				case BasicProjectFile.ProjectFileExtension:
					m_basicProjectFiles.Add(new BasicProjectFile(filePath));

					break;
				case BasicSourceFile.SourceFileExtension:
					m_basicSourceFiles.Add(new BasicSourceFile(filePath));

					break;
				case CSharpProjectFile.ProjectFileExtension:
					m_cSharpProjectFiles.Add(new CSharpProjectFile(filePath));

					break;
				case CSharpSourceFile.SourceFileExtension:
					m_cSharpSourceFiles.Add(new CSharpSourceFile(filePath));

					break;
				case SolutionFile.SolutionFileExtension:
					m_solutionFiles.Add(new SolutionFile(filePath));

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
		static bool IsSupportedExtension(string fileExtension)
		{
			if (!string.IsNullOrEmpty(fileExtension))
			{
				foreach (string extension in SupportedExtensions)
				{
					if (fileExtension.Equals(extension, StringComparison.CurrentCultureIgnoreCase))
						return true;
				}
			}

			return false;
		}

		////////////////////////////////////////////////////////////////////////
		// Public Properties

		/// <summary>
		/// Get initialized Visual Basic project files.
		/// </summary>
		/// <value>
		/// Enumerable collection of BasicProjectFile objects representing
		/// initialized Visual Basic project files.
		/// </value>
		public IEnumerable<BasicProjectFile> BasicProjectFiles
		{
			get { return m_basicProjectFiles; }
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
			get { return m_basicSourceFiles; }
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
			get { return m_cSharpProjectFiles; }
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
			get { return m_cSharpSourceFiles; }
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
			get { return m_solutionFiles; }
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get option to use when searching for files.
		/// </summary>
		/// <value>
		/// SearchOption enumeration value specifying whether to search all
		/// subdirectories for files or only current directory.
		/// </value>
		SearchOption FileSearchOption
		{
			get { return m_fileSearchOption; }
		}
	}
}
