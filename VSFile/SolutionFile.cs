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
using VSFile.Project;
using VSFile.Solution;
using VSFile.System;

namespace VSFile
{
	/// <summary>
	/// Represents a Visual Studio solution file.
	/// </summary>
	public class SolutionFile : VisualStudioFile
	{
		/// <summary>
		/// File extension of solution file.
		/// </summary>
		public const string SolutionFileExtension = ".sln";

		////////////////////////////////////////////////////////////////////////

		private List<BasicProjectFile> basicProjectFiles;

		private List<CSharpProjectFile> cSharpProjectFiles;

		private List<FSharpProjectFile> fSharpProjectFiles;

		private List<WebSiteDirectory> webSiteDirectories;

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="filePath">
		/// String representing path to solution file.
		/// </param>
		public SolutionFile(string filePath)
			: this(filePath, new FileSystem(), new TextFileReaderFactory())
		{
		}

		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="filePath">
		/// String representing path to solution file.
		/// </param>
		/// <param name="fileSystem">
		/// IFileSystem instance representing file system.
		/// </param>
		/// <param name="textFileReaderFactory">
		/// ITextFileReaderFactory instance representing text file reader factory.
		/// </param>
		internal SolutionFile(string filePath, IFileSystem fileSystem, ITextFileReaderFactory textFileReaderFactory)
			: base(SolutionFileExtension, filePath, fileSystem)
		{
			if (textFileReaderFactory == null)
				throw new ArgumentNullException(nameof(textFileReaderFactory));

			basicProjectFiles = new List<BasicProjectFile>();
			cSharpProjectFiles = new List<CSharpProjectFile>();
			fSharpProjectFiles = new List<FSharpProjectFile>();
			TextFileReaderFactory = textFileReaderFactory;
			webSiteDirectories = new List<WebSiteDirectory>();
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Read file.
		/// </summary>
		protected override void ReadFile()
		{
			ClearFiles();

			using (ITextFileReader textFileReader = TextFileReaderFactory.Create(FilePath))
			{
				FormatVersion = SolutionFileHeaderReader.Read(textFileReader);

				// Read rest of solution file.
				while (textFileReader.HasText())
				{
					SolutionFileProject project = SolutionFileProjectReader.Read(textFileReader);

					AddProject(project);
				}
			}
		}

		/// <summary>
		/// Add given project from solution file.
		/// </summary>
		/// <param name="project">
		/// SolutionFileProject representing project reference in solution file.
		/// </param>
		private void AddProject(SolutionFileProject project)
		{
			if (project == SolutionFileProject.None)
				return;

			string path = GetFullPath(project.Path);

			switch (project.TypeGuid)
			{
				case ProjectTypeGuid.Basic:
					basicProjectFiles.Add(new BasicProjectFile(project.Name, path));

					break;
				case ProjectTypeGuid.CSharp:
					cSharpProjectFiles.Add(new CSharpProjectFile(project.Name, path));

					break;
				case ProjectTypeGuid.FSharp:
					fSharpProjectFiles.Add(new FSharpProjectFile(project.Name, path));

					break;
				case ProjectTypeGuid.WebSite:
					webSiteDirectories.Add(new WebSiteDirectory(project.Name, path));

					break;
			}
		}

		/// <summary>
		/// Clear referenced project files.
		/// </summary>
		private void ClearFiles()
		{
			basicProjectFiles.Clear();
			cSharpProjectFiles.Clear();
			fSharpProjectFiles.Clear();
			webSiteDirectories.Clear();
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get Visual Basic project files referenced in this solution file.
		/// </summary>
		/// <value>
		/// Enumerable collection of BasicProjectFile objects representing
		/// Visual Basic project files referenced in this solution file.
		/// </value>
		public IEnumerable<BasicProjectFile> BasicProjectFiles
		{
			get { return basicProjectFiles; }
		}

		/// <summary>
		/// Get Visual C# project files referenced in this solution file.
		/// </summary>
		/// <value>
		/// Enumerable collection of CSharpProjectFile objects representing
		/// Visual C# project files referenced in this solution file.
		/// </value>
		public IEnumerable<CSharpProjectFile> CSharpProjectFiles
		{
			get { return cSharpProjectFiles; }
		}

		/// <summary>
		/// Get solution file format version.
		/// </summary>
		/// <value>
		/// Integer representing solution file format version.
		/// </value>
		public int FormatVersion { get; private set; }

		/// <summary>
		/// Get Visual F# project files referenced in this solution file.
		/// </summary>
		/// <value>
		/// Enumerable collection of FSharpProjectFile objects representing
		/// Visual F# project files referenced in this solution file.
		/// </value>
		public IEnumerable<FSharpProjectFile> FSharpProjectFiles
		{
			get { return fSharpProjectFiles; }
		}

		/// <summary>
		/// Get ASP.NET web site directories referenced in this solution file.
		/// </summary>
		/// <value>
		/// Enumerable collection of WebSiteDirectory objects representing
		/// ASP.NET web site directories referenced in this solution file.
		/// </value>
		public IEnumerable<WebSiteDirectory> WebSiteDirectories
		{
			get { return webSiteDirectories; }
		}

		/// <summary>
		/// Get/set text file reader factory.
		/// </summary>
		/// <value>
		/// ITextFileReaderFactory instance representing text file reader factory.
		/// </value>
		private ITextFileReaderFactory TextFileReaderFactory { get; set; }
	}
}
