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
using VSFile.Source;

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
		static readonly string[] SupportedExtensions = new string[]
		{
			BasicSourceFile.SourceFileExtension,
			CSharpSourceFile.SourceFileExtension,
		};

		////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Visual Basic source files contained in this web site.
		/// </summary>
		List<BasicSourceFile> m_basicSourceFiles;

		/// <summary>
		/// Visual C# source files contained in this web site.
		/// </summary>
		List<CSharpSourceFile> m_cSharpSourceFiles;

		/// <summary>
		/// Path to directory containing web site files.
		/// </summary>
		string m_directoryPath;

		/// <summary>
		/// Web site name.
		/// </summary>
		string m_name;

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
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(directoryPath))
				throw new ArgumentException();

			m_basicSourceFiles = new List<BasicSourceFile>();
			m_cSharpSourceFiles = new List<CSharpSourceFile>();
			m_directoryPath = directoryPath;
			m_name = name;
		}

		////////////////////////////////////////////////////////////////////////
		// Public Methods

		/// <summary>
		/// Load web site files.
		/// </summary>
		public void Load()
		{
			CheckDirectoryPath();

			ClearFiles();

			LoadFiles();
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Throw exception if directory path is invalid.
		/// </summary>
		void CheckDirectoryPath()
		{
			if (!Directory.Exists(DirectoryPath))
				throw new DirectoryNotFoundException("Directory not found at path: " + DirectoryPath);
		}

		/// <summary>
		/// Clear web site files.
		/// </summary>
		void ClearFiles()
		{
			m_basicSourceFiles.Clear();
			m_cSharpSourceFiles.Clear();
		}

		/// <summary>
		/// Load web site files.
		/// </summary>
		void LoadFiles()
		{
			List<string> filePaths = new List<string>();

			foreach (string extension in SupportedExtensions)
			{
				string filePath = Path.Combine(DirectoryPath, Wildcard.AddAsterisk(extension));

				filePaths.Add(filePath);
			}

			VisualStudioFiles files = new VisualStudioFiles(filePaths);

			m_basicSourceFiles.AddRange(files.BasicSourceFiles);
			m_cSharpSourceFiles.AddRange(files.CSharpSourceFiles);
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
			get { return m_basicSourceFiles; }
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
			get { return m_cSharpSourceFiles; }
		}

		/// <summary>
		/// Get directory path.
		/// </summary>
		/// <value>
		/// String representing path to directory containing web site files.
		/// </value>
		public string DirectoryPath
		{
			get { return m_directoryPath; }
		}

		/// <summary>
		/// Get web site name.
		/// </summary>
		/// <value>
		/// String representing web site name.
		/// </value>
		public string Name
		{
			get { return m_name; }
		}
	}
}
