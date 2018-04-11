////////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Bernard Badjari
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
using NUnit.Framework;
using VSFile.Tests.Category;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Integration
{
	/// <summary>
	/// Represents an integration test fixture for single file.
	/// </summary>
	[TestFixture, Integration]
	public abstract class FileIntegrationTestFixture
	{
		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fileName">
		/// String representing file name.
		/// </param>
		/// <param name="fileContents">
		/// String representing file contents.
		/// </param>
		protected FileIntegrationTestFixture(string fileName, string fileContents)
		{
			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentException(ExceptionMessages.InvalidFileName, "fileName");

			if (string.IsNullOrWhiteSpace(fileContents))
				throw new ArgumentException(ExceptionMessages.InvalidFileContents, "fileContents");

			FileContents = fileContents;
			FileName = fileName;
			FilePath = Path.Combine(Path.GetTempPath(), fileName);
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Called after each test executed.
		/// </summary>
		[TearDown]
		public virtual void AfterTest()
		{
			File.Delete(FilePath);
		}

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public virtual void BeforeTest()
		{
			File.WriteAllText(FilePath, FileContents);
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get file path.
		/// </summary>
		/// <value>
		/// String representing file path.
		/// </value>
		protected string FilePath { get; private set; }

		/// <summary>
		/// Get/set file contents.
		/// </summary>
		/// <value>
		/// String representing file contents.
		/// </value>
		private string FileContents { get; set; }

		/// <summary>
		/// Get/set file name.
		/// </summary>
		/// <value>
		/// String representing file name.
		/// </value>
		private string FileName { get; set; }
	}
}
