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
		static class ProjectTag
		{
			/// <summary>
			/// Regular expression used to obtain data in project tag.
			/// </summary>
			public static class Regex
			{
				/// <summary>
				/// Group used to match project file path.
				/// </summary>
				public const string FilePathGroup = "Path";

				/// <summary>
				/// Group used to match project name.
				/// </summary>
				public const string NameGroup = "Name";

				/// <summary>
				/// Pattern used to match project GUIDs, name and file path.
				/// </summary>
				public const string Pattern = "^" + Begin + @"\(" +
					Guid.TypeGuid + @"\) = " + Name + ", " + FilePath + ", " +
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
				static class Guid
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
					const string Begin = @"""{(?<";

					/// <summary>
					/// End of GUID pattern.
					/// </summary>
					const string End = @">[A-F\d-]+)}""";
				}

				/// <summary>
				/// Pattern used to match project file path.
				/// </summary>
				const string FilePath = @"""(?<" + FilePathGroup + @">.+)""";

				/// <summary>
				/// Pattern used to match project name.
				/// </summary>
				const string Name = @"""(?<" + NameGroup + @">.+)""";
			}

			/// <summary>
			/// Beginning of project tag.
			/// </summary>
			public const string Begin = "Project";
		}

		/// <summary>
		/// GUIDs used to identify types of projects.
		/// </summary>
		static class ProjectTypeGuid
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
		}

		////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Visual Basic project files referenced in this solution file.
		/// </summary>
		List<BasicProjectFile> m_basicProjectFiles;

		/// <summary>
		/// Visual C# project files referenced in this solution file.
		/// </summary>
		List<CSharpProjectFile> m_cSharpProjectFiles;

		/// <summary>
		/// Visual F# project files referenced in this solution file.
		/// </summary>
		List<FSharpProjectFile> m_fSharpProjectFiles;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="filePath">
		/// String representing path to solution file.
		/// </param>
		public SolutionFile(string filePath)
			: base(SolutionFileExtension, filePath)
		{
			m_basicProjectFiles = new List<BasicProjectFile>();
			m_cSharpProjectFiles = new List<CSharpProjectFile>();
			m_fSharpProjectFiles = new List<FSharpProjectFile>();
		}

		////////////////////////////////////////////////////////////////////////
		// Protected Methods

		/// <summary>
		/// Read file.
		/// </summary>
		protected override void ReadFile()
		{
			using (TextReader reader = new StreamReader(FilePath))
			{
				string inputLine;

				// Read entire solution file.
				while ((inputLine = reader.ReadLine()) != null)
				{
					if (inputLine.StartsWith(ProjectTag.Begin, StringComparison.Ordinal))
						AddProjectFile(inputLine);
				}
			}
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Add project file given line of input from solution file.
		/// </summary>
		/// <param name="inputLine">
		/// String representing line of input from solution file.
		/// </param>
		void AddProjectFile(string inputLine)
		{
			Match match = Regex.Match(inputLine, ProjectTag.Regex.Pattern);

			if (match.Success)
			{
				string projectName = GetMatchValue(match, ProjectTag.Regex.NameGroup);
				string projectTypeGuid = GetMatchValue(match, ProjectTag.Regex.TypeGuidGroup);
				string relativeFilePath = GetMatchValue(match, ProjectTag.Regex.FilePathGroup);

				string filePath = GetFullFilePath(relativeFilePath);

				switch (projectTypeGuid)
				{
					case ProjectTypeGuid.Basic:
						m_basicProjectFiles.Add(new BasicProjectFile(projectName, filePath));

						break;
					case ProjectTypeGuid.CSharp:
						m_cSharpProjectFiles.Add(new CSharpProjectFile(projectName, filePath));

						break;
					case ProjectTypeGuid.FSharp:
						m_fSharpProjectFiles.Add(new FSharpProjectFile(projectName, filePath));

						break;
				}
			}
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
		static string GetMatchValue(Match match, string groupName)
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
			get { return m_basicProjectFiles; }
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
			get { return m_cSharpProjectFiles; }
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
			get { return m_fSharpProjectFiles; }
		}
	}
}
