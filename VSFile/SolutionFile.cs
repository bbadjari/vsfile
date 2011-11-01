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

namespace VSFile
{
	/// <summary>
	/// Represents a Visual Studio solution file.
	/// </summary>
	public class SolutionFile : VisualStudioFile
	{
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
			/// C# project type.
			/// </summary>
			public const string CSharp = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
		}

		/// <summary>
		/// File extension of solution file.
		/// </summary>
		const string SolutionFileExtension = ".sln";

		////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// C# project files contained in solution file.
		/// </summary>
		List<CSharpProjectFile> m_projectFiles;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="filePath">
		/// String representing path to solution file.
		/// </param>
		public SolutionFile(string filePath)
			: base(SolutionFileExtension, filePath)
		{
			m_projectFiles = new List<CSharpProjectFile>();
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
				string projectTypeGuid = GetMatchValue(match, ProjectTag.Regex.TypeGuidGroup);

				// Skip non-C# project types.
				if (projectTypeGuid != ProjectTypeGuid.CSharp)
					return;

				string projectName = GetMatchValue(match, ProjectTag.Regex.NameGroup);
				string relativeFilePath = GetMatchValue(match, ProjectTag.Regex.FilePathGroup);

				m_projectFiles.Add(new CSharpProjectFile(projectName,
					GetFullFilePath(relativeFilePath)));
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
		/// Get C# project files contained in this solution file.
		/// </summary>
		/// <value>
		/// Enumerable collection of CSharpProjectFile objects representing
		/// C# project files referenced in solution file.
		/// </value>
		public IEnumerable<CSharpProjectFile> ProjectFiles
		{
			get { return m_projectFiles; }
		}
	}
}
