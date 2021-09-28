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
using VSFile.Properties;
using VSFile.System;

namespace VSFile.Solution
{
	/// <summary>
	/// Reads the header contained in a Visual Studio solution file.
	/// </summary>
	internal static class SolutionFileHeaderReader
	{
		/// <summary>
		/// Header expected to be contained on one of first two lines of solution file.
		/// </summary>
		private const int MaximumLinesToRead = 2;

		/// <summary>
		/// Header prefix.
		/// </summary>
		private const string Prefix = "Microsoft Visual Studio Solution File, Format Version";

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Read header in solution file and return format version.
		/// </summary>
		/// <param name="textFileReader">
		/// ITextFileReader instance representing text file reader.
		/// </param>
		/// <returns>
		/// Integer representing format version.
		/// </returns>
		public static int Read(ITextFileReader textFileReader)
		{
			string inputLine = null;
			bool hasNoHeader = true;

			for (int line = 0; line < MaximumLinesToRead; line++)
			{
				inputLine = textFileReader.ReadLine();

				if (HasHeader(inputLine))
				{
					hasNoHeader = false;

					break;
				}
			}

			if (hasNoHeader)
				throw new FileFormatException(ExceptionMessages.InvalidSolutionFile);

			return GetFormatVersion(inputLine);
		}

		/// <summary>
		/// Get format version from given input line.
		/// </summary>
		/// <param name="inputLine">
		/// String representing line of input from solution file.
		/// </param>
		/// <returns>
		/// Integer representing format version.
		/// </returns>
		private static int GetFormatVersion(string inputLine)
		{
			Debug.Assert(HasHeader(inputLine), "Invalid file header.");

			string versionInHeader = inputLine.Substring(Prefix.Length);

			Version version;

			if (!Version.TryParse(versionInHeader, out version))
				throw new FileFormatException(ExceptionMessages.InvalidSolutionFileHeader);

			return version.Major;
		}

		/// <summary>
		/// Determine if given input line contains header.
		/// </summary>
		/// <param name="inputLine">
		/// String representing line of input from solution file.
		/// </param>
		/// <returns>
		/// True if input line contains header, false otherwise.
		/// </returns>
		private static bool HasHeader(string inputLine)
		{
			return string.IsNullOrWhiteSpace(inputLine) ? false : inputLine.StartsWith(Prefix, StringComparison.Ordinal);
		}
	}
}
