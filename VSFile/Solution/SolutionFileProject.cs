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
using System.IO;
using System.Text.RegularExpressions;
using VSFile.Properties;
using VSFile.System;

namespace VSFile.Solution
{
	/// <summary>
	/// Represents project reference contained in a Visual Studio solution file.
	/// </summary>
	internal class SolutionFileProject
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
		public void Read(ITextFileReader textFileReader)
		{
			if (textFileReader == null)
				throw new ArgumentNullException("textFileReader");

			bool hasNoBegin = true;
			bool hasNoEnd = true;

			while (textFileReader.HasText())
			{
				string inputLine = textFileReader.ReadLine();

				if (inputLine.StartsWith(Begin, StringComparison.Ordinal))
				{
					hasNoBegin = false;

					SetValues(inputLine);
				}
				else if (inputLine.Equals(End, StringComparison.Ordinal))
				{
					hasNoEnd = false;

					break;
				}
			}

			if (hasNoBegin ^ hasNoEnd)
				throw new FileFormatException(ExceptionMessages.InvalidSolutionFileProjectReference);

			IsValid = !hasNoBegin && !hasNoEnd;
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

		/// <summary>
		/// Set project reference values given line of input from solution file.
		/// </summary>
		/// <param name="inputLine">
		/// String representing line of input from solution file.
		/// </param>
		private void SetValues(string inputLine)
		{
			Match match = Regex.Match(inputLine, Header.Pattern);

			if (match.Success)
			{
				Name = GetMatchValue(match, Header.NameGroup);
				Path = GetMatchValue(match, Header.PathGroup);
				TypeGuid = GetMatchValue(match, Header.TypeGuidGroup);
				UniqueGuid = GetMatchValue(match, Header.UniqueGuidGroup);
			}
			else
			{
				throw new FileFormatException(ExceptionMessages.InvalidSolutionFileProjectReference);
			}
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Determine whether project reference valid.
		/// </summary>
		/// <value>
		/// True if project reference valid, false otherwise.
		/// </value>
		public bool IsValid { get; private set; }

		/// <summary>
		/// Get project name.
		/// </summary>
		/// <value>
		/// String representing project name.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		/// Get project relative path.
		/// </summary>
		/// <value>
		/// String representing project relative path.
		/// </value>
		public string Path { get; private set; }

		/// <summary>
		/// Get project type GUID.
		/// </summary>
		/// <value>
		/// String representing project type GUID.
		/// </value>
		public string TypeGuid { get; private set; }

		/// <summary>
		/// Get project unique GUID.
		/// </summary>
		/// <value>
		/// String representing project unique GUID.
		/// </value>
		public string UniqueGuid { get; private set; }
	}
}
