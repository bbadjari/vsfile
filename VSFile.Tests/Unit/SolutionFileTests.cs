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
using VSFile.System;
using VSFile.Tests.Fake;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Unit
{
	/// <summary>
	/// Unit tests for SolutionFile class.
	/// </summary>
	[TestFixture]
	public class SolutionFileTests
	{
		private const string DirectoryPath = "";

		private const string FileExtension = ".sln";

		private const string FileNameNoExtension = "SolutionFile";

		private const string FilePath = DirectoryPath + FileNameNoExtension + FileExtension;

		////////////////////////////////////////////////////////////////////////
		// Helper Methods

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public void BeforeTest()
		{
			MockFileSystem = new Mock<IFileSystem>();
			MockTextFileReaderFactory = new Mock<ITextFileReaderFactory>();

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(FilePath)).Returns(true);
			MockFileSystem.Setup(fileSystem => fileSystem.GetCurrentDirectory()).Returns(DirectoryPath);

			MockTextFileReaderFactory.Setup(factory => factory.Create(It.IsAny<string>())).Returns(new FakeTextFileReader(EmbeddedFiles.SolutionFile));

			SolutionFile = new SolutionFile(FilePath, MockFileSystem.Object, MockTextFileReaderFactory.Object);
		}

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Test constructor specifying empty file path.
		/// </summary>
		[Test]
		public void WithEmptyFilePath()
		{
			const string EmptyFilePath = "";

			Assert.Throws<ArgumentException>(() => new SolutionFile(EmptyFilePath));
		}

		/// <summary>
		/// Test constructor specifying no file path.
		/// </summary>
		[Test]
		public void WithNoFilePath()
		{
			const string NoFilePath = null;

			Assert.Throws<ArgumentException>(() => new SolutionFile(NoFilePath));
		}

		/// <summary>
		/// Test constructor specifying white-space file path.
		/// </summary>
		[Test]
		public void WithWhiteSpaceFilePath()
		{
			const string WhiteSpaceFilePath = " ";

			Assert.Throws<ArgumentException>(() => new SolutionFile(WhiteSpaceFilePath));
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

		/// <summary>
		/// Test Load() method with non-existent file path.
		/// </summary>
		[Test]
		public void LoadWithNonExistentFilePath()
		{
			const string NonExistentFilePath = "NonExistentFile.xxx";

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(NonExistentFilePath)).Returns(false);

			SolutionFile = new SolutionFile(NonExistentFilePath, MockFileSystem.Object, MockTextFileReaderFactory.Object);

			Assert.Throws<FileNotFoundException>(() => SolutionFile.Load());
		}

		/// <summary>
		/// Test Load() method with wrong file extension.
		/// </summary>
		[Test]
		public void LoadWithWrongFileExtension()
		{
			const string FilePathWithWrongFileExtension = "SolutionFile.xxx";

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(FilePathWithWrongFileExtension)).Returns(true);

			SolutionFile = new SolutionFile(FilePathWithWrongFileExtension, MockFileSystem.Object, MockTextFileReaderFactory.Object);

			Assert.Throws<IOException>(() => SolutionFile.Load());
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
		/// Test BasicProjectFiles property when solution file not loaded.
		/// </summary>
		[Test]
		public void BasicProjectFilesWhenFileNotLoaded()
		{
			CollectionAssert.IsEmpty(SolutionFile.BasicProjectFiles);
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
		/// Test CSharpProjectFiles property when solution file not loaded.
		/// </summary>
		[Test]
		public void CSharpProjectFilesWhenFileNotLoaded()
		{
			CollectionAssert.IsEmpty(SolutionFile.CSharpProjectFiles);
		}

		/// <summary>
		/// Test DirectoryPath property.
		/// </summary>
		[Test]
		public void DirectoryPathProperty()
		{
			Assert.AreEqual(DirectoryPath, SolutionFile.DirectoryPath);
		}

		/// <summary>
		/// Test FileExtension property.
		/// </summary>
		[Test]
		public void FileExtensionProperty()
		{
			Assert.AreEqual(FileExtension, SolutionFile.FileExtension);
		}

		/// <summary>
		/// Test FileName property.
		/// </summary>
		[Test]
		public void FileNameProperty()
		{
			Assert.AreEqual(FilePath, SolutionFile.FileName);
		}

		/// <summary>
		/// Test FileNameNoExtension property.
		/// </summary>
		[Test]
		public void FileNameNoExtensionProperty()
		{
			Assert.AreEqual(FileNameNoExtension, SolutionFile.FileNameNoExtension);
		}

		/// <summary>
		/// Test FilePath property.
		/// </summary>
		[Test]
		public void FilePathProperty()
		{
			Assert.AreEqual(FilePath, SolutionFile.FilePath);
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
		/// Test FSharpProjectFiles property when solution file not loaded.
		/// </summary>
		[Test]
		public void FSharpProjectFilesWhenFileNotLoaded()
		{
			CollectionAssert.IsEmpty(SolutionFile.FSharpProjectFiles);
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

		/// <summary>
		/// Test WebSiteDirectories property when solution file not loaded.
		/// </summary>
		[Test]
		public void WebSiteDirectoriesWhenFileNotLoaded()
		{
			CollectionAssert.IsEmpty(SolutionFile.WebSiteDirectories);
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set mock file system.
		/// </summary>
		/// <value>
		/// Mock<IFileSystem> representing mock file system.
		/// </value>
		private Mock<IFileSystem> MockFileSystem { get; set; }

		/// <summary>
		/// Get/set mock text file reader factory.
		/// </summary>
		/// <value>
		/// Mock<ITextFileReaderFactory> representing mock text file reader factory.
		/// </value>
		private Mock<ITextFileReaderFactory> MockTextFileReaderFactory { get; set; }

		/// <summary>
		/// Get/set solution file.
		/// </summary>
		/// <value>
		/// SolutionFile representing solution file.
		/// </value>
		private SolutionFile SolutionFile { get; set; }
	}
}
