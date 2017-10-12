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
using VSFile.Tests.Fake;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Unit
{
	/// <summary>
	/// Unit tests for FSharpProjectFile class.
	/// </summary>
	[TestFixture]
	public class FSharpProjectFileTests
	{
		private const string DirectoryPath = "";

		private const string FileExtension = ".fsproj";

		private const string FileNameNoExtension = "FSharpProjectFile";

		private const string FilePath = DirectoryPath + FileNameNoExtension + FileExtension;

		private const string ProjectName = "FSharpProjectFile";

		////////////////////////////////////////////////////////////////////////
		// Helper Methods

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public void BeforeTest()
		{
			MockFileSystem = new Mock<IFileSystem>();

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(FilePath)).Returns(true);
			MockFileSystem.Setup(fileSystem => fileSystem.GetCurrentDirectory()).Returns(DirectoryPath);

			FSharpProjectFile = new FSharpProjectFile(ProjectName, FilePath, new FakeXmlFileReader(EmbeddedFiles.FSharpProjectFile), MockFileSystem.Object);
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

			Assert.Throws<ArgumentException>(() => new FSharpProjectFile(EmptyFilePath));
		}

		/// <summary>
		/// Test constructor specifying empty project name.
		/// </summary>
		[Test]
		public void WithEmptyProjectName()
		{
			const string EmptyProjectName = "";

			Assert.DoesNotThrow(() => new FSharpProjectFile(EmptyProjectName, FilePath));
		}

		/// <summary>
		/// Test constructor specifying no file path.
		/// </summary>
		[Test]
		public void WithNoFilePath()
		{
			const string NoFilePath = null;

			Assert.Throws<ArgumentException>(() => new FSharpProjectFile(NoFilePath));
		}

		/// <summary>
		/// Test constructor specifying no project name.
		/// </summary>
		[Test]
		public void WithNoProjectName()
		{
			const string NoProjectName = null;

			Assert.DoesNotThrow(() => new FSharpProjectFile(NoProjectName, FilePath));
		}

		/// <summary>
		/// Test constructor specifying white-space file path.
		/// </summary>
		[Test]
		public void WithWhiteSpaceFilePath()
		{
			const string WhiteSpaceFilePath = " ";

			Assert.Throws<ArgumentException>(() => new FSharpProjectFile(WhiteSpaceFilePath));
		}

		/// <summary>
		/// Test constructor specifying white-space project name.
		/// </summary>
		[Test]
		public void WithWhiteSpaceProjectName()
		{
			const string WhiteSpaceProjectName = " ";

			Assert.DoesNotThrow(() => new FSharpProjectFile(WhiteSpaceProjectName, FilePath));
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Test Load() method.
		/// </summary>
		[Test]
		public void Load()
		{
			Assert.DoesNotThrow(() => FSharpProjectFile.Load());
		}

		/// <summary>
		/// Test Load() method with non-existent file path.
		/// </summary>
		[Test]
		public void LoadWithNonExistentFilePath()
		{
			const string NonExistentFilePath = "NonExistentFile.xxx";

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(NonExistentFilePath)).Returns(false);

			FSharpProjectFile = new FSharpProjectFile(ProjectName, NonExistentFilePath, new FakeXmlFileReader(EmbeddedFiles.FSharpProjectFile), MockFileSystem.Object);

			Assert.Throws<FileNotFoundException>(() => FSharpProjectFile.Load());
		}

		/// <summary>
		/// Test Load() method with wrong file extension.
		/// </summary>
		[Test]
		public void LoadWithWrongFileExtension()
		{
			const string FilePathWithWrongFileExtension = "FSharpProjectFile.xxx";

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(FilePathWithWrongFileExtension)).Returns(true);

			FSharpProjectFile = new FSharpProjectFile(ProjectName, FilePathWithWrongFileExtension, new FakeXmlFileReader(EmbeddedFiles.FSharpProjectFile), MockFileSystem.Object);

			Assert.Throws<IOException>(() => FSharpProjectFile.Load());
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Test DirectoryPath property.
		/// </summary>
		[Test]
		public void DirectoryPathProperty()
		{
			Assert.AreEqual(DirectoryPath, FSharpProjectFile.DirectoryPath);
		}

		/// <summary>
		/// Test FileExtension property.
		/// </summary>
		[Test]
		public void FileExtensionProperty()
		{
			Assert.AreEqual(FileExtension, FSharpProjectFile.FileExtension);
		}

		/// <summary>
		/// Test FileName property.
		/// </summary>
		[Test]
		public void FileNameProperty()
		{
			Assert.AreEqual(FilePath, FSharpProjectFile.FileName);
		}

		/// <summary>
		/// Test FileNameNoExtension property.
		/// </summary>
		[Test]
		public void FileNameNoExtensionProperty()
		{
			Assert.AreEqual(FileNameNoExtension, FSharpProjectFile.FileNameNoExtension);
		}

		/// <summary>
		/// Test FilePath property.
		/// </summary>
		[Test]
		public void FilePathProperty()
		{
			Assert.AreEqual(FilePath, FSharpProjectFile.FilePath);
		}

		/// <summary>
		/// Test ProjectName property.
		/// </summary>
		[Test]
		public void ProjectNameProperty()
		{
			Assert.AreEqual(ProjectName, FSharpProjectFile.ProjectName);
		}

		/// <summary>
		/// Test SourceFileExtension property.
		/// </summary>
		[Test]
		public void SourceFileExtensionProperty()
		{
			Assert.AreEqual(FSharpSourceFile.SourceFileExtension, FSharpProjectFile.SourceFileExtension);
		}

		/// <summary>
		/// Test SourceFiles property when project file loaded.
		/// </summary>
		[Test]
		public void SourceFilesWhenFileLoaded()
		{
			const string SourceFileName = "FSharpSourceFile.fs";

			FSharpProjectFile.Load();

			CollectionAssert.IsNotEmpty(FSharpProjectFile.SourceFiles);

			IEnumerator<FSharpSourceFile> sourceFileEnumerator = FSharpProjectFile.SourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(SourceFileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test SourceFiles property when project file not loaded.
		/// </summary>
		[Test]
		public void SourceFilesWhenFileNotLoaded()
		{
			CollectionAssert.IsEmpty(FSharpProjectFile.SourceFiles);
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set Visual F# project file.
		/// </summary>
		/// <value>
		/// FSharpProjectFile representing Visual F# project file.
		/// </value>
		private FSharpProjectFile FSharpProjectFile { get; set; }

		/// <summary>
		/// Get/set mock file system.
		/// </summary>
		/// <value>
		/// Mock<IFileSystem> representing mock file system.
		/// </value>
		private Mock<IFileSystem> MockFileSystem { get; set; }
	}
}
