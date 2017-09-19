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
	/// Unit tests for CSharpProjectFile class.
	/// </summary>
	[TestFixture]
	public class CSharpProjectFileTests
	{
		private const string DirectoryPath = "";

		private const string FileExtension = ".csproj";

		private const string FileNameNoExtension = "CSharpProjectFile";

		private const string FilePath = DirectoryPath + FileNameNoExtension + FileExtension;

		private const string ProjectName = "CSharpProjectFile";

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

			CSharpProjectFile = new CSharpProjectFile(ProjectName, FilePath, new FakeXmlFileReader(EmbeddedFiles.CSharpProjectFile), MockFileSystem.Object);
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

			Assert.Throws<ArgumentException>(() => new CSharpProjectFile(EmptyFilePath));
		}

		/// <summary>
		/// Test constructor specifying empty project name.
		/// </summary>
		[Test]
		public void WithEmptyProjectName()
		{
			const string EmptyProjectName = "";

			Assert.DoesNotThrow(() => new CSharpProjectFile(EmptyProjectName, FilePath));
		}

		/// <summary>
		/// Test constructor specifying no file path.
		/// </summary>
		[Test]
		public void WithNoFilePath()
		{
			const string NoFilePath = null;

			Assert.Throws<ArgumentException>(() => new CSharpProjectFile(NoFilePath));
		}

		/// <summary>
		/// Test constructor specifying no project name.
		/// </summary>
		[Test]
		public void WithNoProjectName()
		{
			const string NoProjectName = null;

			Assert.DoesNotThrow(() => new CSharpProjectFile(NoProjectName, FilePath));
		}

		/// <summary>
		/// Test constructor specifying white-space file path.
		/// </summary>
		[Test]
		public void WithWhiteSpaceFilePath()
		{
			const string WhiteSpaceFilePath = " ";

			Assert.Throws<ArgumentException>(() => new CSharpProjectFile(WhiteSpaceFilePath));
		}

		/// <summary>
		/// Test constructor specifying white-space project name.
		/// </summary>
		[Test]
		public void WithWhiteSpaceProjectName()
		{
			const string WhiteSpaceProjectName = " ";

			Assert.DoesNotThrow(() => new CSharpProjectFile(WhiteSpaceProjectName, FilePath));
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Test Load() method.
		/// </summary>
		[Test]
		public void Load()
		{
			Assert.DoesNotThrow(() => CSharpProjectFile.Load());
		}

		/// <summary>
		/// Test Load() method with non-existent file path.
		/// </summary>
		[Test]
		public void LoadWithNonExistentFilePath()
		{
			const string NonExistentFilePath = "NonExistentFile.xxx";

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(NonExistentFilePath)).Returns(false);

			CSharpProjectFile = new CSharpProjectFile(ProjectName, NonExistentFilePath, new FakeXmlFileReader(EmbeddedFiles.CSharpProjectFile), MockFileSystem.Object);

			Assert.Throws<FileNotFoundException>(() => CSharpProjectFile.Load());
		}

		/// <summary>
		/// Test Load() method with wrong file extension.
		/// </summary>
		[Test]
		public void LoadWithWrongFileExtension()
		{
			const string FilePathWithWrongFileExtension = "CSharpProjectFile.xxx";

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(FilePathWithWrongFileExtension)).Returns(true);

			CSharpProjectFile = new CSharpProjectFile(ProjectName, FilePathWithWrongFileExtension, new FakeXmlFileReader(EmbeddedFiles.CSharpProjectFile), MockFileSystem.Object);

			Assert.Throws<IOException>(() => CSharpProjectFile.Load());
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Test DirectoryPath property.
		/// </summary>
		[Test]
		public void DirectoryPathProperty()
		{
			Assert.AreEqual(DirectoryPath, CSharpProjectFile.DirectoryPath);
		}

		/// <summary>
		/// Test FileExtension property.
		/// </summary>
		[Test]
		public void FileExtensionProperty()
		{
			Assert.AreEqual(FileExtension, CSharpProjectFile.FileExtension);
		}

		/// <summary>
		/// Test FileName property.
		/// </summary>
		[Test]
		public void FileNameProperty()
		{
			Assert.AreEqual(FilePath, CSharpProjectFile.FileName);
		}

		/// <summary>
		/// Test FileNameNoExtension property.
		/// </summary>
		[Test]
		public void FileNameNoExtensionProperty()
		{
			Assert.AreEqual(FileNameNoExtension, CSharpProjectFile.FileNameNoExtension);
		}

		/// <summary>
		/// Test FilePath property.
		/// </summary>
		[Test]
		public void FilePathProperty()
		{
			Assert.AreEqual(FilePath, CSharpProjectFile.FilePath);
		}

		/// <summary>
		/// Test ProjectName property.
		/// </summary>
		[Test]
		public void ProjectNameProperty()
		{
			Assert.AreEqual(ProjectName, CSharpProjectFile.ProjectName);
		}

		/// <summary>
		/// Test SourceFileExtension property.
		/// </summary>
		[Test]
		public void SourceFileExtensionProperty()
		{
			Assert.AreEqual(CSharpSourceFile.SourceFileExtension, CSharpProjectFile.SourceFileExtension);
		}

		/// <summary>
		/// Test SourceFiles property when project file loaded.
		/// </summary>
		[Test]
		public void SourceFilesWhenFileLoaded()
		{
			const string SourceFileName = "CSharpSourceFile.cs";

			CSharpProjectFile.Load();

			CollectionAssert.IsNotEmpty(CSharpProjectFile.SourceFiles);

			IEnumerator<CSharpSourceFile> sourceFileEnumerator = CSharpProjectFile.SourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(SourceFileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist and any auto-generated files ignored.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test SourceFiles property when project file not loaded.
		/// </summary>
		[Test]
		public void SourceFilesWhenFileNotLoaded()
		{
			CollectionAssert.IsEmpty(CSharpProjectFile.SourceFiles);
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set Visual C# project file.
		/// </summary>
		/// <value>
		/// CSharpProjectFile representing Visual C# project file.
		/// </value>
		private CSharpProjectFile CSharpProjectFile { get; set; }

		/// <summary>
		/// Get/set mock file system.
		/// </summary>
		/// <value>
		/// Mock<IFileSystem> representing mock file system.
		/// </value>
		private Mock<IFileSystem> MockFileSystem { get; set; }
	}
}
