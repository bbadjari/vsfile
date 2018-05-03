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
	/// Represents an integration test fixture for single directory.
	/// </summary>
	[TestFixture, Integration]
	public abstract class DirectoryIntegrationTestFixture
	{
		private const string DirectoryName = "VSFile.Tests.Integration";

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public DirectoryIntegrationTestFixture()
		{
			DirectoryPath = Path.Combine(Path.GetTempPath(), DirectoryName);
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Called after each test executed.
		/// </summary>
		[TearDown]
		public virtual void AfterTest()
		{
			Directory.Delete(DirectoryPath, true);
		}

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public virtual void BeforeTest()
		{
			Directory.CreateDirectory(DirectoryPath);
		}

		/// <summary>
		/// Write file with given file name and file contents to directory.
		/// </summary>
		/// <param name="fileName">
		/// String representing file name.
		/// </param>
		/// <param name="fileContents">
		/// String representing file contents.
		/// </param>
		protected void WriteFile(string fileName, string fileContents)
		{
			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentException(ExceptionMessages.InvalidFileName, "fileName");

			if (string.IsNullOrWhiteSpace(fileContents))
				throw new ArgumentException(ExceptionMessages.InvalidFileContents, "fileContents");

			File.WriteAllText(Path.Combine(DirectoryPath, fileName), fileContents);
		}

		/// <summary>
		/// Write file with given file name and file contents to given subdirectory path in directory.
		/// </summary>
		/// <param name="subdirectoryPath">
		/// String representing path to subdirectory in directory.
		/// </param>
		/// <param name="fileName">
		/// String representing file name.
		/// </param>
		/// <param name="fileContents">
		/// String representing file contents.
		/// </param>
		protected void WriteFile(string subdirectoryPath, string fileName, string fileContents)
		{
			if (string.IsNullOrWhiteSpace(subdirectoryPath))
				throw new ArgumentException(ExceptionMessages.InvalidDirectoryPath, "subdirectoryPath");

			fileName = Path.Combine(subdirectoryPath, fileName);

			WriteFile(fileName, fileContents);
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get directory path.
		/// </summary>
		/// <value>
		/// String representing directory path.
		/// </value>
		protected string DirectoryPath { get; private set; }
	}
}
