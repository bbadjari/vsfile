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
using System.Diagnostics;
using System.IO;
using Moq;
using NUnit.Framework;
using VSFile.Project;
using VSFile.Source;
using VSFile.System;
using VSFile.Tests.Category;

namespace VSFile.Tests.Unit
{
	/// <summary>
	/// Unit tests for VisualStudioFiles class.
	/// </summary>
	[TestFixture, Unit]
	public class VisualStudioFilesTests
	{
		private const string BasicProjectFileName = "BasicProjectFile.vbproj";

		private const string BasicSourceFileName = "BasicSourceFile.vb";

		private const string CSharpProjectFileName = "CSharpProjectFile.csproj";

		private const string CSharpSourceFileName = "CSharpSourceFile.cs";

		private const string DirectoryPath = ".";

		private const string FSharpProjectFileName = "FSharpProjectFile.fsproj";

		private const string FSharpSourceFileName = "FSharpSourceFile.fs";

		private const string SolutionFileName = "SolutionFile.sln";

		////////////////////////////////////////////////////////////////////////
		// Helper Methods

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public void BeforeTest()
		{
			FilePaths = new List<string>();
			MockFileSystem = new Mock<IFileSystem>();

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(It.IsAny<string>())).Returns(true);
			MockFileSystem.Setup(fileSystem => fileSystem.GetCurrentDirectory()).Returns(DirectoryPath);

			Create();
		}

		/// <summary>
		/// Add Visual Basic project files to file paths and mock file system.
		/// </summary>
		private void AddBasicProjectFiles()
		{
			const string SearchPattern = "*.vbproj";

			AddFiles(SearchPattern, BasicProjectFileName);
		}

		/// <summary>
		/// Add Visual Basic source files to file paths and mock file system.
		/// </summary>
		private void AddBasicSourceFiles()
		{
			const string SearchPattern = "*.vb";

			AddFiles(SearchPattern, BasicSourceFileName);
		}

		/// <summary>
		/// Add Visual C# project files to file paths and mock file system.
		/// </summary>
		private void AddCSharpProjectFiles()
		{
			const string SearchPattern = "*.csproj";

			AddFiles(SearchPattern, CSharpProjectFileName);
		}

		/// <summary>
		/// Add Visual C# source files to file paths and mock file system.
		/// </summary>
		private void AddCSharpSourceFiles()
		{
			const string SearchPattern = "*.cs";

			AddFiles(SearchPattern, CSharpSourceFileName);
		}

		/// <summary>
		/// Add given search pattern and file name to file paths and mock file system.
		/// </summary>
		/// <param name="searchPattern">
		/// String representing search pattern to add.
		/// </param>
		/// <param name="fileName">
		/// String representing file name to add.
		/// </param>
		private void AddFiles(string searchPattern, string fileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(searchPattern), "Invalid search pattern.");
			Debug.Assert(!string.IsNullOrWhiteSpace(fileName), "Invalid file name.");

			FilePaths.Add(searchPattern);

			MockFileSystem.Setup(fileSystem => fileSystem.GetFiles(DirectoryPath, searchPattern, SearchOption.AllDirectories)).Returns(new string[] { fileName });
		}

		/// <summary>
		/// Add Visual F# project files to file paths and mock file system.
		/// </summary>
		private void AddFSharpProjectFiles()
		{
			const string SearchPattern = "*.fsproj";

			AddFiles(SearchPattern, FSharpProjectFileName);
		}

		/// <summary>
		/// Add Visual F# source files to file paths and mock file system.
		/// </summary>
		private void AddFSharpSourceFiles()
		{
			const string SearchPattern = "*.fs";

			AddFiles(SearchPattern, FSharpSourceFileName);
		}

		/// <summary>
		/// Add Visual Studio solution files to file paths and mock file system.
		/// </summary>
		private void AddSolutionFiles()
		{
			const string SearchPattern = "*.sln";

			AddFiles(SearchPattern, SolutionFileName);
		}

		/// <summary>
		/// Create instance of class being tested.
		/// </summary>
		private void Create()
		{
			VisualStudioFiles = new VisualStudioFiles(FilePaths, true, MockFileSystem.Object);
		}

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Test constructor specifying file paths containing empty file path.
		/// </summary>
		[Test]
		public void WithEmptyFilePath()
		{
			const string EmptyFilePath = "";

			// Should be treated as constant.
			string[] filePaths = new string[] { EmptyFilePath };

			Assert.Throws<ArgumentException>(() => new VisualStudioFiles(filePaths));
		}

		/// <summary>
		/// Test constructor specifying empty file paths.
		/// </summary>
		[Test]
		public void WithEmptyFilePaths()
		{
			// Should be treated as constant.
			string[] emptyFilePaths = new string[0];

			Assert.DoesNotThrow(() => new VisualStudioFiles(emptyFilePaths));
		}

		/// <summary>
		/// Test constructor specifying file paths containing no file path.
		/// </summary>
		[Test]
		public void WithNoFilePath()
		{
			const string NoFilePath = null;

			// Should be treated as constant.
			string[] filePaths = new string[] { NoFilePath };

			Assert.Throws<ArgumentException>(() => new VisualStudioFiles(filePaths));
		}

		/// <summary>
		/// Test constructor specifying no file paths.
		/// </summary>
		[Test]
		public void WithNoFilePaths()
		{
			const string[] NoFilePaths = null;

			Assert.Throws<ArgumentNullException>(() => new VisualStudioFiles(NoFilePaths));
		}

		/// <summary>
		/// Test constructor specifying non-existent file with supported file extension.
		/// </summary>
		[Test]
		public void WithNonExistentSupportedFile()
		{
			const string NonExistentFileName = "NonExistentFile.csproj";

			AddFiles(NonExistentFileName, NonExistentFileName);

			MockFileSystem.Setup(fileSystem => fileSystem.FileExists(NonExistentFileName)).Returns(false);

			Assert.Throws<FileNotFoundException>(() => Create());
		}

		/// <summary>
		/// Test constructor specifying non-existent file with unsupported file extension.
		/// </summary>
		[Test]
		public void WithNonExistentUnsupportedFile()
		{
			const string NonExistentFileName = "NonExistentFile.xxx";

			AddFiles(NonExistentFileName, NonExistentFileName);

			Assert.DoesNotThrow(() => Create());
		}

		/// <summary>
		/// Test constructor specifying file paths containing white-space file path.
		/// </summary>
		[Test]
		public void WithWhiteSpaceFilePath()
		{
			const string WhiteSpaceFilePath = " ";

			// Should be treated as constant.
			string[] filePaths = new string[] { WhiteSpaceFilePath };

			Assert.Throws<ArgumentException>(() => new VisualStudioFiles(filePaths));
		}

		/// <summary>
		/// Test constructor specifying file paths containing wildcard directory path.
		/// </summary>
		[Test]
		public void WithWildcardDirectoryPath()
		{
			const string WildcardDirectoryPath = "*\\*";

			// Should be treated as constant.
			string[] filePaths = new string[] { WildcardDirectoryPath };

			Assert.DoesNotThrow(() => new VisualStudioFiles(filePaths));
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Test BasicProjectFiles property when Visual Basic project files found.
		/// </summary>
		[Test]
		public void BasicProjectFilesWhenFound()
		{
			AddBasicProjectFiles();

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.BasicProjectFiles);

			IEnumerator<BasicProjectFile> projectFileEnumerator = VisualStudioFiles.BasicProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(BasicProjectFileName, projectFileEnumerator.Current.FileName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test BasicProjectFiles property when no Visual Basic project files found.
		/// </summary>
		[Test]
		public void BasicProjectFilesWhenNoneFound()
		{
			CollectionAssert.IsEmpty(VisualStudioFiles.BasicProjectFiles);
		}

		/// <summary>
		/// Test BasicSourceFiles property when Visual Basic source files found.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenFound()
		{
			AddBasicSourceFiles();

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.BasicSourceFiles);

			IEnumerator<BasicSourceFile> sourceFileEnumerator = VisualStudioFiles.BasicSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(BasicSourceFileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test BasicSourceFiles property when no Visual Basic source files found.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenNoneFound()
		{
			CollectionAssert.IsEmpty(VisualStudioFiles.BasicSourceFiles);
		}

		/// <summary>
		/// Test CSharpProjectFiles property when Visual C# project files found.
		/// </summary>
		[Test]
		public void CSharpProjectFilesWhenFound()
		{
			AddCSharpProjectFiles();

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.CSharpProjectFiles);

			IEnumerator<CSharpProjectFile> projectFileEnumerator = VisualStudioFiles.CSharpProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(CSharpProjectFileName, projectFileEnumerator.Current.FileName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test CSharpProjectFiles property when no Visual C# project files found.
		/// </summary>
		[Test]
		public void CSharpProjectFilesWhenNoneFound()
		{
			CollectionAssert.IsEmpty(VisualStudioFiles.CSharpProjectFiles);
		}

		/// <summary>
		/// Test CSharpSourceFiles property when Visual C# source files found.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenFound()
		{
			AddCSharpSourceFiles();

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.CSharpSourceFiles);

			IEnumerator<CSharpSourceFile> sourceFileEnumerator = VisualStudioFiles.CSharpSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(CSharpSourceFileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test CSharpSourceFiles property when no Visual C# source files found.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenNoneFound()
		{
			CollectionAssert.IsEmpty(VisualStudioFiles.CSharpSourceFiles);
		}

		/// <summary>
		/// Test FSharpProjectFiles property when Visual F# project files found.
		/// </summary>
		[Test]
		public void FSharpProjectFilesWhenFound()
		{
			AddFSharpProjectFiles();

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.FSharpProjectFiles);

			IEnumerator<FSharpProjectFile> projectFileEnumerator = VisualStudioFiles.FSharpProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(FSharpProjectFileName, projectFileEnumerator.Current.FileName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test FSharpProjectFiles property when no Visual F# project files found.
		/// </summary>
		[Test]
		public void FSharpProjectFilesWhenNoneFound()
		{
			CollectionAssert.IsEmpty(VisualStudioFiles.FSharpProjectFiles);
		}

		/// <summary>
		/// Test FSharpSourceFiles property when Visual F# source files found.
		/// </summary>
		[Test]
		public void FSharpSourceFilesWhenFound()
		{
			AddFSharpSourceFiles();

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.FSharpSourceFiles);

			IEnumerator<FSharpSourceFile> sourceFileEnumerator = VisualStudioFiles.FSharpSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(FSharpSourceFileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test FSharpSourceFiles property when no Visual F# source files found.
		/// </summary>
		[Test]
		public void FSharpSourceFilesWhenNoneFound()
		{
			CollectionAssert.IsEmpty(VisualStudioFiles.FSharpSourceFiles);
		}

		/// <summary>
		/// Test SolutionFiles property when Visual Studio solution files found.
		/// </summary>
		[Test]
		public void SolutionFilesWhenFound()
		{
			AddSolutionFiles();

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.SolutionFiles);

			IEnumerator<SolutionFile> solutionFileEnumerator = VisualStudioFiles.SolutionFiles.GetEnumerator();

			// Ensure solution file exists.
			Assert.IsTrue(solutionFileEnumerator.MoveNext());

			Assert.AreEqual(SolutionFileName, solutionFileEnumerator.Current.FileName);

			// Ensure no more solution files exist.
			Assert.IsFalse(solutionFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test SolutionFiles property when no Visual Studio solution files found.
		/// </summary>
		[Test]
		public void SolutionFilesWhenNoneFound()
		{
			CollectionAssert.IsEmpty(VisualStudioFiles.SolutionFiles);
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set file paths.
		/// </summary>
		/// <value>
		/// List of strings representing file paths.
		/// </value>
		private List<string> FilePaths { get; set; }

		/// <summary>
		/// Get/set mock file system.
		/// </summary>
		/// <value>
		/// Mock<IFileSystem> representing mock file system.
		/// </value>
		private Mock<IFileSystem> MockFileSystem { get; set; }

		/// <summary>
		/// Get/set Visual Studio files.
		/// </summary>
		/// <value>
		/// VisualStudioFiles representing Visual Studio files of varying type.
		/// </value>
		private VisualStudioFiles VisualStudioFiles { get; set; }
	}
}
