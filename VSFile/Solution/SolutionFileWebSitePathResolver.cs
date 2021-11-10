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
using VSFile.Project;
using VSFile.Properties;
using VSFile.System;

namespace VSFile.Solution
{
	/// <summary>
	/// Resolves relative paths for ASP.NET web sites referenced in Visual Studio solution files.
	/// </summary>
	[SolutionFileProject(ProjectTypeGuid.WebSite, SolutionFileFormatVersion.VisualStudio2012)]
	internal class SolutionFileWebSitePathResolver : ISolutionFileProjectPathResolver
	{
		/// <summary>
		/// Regular expression used to obtain key-value pairs in web site reference.
		/// </summary>
		private static class KeyValue
		{
			/// <summary>
			/// Group used to match key.
			/// </summary>
			public const string KeyGroup = "Key";

			/// <summary>
			/// Pattern used to match key-value pair.
			/// </summary>
			public const string Pattern = Key + " = " + Value;

			/// <summary>
			/// Group used to match value.
			/// </summary>
			public const string ValueGroup = "Value";

			/// <summary>
			/// Pattern used to match key.
			/// </summary>
			private const string Key = @"(?<" + KeyGroup + @">.+)";

			/// <summary>
			/// Pattern used to match value.
			/// </summary>
			private const string Value = @"""(?<" + ValueGroup + @">.+)""";
		}

		/// <summary>
		/// End of project section.
		/// </summary>
		private const string End = "EndProjectSection";

		/// <summary>
		/// Relative path key.
		/// </summary>
		private const string RelativePathKey = "SlnRelativePath";

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Get project relative path in solution file.
		/// </summary>
		/// <param name="textFileReader">
		/// ITextFileReader instance representing text file reader.
		/// </param>
		/// <returns>
		/// String representing project relative path.
		/// </returns>
		public string GetPath(ITextFileReader textFileReader)
		{
			if (textFileReader == null)
				throw new ArgumentNullException(nameof(textFileReader));

			while (textFileReader.HasText())
			{
				string inputLine = textFileReader.ReadLine().Trim();

				if (inputLine == End)
					break;

				Match match = Regex.Match(inputLine, KeyValue.Pattern);

				if (match.Success)
				{
					string key = match.Groups[KeyValue.KeyGroup].Value;

					if (key == RelativePathKey)
						return match.Groups[KeyValue.ValueGroup].Value;
				}
			}

			throw new FileFormatException(ExceptionMessages.InvalidSolutionFileProjectReference);
		}
	}
}
