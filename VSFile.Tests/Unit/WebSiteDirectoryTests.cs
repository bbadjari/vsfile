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
using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;
using VSFile.Project;
using VSFile.Source;
using VSFile.System;

namespace VSFile.Tests.Unit
{
	/// <summary>
	/// Unit tests for WebSiteDirectory class.
	/// </summary>
	[TestFixture]
	public class WebSiteDirectoryTests
	{
		private const string BasicSourceSearchPattern = "*.vb";

		private const string CSharpSourceFileName = "CSharpSourceFile.cs";

		private const string CSharpSourceSearchPattern = "*.cs";

		private const string DirectoryPath = ".";

		private const string Name = "WebSite";

		////////////////////////////////////////////////////////////////////////
		// Helper Methods

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public void BeforeTest()
		{
			Mock<IFileSystem> mockFileSystem = new Mock<IFileSystem>();

			mockFileSystem.Setup(fileSystem => fileSystem.DirectoryExists(DirectoryPath)).Returns(true);
			mockFileSystem.Setup(fileSystem => fileSystem.GetCurrentDirectory()).Returns(DirectoryPath);
			mockFileSystem.Setup(fileSystem => fileSystem.GetFiles(DirectoryPath, BasicSourceSearchPattern, SearchOption.AllDirectories)).Returns(new string[] { });
			mockFileSystem.Setup(fileSystem => fileSystem.GetFiles(DirectoryPath, CSharpSourceSearchPattern, SearchOption.AllDirectories)).Returns(new string[] { CSharpSourceFileName });

			WebSiteDirectory = new WebSiteDirectory(Name, DirectoryPath, mockFileSystem.Object);
		}

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Test constructor specifying empty directory path.
		/// </summary>
		[Test]
		public void WithEmptyDirectoryPath()
		{
			const string EmptyDirectoryPath = "";

			Assert.Throws<ArgumentException>(() => new WebSiteDirectory(Name, EmptyDirectoryPath));
		}

		/// <summary>
		/// Test constructor specifying empty web site name.
		/// </summary>
		[Test]
		public void WithEmptyName()
		{
			const string EmptyName = "";

			Assert.Throws<ArgumentException>(() => new WebSiteDirectory(EmptyName, DirectoryPath));
		}

		/// <summary>
		/// Test constructor specifying no directory path.
		/// </summary>
		[Test]
		public void WithNoDirectoryPath()
		{
			const string NoDirectoryPath = null;

			Assert.Throws<ArgumentException>(() => new WebSiteDirectory(Name, NoDirectoryPath));
		}

		/// <summary>
		/// Test constructor specifying no web site name.
		/// </summary>
		[Test]
		public void WithNoName()
		{
			const string NoName = null;

			Assert.Throws<ArgumentException>(() => new WebSiteDirectory(NoName, DirectoryPath));
		}

		/// <summary>
		/// Test constructor specifying white-space directory path.
		/// </summary>
		[Test]
		public void WithWhiteSpaceDirectoryPath()
		{
			const string WhiteSpaceDirectoryPath = " ";

			Assert.Throws<ArgumentException>(() => new WebSiteDirectory(Name, WhiteSpaceDirectoryPath));
		}

		/// <summary>
		/// Test constructor specifying white-space web site name.
		/// </summary>
		[Test]
		public void WithWhiteSpaceName()
		{
			const string WhiteSpaceName = " ";

			Assert.Throws<ArgumentException>(() => new WebSiteDirectory(WhiteSpaceName, DirectoryPath));
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

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Test BasicSourceFiles property when web site directory loaded.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenDirectoryLoaded()
		{
			WebSiteDirectory.Load();

			CollectionAssert.IsEmpty(WebSiteDirectory.BasicSourceFiles);
		}

		/// <summary>
		/// Test BasicSourceFiles property when web site directory not loaded.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenDirectoryNotLoaded()
		{
			CollectionAssert.IsEmpty(WebSiteDirectory.BasicSourceFiles);
		}

		/// <summary>
		/// Test CSharpSourceFiles property when web site directory loaded.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenDirectoryLoaded()
		{
			WebSiteDirectory.Load();

			CollectionAssert.IsNotEmpty(WebSiteDirectory.CSharpSourceFiles);

			IEnumerator<CSharpSourceFile> sourceFileEnumerator = WebSiteDirectory.CSharpSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(CSharpSourceFileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test CSharpSourceFiles property when web site directory not loaded.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenDirectoryNotLoaded()
		{
			CollectionAssert.IsEmpty(WebSiteDirectory.CSharpSourceFiles);
		}

		/// <summary>
		/// Test DirectoryPath property.
		/// </summary>
		[Test]
		public void DirectoryPathProperty()
		{
			Assert.AreEqual(DirectoryPath, WebSiteDirectory.DirectoryPath);
		}

		/// <summary>
		/// Test Name property.
		/// </summary>
		[Test]
		public void NameProperty()
		{
			Assert.AreEqual(Name, WebSiteDirectory.Name);
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set ASP.NET web site directory.
		/// </summary>
		/// <value>
		/// WebSiteDirectory representing ASP.NET web site directory.
		/// </value>
		private WebSiteDirectory WebSiteDirectory { get; set; }
	}
}
