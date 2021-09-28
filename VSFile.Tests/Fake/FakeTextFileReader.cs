////////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Bernard Badjari
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
using VSFile.System;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Fake
{
	/// <summary>
	/// Fake implementation of text file reader that reads file stored in memory for testing purposes.
	/// </summary>
	internal class FakeTextFileReader : ITextFileReader
	{
		/// <summary>
		/// Newline string representations in files.
		/// </summary>
		private static class NewLine
		{
			public const string NonUnix = "\r\n";
			public const string Unix = "\n";
		}

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fileContents">
		/// String representing file contents to be stored in memory.
		/// </param>
		public FakeTextFileReader(string fileContents)
		{
			if (string.IsNullOrWhiteSpace(fileContents))
				throw new ArgumentException(ExceptionMessages.InvalidFileContents, "fileContents");

			CurrentLineNumber = 0;

			SplitContents(fileContents);
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public void Dispose()
		{
			// Nothing to dispose.
		}

		/// <summary>
		/// Determine whether there is text to read from file; that is, end of file not reached.
		/// </summary>
		/// <returns>
		/// True if there is text to read from file, false otherwise.
		/// </returns>
		public bool HasText()
		{
			return FileLines != null && CurrentLineNumber < FileLines.Length;
		}

		/// <summary>
		/// Read current line in text file.
		/// </summary>
		/// <returns>
		/// String representing current line in text file.
		/// </returns>
		public string ReadLine()
		{
			if (CurrentLineNumber == FileLines.Length)
				return null;

			return FileLines[CurrentLineNumber++];
		}

		/// <summary>
		/// Split given file contents into separate lines.
		/// </summary>
		/// <param name="fileContents">
		/// String representing file contents.
		/// </param>
		private void SplitContents(string fileContents)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fileContents), "Invalid file contents.");

			FileLines = fileContents.Split(new string[] { NewLine.NonUnix, NewLine.Unix }, StringSplitOptions.None);
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get/set current line number in file.
		/// </summary>
		/// <value>
		/// Integer representing current line number in file.
		/// </value>
		private int CurrentLineNumber { get; set; }

		/// <summary>
		/// Get/set separated lines in file.
		/// </summary>
		/// <value>
		/// Array of strings representing separated lines in file.
		/// </value>
		private string[] FileLines { get; set; }
	}
}
