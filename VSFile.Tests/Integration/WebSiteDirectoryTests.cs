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

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using VSFile.Project;
using VSFile.Source;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Integration
{
	/// <summary>
	/// Integration tests for WebSiteDirectory class.
	/// </summary>
	public class WebSiteDirectoryTests : DirectoryIntegrationTestFixture
	{
		private const string AppCodeDirectoryName = "App_Code";

		private const string Name = "WebSite";

		////////////////////////////////////////////////////////////////////////
		// Helper Methods

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public override void BeforeTest()
		{
			base.BeforeTest();

			SubdirectoryPath = Path.Combine(Name, AppCodeDirectoryName);

			FullDirectoryPath = Path.Combine(DirectoryPath, SubdirectoryPath);

			Directory.CreateDirectory(FullDirectoryPath);

			WebSiteDirectory = new WebSiteDirectory(Name, Path.Combine(DirectoryPath, Name));
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Test Load() method.
		/// </summary>
		[Test]
		public void Load()
		{
			Assert.DoesNotThrow(() => WebSiteDirectory.Load());
		}

		/// <summary>
		/// Test Load() method with non-existent directory path.
		/// </summary>
		[Test]
		public void LoadWithNonExistentDirectoryPath()
		{
			const string NonExistentDirectoryPath = "NonExistentDirectory";

			WebSiteDirectory = new WebSiteDirectory(Name, NonExistentDirectoryPath);

			Assert.Throws<DirectoryNotFoundException>(() => WebSiteDirectory.Load());
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Test BasicSourceFiles property when Visual Basic source file exists.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenFileExists()
		{
			const string FileName = "BasicSourceFile.vb";

			WriteFile(SubdirectoryPath, FileName, EmbeddedFiles.BasicSourceFile);

			WebSiteDirectory.Load();

			CollectionAssert.IsNotEmpty(WebSiteDirectory.BasicSourceFiles);

			IEnumerator<BasicSourceFile> sourceFileEnumerator = WebSiteDirectory.BasicSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(FileName, sourceFileEnumerator.Current.FileName);
			Assert.AreEqual(FullDirectoryPath, sourceFileEnumerator.Current.DirectoryPath);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test BasicSourceFiles property when Visual Basic source file does not exist.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenFileNotExists()
		{
			WebSiteDirectory.Load();

			CollectionAssert.IsEmpty(WebSiteDirectory.BasicSourceFiles);
		}

		/// <summary>
		/// Test CSharpSourceFiles property when Visual C# source file exists.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenFileExists()
		{
			const string FileName = "CSharpSourceFile.cs";

			WriteFile(SubdirectoryPath, FileName, EmbeddedFiles.CSharpSourceFile);

			WebSiteDirectory.Load();

			CollectionAssert.IsNotEmpty(WebSiteDirectory.CSharpSourceFiles);

			IEnumerator<CSharpSourceFile> sourceFileEnumerator = WebSiteDirectory.CSharpSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(FileName, sourceFileEnumerator.Current.FileName);
			Assert.AreEqual(FullDirectoryPath, sourceFileEnumerator.Current.DirectoryPath);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test CSharpSourceFiles property when Visual C# source file does not exist.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenFileNotExists()
		{
			WebSiteDirectory.Load();

			CollectionAssert.IsEmpty(WebSiteDirectory.CSharpSourceFiles);
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set full directory path.
		/// </summary>
		/// <value>
		/// String representing full directory path.
		/// </value>
		private string FullDirectoryPath { get; set; }

		/// <summary>
		/// Get/set subdirectory path.
		/// </summary>
		/// <value>
		/// String representing path to subdirectory in directory.
		/// </value>
		private string SubdirectoryPath { get; set; }

		/// <summary>
		/// Get/set ASP.NET web site directory.
		/// </summary>
		/// <value>
		/// WebSiteDirectory representing ASP.NET web site directory.
		/// </value>
		private WebSiteDirectory WebSiteDirectory { get; set; }
	}
}
