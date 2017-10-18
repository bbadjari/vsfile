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
using NUnit.Framework;
using VSFile.Tests.Category;
using VSFile.Tests.Fake;

namespace VSFile.Tests.Unit
{
	/// <summary>
	/// Unit tests for FakeTextFileReader class.
	/// </summary>
	[TestFixture, Unit]
	public class FakeTextFileReaderTests
	{
		/// <summary>
		/// Fake text file for testing purposes.
		/// </summary>
		private static class FakeTextFile
		{
			public const string EmptyLine = "";
			public const string Line1 = "Line One";
			public const string Line2 = "Line Two";
			public const string Line3 = "Line Three";
		}

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Test constructor specifying empty file contents.
		/// </summary>
		[Test]
		public void WithEmptyFileContents()
		{
			const string EmptyFileContents = "";

			Assert.Throws<ArgumentException>(() => new FakeTextFileReader(EmptyFileContents));
		}

		/// <summary>
		/// Test constructor specifying no file contents.
		/// </summary>
		[Test]
		public void WithNoFileContents()
		{
			const string NoFileContents = null;

			Assert.Throws<ArgumentException>(() => new FakeTextFileReader(NoFileContents));
		}

		/// <summary>
		/// Test constructor specifying white-space file contents.
		/// </summary>
		[Test]
		public void WithWhiteSpaceFileContents()
		{
			const string WhiteSpaceFileContents = " ";

			Assert.Throws<ArgumentException>(() => new FakeTextFileReader(WhiteSpaceFileContents));
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Test ReadLine() method specifying file contents containing one line.
		/// </summary>
		[Test]
		public void ReadLineWithOneLineFileContents()
		{
			const string FileContents = FakeTextFile.Line1;

			FakeTextFileReader reader = new FakeTextFileReader(FileContents);

			Assert.AreEqual(FakeTextFile.Line1, reader.ReadLine());

			Assert.IsNull(reader.ReadLine(), "End of file not reached.");
		}

		/// <summary>
		/// Test ReadLine() method specifying file contents containing three lines.
		/// </summary>
		[Test]
		public void ReadLineWithThreeLineFileContents()
		{
			// Should be treated as constants.
			string FileContents = FakeTextFile.Line1 + Environment.NewLine + FakeTextFile.Line2 + Environment.NewLine + FakeTextFile.Line3;

			FakeTextFileReader reader = new FakeTextFileReader(FileContents);

			Assert.AreEqual(FakeTextFile.Line1, reader.ReadLine());
			Assert.AreEqual(FakeTextFile.Line2, reader.ReadLine());
			Assert.AreEqual(FakeTextFile.Line3, reader.ReadLine());

			Assert.IsNull(reader.ReadLine(), "End of file not reached.");
		}

		/// <summary>
		/// Test ReadLine() method specifying file contents containing three lines with second line empty.
		/// </summary>
		[Test]
		public void ReadLineWithThreeLineFileContentsSecondLineEmpty()
		{
			// Should be treated as constants.
			string FileContents = FakeTextFile.Line1 + Environment.NewLine + FakeTextFile.EmptyLine + Environment.NewLine + FakeTextFile.Line3;

			FakeTextFileReader reader = new FakeTextFileReader(FileContents);

			Assert.AreEqual(FakeTextFile.Line1, reader.ReadLine());
			Assert.AreEqual(FakeTextFile.EmptyLine, reader.ReadLine());
			Assert.AreEqual(FakeTextFile.Line3, reader.ReadLine());

			Assert.IsNull(reader.ReadLine(), "End of file not reached.");
		}

		/// <summary>
		/// Test ReadLine() method specifying file contents containing two lines.
		/// </summary>
		[Test]
		public void ReadLineWithTwoLineFileContents()
		{
			// Should be treated as constants.
			string FileContents = FakeTextFile.Line1 + Environment.NewLine + FakeTextFile.Line2;

			FakeTextFileReader reader = new FakeTextFileReader(FileContents);

			Assert.AreEqual(FakeTextFile.Line1, reader.ReadLine());
			Assert.AreEqual(FakeTextFile.Line2, reader.ReadLine());

			Assert.IsNull(reader.ReadLine(), "End of file not reached.");
		}

		/// <summary>
		/// Test ReadLine() method specifying file contents containing two lines with first line empty.
		/// </summary>
		[Test]
		public void ReadLineWithTwoLineFileContentsFirstLineEmpty()
		{
			// Should be treated as constants.
			string FileContents = FakeTextFile.EmptyLine + Environment.NewLine + FakeTextFile.Line2;

			FakeTextFileReader reader = new FakeTextFileReader(FileContents);

			Assert.AreEqual(FakeTextFile.EmptyLine, reader.ReadLine());
			Assert.AreEqual(FakeTextFile.Line2, reader.ReadLine());

			Assert.IsNull(reader.ReadLine(), "End of file not reached.");
		}

		/// <summary>
		/// Test ReadLine() method specifying file contents containing two lines with second line empty.
		/// </summary>
		[Test]
		public void ReadLineWithTwoLineFileContentsSecondLineEmpty()
		{
			// Should be treated as constants.
			string FileContents = FakeTextFile.Line1 + Environment.NewLine + FakeTextFile.EmptyLine;

			FakeTextFileReader reader = new FakeTextFileReader(FileContents);

			Assert.AreEqual(FakeTextFile.Line1, reader.ReadLine());
			Assert.AreEqual(FakeTextFile.EmptyLine, reader.ReadLine());

			Assert.IsNull(reader.ReadLine(), "End of file not reached.");
		}
	}
}
