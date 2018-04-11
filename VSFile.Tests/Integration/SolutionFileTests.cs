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

using System.Collections.Generic;
using NUnit.Framework;
using VSFile.Project;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Integration
{
	/// <summary>
	/// Integration tests for SolutionFile class.
	/// </summary>
	public class SolutionFileTests : IntegrationTestFixture
	{
		private const string FileName = "SolutionFile.sln";

		////////////////////////////////////////////////////////////////////////
		// Helper Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public SolutionFileTests()
			: base(FileName, EmbeddedFiles.SolutionFile)
		{
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Methods

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		public override void BeforeTest()
		{
			base.BeforeTest();

			SolutionFile = new SolutionFile(FilePath);
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Test Load() method.
		/// </summary>
		[Test]
		public void Load()
		{
			Assert.DoesNotThrow(() => SolutionFile.Load());
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Test BasicProjectFiles property when solution file loaded.
		/// </summary>
		[Test]
		public void BasicProjectFilesWhenFileLoaded()
		{
			const string ProjectName = "BasicProjectFile";

			SolutionFile.Load();

			CollectionAssert.IsNotEmpty(SolutionFile.BasicProjectFiles);

			IEnumerator<BasicProjectFile> projectFileEnumerator = SolutionFile.BasicProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(ProjectName, projectFileEnumerator.Current.ProjectName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test CSharpProjectFiles property when solution file loaded.
		/// </summary>
		[Test]
		public void CSharpProjectFilesWhenFileLoaded()
		{
			const string ProjectName = "CSharpProjectFile";

			SolutionFile.Load();

			CollectionAssert.IsNotEmpty(SolutionFile.CSharpProjectFiles);

			IEnumerator<CSharpProjectFile> projectFileEnumerator = SolutionFile.CSharpProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(ProjectName, projectFileEnumerator.Current.ProjectName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test FSharpProjectFiles property when solution file loaded.
		/// </summary>
		[Test]
		public void FSharpProjectFilesWhenFileLoaded()
		{
			const string ProjectName = "FSharpProjectFile";

			SolutionFile.Load();

			CollectionAssert.IsNotEmpty(SolutionFile.FSharpProjectFiles);

			IEnumerator<FSharpProjectFile> projectFileEnumerator = SolutionFile.FSharpProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(ProjectName, projectFileEnumerator.Current.ProjectName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test WebSiteDirectories property when solution file loaded.
		/// </summary>
		[Test]
		public void WebSiteDirectoriesWhenFileLoaded()
		{
			const string WebSiteName = "WebSite";

			SolutionFile.Load();

			CollectionAssert.IsNotEmpty(SolutionFile.WebSiteDirectories);

			IEnumerator<WebSiteDirectory> webSiteDirectoryEnumerator = SolutionFile.WebSiteDirectories.GetEnumerator();

			// Ensure web site directory exists.
			Assert.IsTrue(webSiteDirectoryEnumerator.MoveNext());

			Assert.AreEqual(WebSiteName, webSiteDirectoryEnumerator.Current.Name);

			// Ensure no more web site directories exist.
			Assert.IsFalse(webSiteDirectoryEnumerator.MoveNext());
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set solution file.
		/// </summary>
		/// <value>
		/// SolutionFile representing solution file.
		/// </value>
		private SolutionFile SolutionFile { get; set; }
	}
}
