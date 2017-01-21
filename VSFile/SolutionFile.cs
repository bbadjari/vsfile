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
using System.Text.RegularExpressions;
using VSFile.Project;
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

		/// <summary>
		/// Project tag contained in solution file.
		/// </summary>
		private static class ProjectTag
		{
			/// <summary>
			/// Regular expression used to obtain data in project tag.
			/// </summary>
			public static class Regex
			{
				/// <summary>
				/// Group used to match project name.
				/// </summary>
				public const string NameGroup = "Name";

				/// <summary>
				/// Group used to match project path.
				/// </summary>
				public const string PathGroup = "Path";

				/// <summary>
				/// Pattern used to match project GUIDs, name and path.
				/// </summary>
				public const string Pattern = "^" + Begin + @"\(" +
					Guid.TypeGuid + @"\) = " + Name + ", " + Path + ", " +
					Guid.UniqueGuid + "$";

				/// <summary>
				/// Group used to match project type GUID.
				/// </summary>
				public const string TypeGuidGroup = "TypeGUID";

				/// <summary>
				/// Group used to match project unique GUID.
				/// </summary>
				public const string UniqueGuidGroup = "UniqueGUID";

				/// <summary>
				/// GUID patterns.
				/// </summary>
				private static class Guid
				{
					/// <summary>
					/// Pattern used to match project type GUID.
					/// </summary>
					public const string TypeGuid = Begin + TypeGuidGroup + End;

					/// <summary>
					/// Pattern used to match project unique GUID.
					/// </summary>
					public const string UniqueGuid = Begin + UniqueGuidGroup + End;

					/// <summary>
					/// Beginning of GUID pattern.
					/// </summary>
					private const string Begin = @"""{(?<";

					/// <summary>
					/// End of GUID pattern.
					/// </summary>
					private const string End = @">[A-F\d-]+)}""";
				}

				/// <summary>
				/// Pattern used to match project name.
				/// </summary>
				private const string Name = @"""(?<" + NameGroup + @">.+)""";

				/// <summary>
				/// Pattern used to match project path.
				/// </summary>
				private const string Path = @"""(?<" + PathGroup + @">.+)""";
			}

			/// <summary>
			/// Beginning of project tag.
			/// </summary>
			public const string Begin = "Project";
		}

		/// <summary>
		/// GUIDs used to identify types of projects.
		/// </summary>
		private static class ProjectTypeGuid
		{
			/// <summary>
			/// Visual Basic project type.
			/// </summary>
			public const string Basic = "F184B08F-C81C-45F6-A57F-5ABD9991F28F";

			/// <summary>
			/// Visual C# project type.
			/// </summary>
			public const string CSharp = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";

			/// <summary>
			/// Visual F# project type.
			/// </summary>
			public const string FSharp = "F2A71F9B-5D33-465A-A702-920D77279786";

			/// <summary>
			/// ASP.NET web site project type.
			/// </summary>
			public const string WebSite = "E24C65DC-7377-472B-9ABA-BC803B73C61A";
		}

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
				throw new ArgumentNullException("textFileReaderFactory");

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
				string inputLine;

				// Read entire solution file.
				while ((inputLine = textFileReader.ReadLine()) != null)
				{
					if (inputLine.StartsWith(ProjectTag.Begin, StringComparison.Ordinal))
						AddProject(inputLine);
				}
			}
		}

		/// <summary>
		/// Add project given line of input from solution file.
		/// </summary>
		/// <param name="inputLine">
		/// String representing line of input from solution file.
		/// </param>
		private void AddProject(string inputLine)
		{
			Match match = Regex.Match(inputLine, ProjectTag.Regex.Pattern);

			if (match.Success)
			{
				string projectName = GetMatchValue(match, ProjectTag.Regex.NameGroup);
				string projectTypeGuid = GetMatchValue(match, ProjectTag.Regex.TypeGuidGroup);
				string relativePath = GetMatchValue(match, ProjectTag.Regex.PathGroup);

				string path = GetFullPath(relativePath);

				switch (projectTypeGuid)
				{
					case ProjectTypeGuid.Basic:
						basicProjectFiles.Add(new BasicProjectFile(projectName, path));

						break;
					case ProjectTypeGuid.CSharp:
						cSharpProjectFiles.Add(new CSharpProjectFile(projectName, path));

						break;
					case ProjectTypeGuid.FSharp:
						fSharpProjectFiles.Add(new FSharpProjectFile(projectName, path));

						break;
					case ProjectTypeGuid.WebSite:
						webSiteDirectories.Add(new WebSiteDirectory(projectName, path));

						break;
				}
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

		/// <summary>
		/// Get match value at given group name.
		/// </summary>
		/// <param name="match">
		/// Match representing regular expression match results.
		/// </param>
		/// <param name="groupName">
		/// String representing name of group to match in given match results.
		/// </param>
		/// <returns>
		/// String representing match value at given group name.
		/// </returns>
		private static string GetMatchValue(Match match, string groupName)
		{
			Group group = match.Groups[groupName];

			return group.Value;
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
