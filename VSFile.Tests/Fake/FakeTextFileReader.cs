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
using VSFile.System;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Fake
{
	/// <summary>
	/// Fake implementation of text file reader that reads file stored in memory for testing purposes.
	/// </summary>
	internal class FakeTextFileReader : ITextFileReader
	{
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

			FileContents = fileContents;
			ReadStartIndex = 0;
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
		/// Read current line in text file.
		/// </summary>
		/// <returns>
		/// String representing current line in text file.
		/// </returns>
		public string ReadLine()
		{
			if (ReadStartIndex > FileContents.Length)
				return null;

			int newLineIndex = FileContents.IndexOf(Environment.NewLine, ReadStartIndex);

			int readLength = newLineIndex >= 0 ? newLineIndex - ReadStartIndex : FileContents.Length - ReadStartIndex;

			string currentLine = FileContents.Substring(ReadStartIndex, readLength);

			ReadStartIndex += readLength + Environment.NewLine.Length;

			return currentLine;
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get/set file contents.
		/// </summary>
		/// <value>
		/// String representing file contents.
		/// </value>
		private string FileContents { get; set; }

		/// <summary>
		/// Get/set index in file contents to start reading from.
		/// </summary>
		/// <value>
		/// Integer representing index in file contents to start reading from.
		/// </value>
		private int ReadStartIndex { get; set; }
	}
}
