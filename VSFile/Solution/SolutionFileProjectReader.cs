////////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2021 Bernard Badjari
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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using VSFile.Properties;
using VSFile.System;

namespace VSFile.Solution
{
	/// <summary>
	/// Reads project references contained in a Visual Studio solution file.
	/// </summary>
	internal static class SolutionFileProjectReader
	{
		/// <summary>
		/// Regular expression used to obtain data in project reference header.
		/// </summary>
		private static class Header
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
			public const string Pattern = "^" + Begin + @"\(" + Guid.TypeGuid + @"\) = " + Name + ", " + Path + ", " + Guid.UniqueGuid + "$";

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
		/// Beginning of project reference.
		/// </summary>
		private const string Begin = "Project";

		/// <summary>
		/// End of project reference.
		/// </summary>
		private const string End = "EndProject";

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Read project reference in solution file.
		/// </summary>
		/// <param name="textFileReader">
		/// ITextFileReader instance representing text file reader.
		/// </param>
		/// <param name="formatVersion">
		/// Integer representing solution file format version.
		/// </param>
		/// <returns>
		/// SolutionFileProject representing project reference in solution file.
		/// </returns>
		public static SolutionFileProject Read(ITextFileReader textFileReader, int formatVersion)
		{
			if (textFileReader == null)
				throw new ArgumentNullException(nameof(textFileReader));

			SolutionFileProject project = SolutionFileProject.None;

			bool hasNoBegin = true;
			bool hasNoEnd = true;

			while (textFileReader.HasText())
			{
				string inputLine = textFileReader.ReadLine();

				if (inputLine.StartsWith(Begin, StringComparison.Ordinal))
				{
					hasNoBegin = false;

					project = GetProject(inputLine, textFileReader, formatVersion);
				}
				else if (inputLine.Equals(End, StringComparison.Ordinal))
				{
					hasNoEnd = false;

					break;
				}
			}

			if (hasNoBegin ^ hasNoEnd)
				throw new FileFormatException(ExceptionMessages.InvalidSolutionFileProjectReference);

			return project;
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
			Debug.Assert(match != null, "Invalid match.");
			Debug.Assert(!string.IsNullOrWhiteSpace(groupName), "Invalid group name.");

			Group group = match.Groups[groupName];

			return group.Value;
		}

		/// <summary>
		/// Get project reference from solution file.
		/// </summary>
		/// <param name="inputLine">
		/// String representing line of input from solution file.
		/// </param>
		/// <param name="textFileReader">
		/// ITextFileReader instance representing text file reader.
		/// </param>
		/// <param name="formatVersion">
		/// Integer representing solution file format version.
		/// </param>
		/// <returns>
		/// SolutionFileProject representing project reference in solution file.
		/// </returns>
		private static SolutionFileProject GetProject(string inputLine, ITextFileReader textFileReader, int formatVersion)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(inputLine), "Invalid input line.");
			Debug.Assert(textFileReader != null, "Invalid text file reader.");

			SolutionFileProject project = SolutionFileProject.None;

			Match match = Regex.Match(inputLine, Header.Pattern);

			if (match.Success)
			{
				string name = GetMatchValue(match, Header.NameGroup);
				string typeGuid = GetMatchValue(match, Header.TypeGuidGroup);
				string uniqueGuid = GetMatchValue(match, Header.UniqueGuidGroup);

				string path = GetProjectPath(match, typeGuid, textFileReader, formatVersion);

				project = new SolutionFileProject(name, path, typeGuid, uniqueGuid);
			}
			else
			{
				throw new FileFormatException(ExceptionMessages.InvalidSolutionFileProjectReference);
			}

			return project;
		}

		/// <summary>
		/// Get project relative path.
		/// </summary>
		/// <param name="headerMatch">
		/// Match representing regular expression match results for project reference header.
		/// </param>
		/// <param name="typeGuid">
		/// String representing project type GUID.
		/// </param>
		/// <param name="textFileReader">
		/// ITextFileReader instance representing text file reader.
		/// </param>
		/// <param name="formatVersion">
		/// Integer representing solution file format version.
		/// </param>
		/// <returns>
		/// String representing project relative path.
		/// </returns>
		private static string GetProjectPath(Match headerMatch, string typeGuid, ITextFileReader textFileReader, int formatVersion)
		{
			Debug.Assert(headerMatch != null, "Invalid match.");
			Debug.Assert(!string.IsNullOrWhiteSpace(typeGuid), "Invalid type GUID.");
			Debug.Assert(textFileReader != null, "Invalid text file reader.");

			string path = GetMatchValue(headerMatch, Header.PathGroup);

			ISolutionFileProjectPathResolver pathResolver = SolutionFileProjectPathResolverFactory.Create(typeGuid, textFileReader, formatVersion);

			if (pathResolver != SolutionFileProjectPathResolverFactory.NoPathResolver)
				path = pathResolver.GetPath();

			return path;
		}
	}
}
