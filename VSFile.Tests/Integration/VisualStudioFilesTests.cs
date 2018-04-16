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
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using VSFile.Project;
using VSFile.Source;
using VSFile.Tests.Category;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Integration
{
	/// <summary>
	/// Integration tests for VisualStudioFiles class.
	/// </summary>
	[TestFixture, Integration]
	public class VisualStudioFilesTests
	{
		private const string DirectoryName = "VSFile.Tests.Integration";

		////////////////////////////////////////////////////////////////////////
		// Helper Methods

		/// <summary>
		/// Called after each test executed.
		/// </summary>
		[TearDown]
		public void AfterTest()
		{
			Directory.Delete(DirectoryPath, true);
		}

		/// <summary>
		/// Called before each test executed.
		/// </summary>
		[SetUp]
		public void BeforeTest()
		{
			DirectoryPath = Path.Combine(Path.GetTempPath(), DirectoryName);

			Directory.CreateDirectory(DirectoryPath);
		}

		/// <summary>
		/// Create instance of class being tested.
		/// </summary>
		private void Create()
		{
			string[] filePaths = new string[] { Path.Combine(DirectoryPath, Wildcard.Asterisk) };

			VisualStudioFiles = new VisualStudioFiles(filePaths, false);
		}

		/// <summary>
		/// Write file with given file name and file contents.
		/// </summary>
		/// <param name="fileName">
		/// String representing file name.
		/// </param>
		/// <param name="fileContents">
		/// String representing file contents.
		/// </param>
		private void WriteFile(string fileName, string fileContents)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fileName), "Invalid file name.");
			Debug.Assert(!string.IsNullOrWhiteSpace(fileContents), "Invalid file contents.");

			File.WriteAllText(Path.Combine(DirectoryPath, fileName), fileContents);
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Test BasicProjectFiles property when Visual Basic project files found.
		/// </summary>
		[Test]
		public void BasicProjectFilesWhenFound()
		{
			const string FileName = "BasicProjectFile.vbproj";
			const string ProjectName = "BasicProjectFile";

			WriteFile(FileName, EmbeddedFiles.BasicProjectFile);

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.BasicProjectFiles);

			IEnumerator<BasicProjectFile> projectFileEnumerator = VisualStudioFiles.BasicProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(ProjectName, projectFileEnumerator.Current.ProjectName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test BasicProjectFiles property when no Visual Basic project files found.
		/// </summary>
		[Test]
		public void BasicProjectFilesWhenNoneFound()
		{
			Create();

			CollectionAssert.IsEmpty(VisualStudioFiles.BasicProjectFiles);
		}

		/// <summary>
		/// Test BasicSourceFiles property when Visual Basic source files found.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenFound()
		{
			const string FileName = "BasicSourceFile.vb";

			WriteFile(FileName, EmbeddedFiles.BasicSourceFile);

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.BasicSourceFiles);

			IEnumerator<BasicSourceFile> sourceFileEnumerator = VisualStudioFiles.BasicSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(FileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test BasicSourceFiles property when no Visual Basic source files found.
		/// </summary>
		[Test]
		public void BasicSourceFilesWhenNoneFound()
		{
			Create();

			CollectionAssert.IsEmpty(VisualStudioFiles.BasicSourceFiles);
		}

		/// <summary>
		/// Test CSharpProjectFiles property when Visual C# project files found.
		/// </summary>
		[Test]
		public void CSharpProjectFilesWhenFound()
		{
			const string FileName = "CSharpProjectFile.csproj";
			const string ProjectName = "CSharpProjectFile";

			WriteFile(FileName, EmbeddedFiles.CSharpProjectFile);

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.CSharpProjectFiles);

			IEnumerator<CSharpProjectFile> projectFileEnumerator = VisualStudioFiles.CSharpProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(ProjectName, projectFileEnumerator.Current.ProjectName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test CSharpProjectFiles property when no Visual C# project files found.
		/// </summary>
		[Test]
		public void CSharpProjectFilesWhenNoneFound()
		{
			Create();

			CollectionAssert.IsEmpty(VisualStudioFiles.CSharpProjectFiles);
		}

		/// <summary>
		/// Test CSharpSourceFiles property when Visual C# source files found.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenFound()
		{
			const string FileName = "CSharpSourceFile.cs";

			WriteFile(FileName, EmbeddedFiles.CSharpSourceFile);

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.CSharpSourceFiles);

			IEnumerator<CSharpSourceFile> sourceFileEnumerator = VisualStudioFiles.CSharpSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(FileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test CSharpSourceFiles property when no Visual C# source files found.
		/// </summary>
		[Test]
		public void CSharpSourceFilesWhenNoneFound()
		{
			Create();

			CollectionAssert.IsEmpty(VisualStudioFiles.CSharpSourceFiles);
		}

		/// <summary>
		/// Test FSharpProjectFiles property when Visual F# project files found.
		/// </summary>
		[Test]
		public void FSharpProjectFilesWhenFound()
		{
			const string FileName = "FSharpProjectFile.fsproj";
			const string ProjectName = "FSharpProjectFile";

			WriteFile(FileName, EmbeddedFiles.FSharpProjectFile);

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.FSharpProjectFiles);

			IEnumerator<FSharpProjectFile> projectFileEnumerator = VisualStudioFiles.FSharpProjectFiles.GetEnumerator();

			// Ensure project file exists.
			Assert.IsTrue(projectFileEnumerator.MoveNext());

			Assert.AreEqual(ProjectName, projectFileEnumerator.Current.ProjectName);

			// Ensure no more project files exist.
			Assert.IsFalse(projectFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test FSharpProjectFiles property when no Visual F# project files found.
		/// </summary>
		[Test]
		public void FSharpProjectFilesWhenNoneFound()
		{
			Create();

			CollectionAssert.IsEmpty(VisualStudioFiles.FSharpProjectFiles);
		}

		/// <summary>
		/// Test FSharpSourceFiles property when Visual F# source files found.
		/// </summary>
		[Test]
		public void FSharpSourceFilesWhenFound()
		{
			const string FileName = "FSharpSourceFile.fs";

			WriteFile(FileName, EmbeddedFiles.FSharpSourceFile);

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.FSharpSourceFiles);

			IEnumerator<FSharpSourceFile> sourceFileEnumerator = VisualStudioFiles.FSharpSourceFiles.GetEnumerator();

			// Ensure source file exists.
			Assert.IsTrue(sourceFileEnumerator.MoveNext());

			Assert.AreEqual(FileName, sourceFileEnumerator.Current.FileName);

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test FSharpSourceFiles property when no Visual F# source files found.
		/// </summary>
		[Test]
		public void FSharpSourceFilesWhenNoneFound()
		{
			Create();

			CollectionAssert.IsEmpty(VisualStudioFiles.FSharpSourceFiles);
		}

		/// <summary>
		/// Test SolutionFiles property when Visual Studio solution files found.
		/// </summary>
		[Test]
		public void SolutionFilesWhenFound()
		{
			const string FileName = "SolutionFile.sln";

			WriteFile(FileName, EmbeddedFiles.SolutionFile);

			Create();

			CollectionAssert.IsNotEmpty(VisualStudioFiles.SolutionFiles);

			IEnumerator<SolutionFile> solutionFileEnumerator = VisualStudioFiles.SolutionFiles.GetEnumerator();

			// Ensure solution file exists.
			Assert.IsTrue(solutionFileEnumerator.MoveNext());

			Assert.AreEqual(FileName, solutionFileEnumerator.Current.FileName);

			// Ensure no more solution files exist.
			Assert.IsFalse(solutionFileEnumerator.MoveNext());
		}

		/// <summary>
		/// Test SolutionFiles property when no Visual Studio solution files found.
		/// </summary>
		[Test]
		public void SolutionFilesWhenNoneFound()
		{
			Create();

			CollectionAssert.IsEmpty(VisualStudioFiles.SolutionFiles);
		}

		////////////////////////////////////////////////////////////////////////
		// Helper Properties

		/// <summary>
		/// Get/set directory path.
		/// </summary>
		/// <value>
		/// String representing directory path.
		/// </value>
		private string DirectoryPath { get; set; }

		/// <summary>
		/// Get/set Visual Studio files.
		/// </summary>
		/// <value>
		/// VisualStudioFiles representing Visual Studio files of varying type.
		/// </value>
		private VisualStudioFiles VisualStudioFiles { get; set; }
	}
}
