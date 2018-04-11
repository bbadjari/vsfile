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
using NUnit.Framework;
using VSFile.Project;
using VSFile.Source;
using VSFile.Tests.Properties;

namespace VSFile.Tests.Integration
{
	/// <summary>
	/// Integration tests for CSharpProjectFile class.
	/// </summary>
	public class CSharpProjectFileTests : IntegrationTestFixture
	{
		private const string FileName = "CSharpProjectFile.csproj";

		////////////////////////////////////////////////////////////////////////
		// Helper Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public CSharpProjectFileTests()
			: base(FileName, EmbeddedFiles.CSharpProjectFile)
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

			CSharpProjectFile = new CSharpProjectFile(FilePath);
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

		////////////////////////////////////////////////////////////////////////
		// Properties

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

			// Ensure no more source files exist.
			Assert.IsFalse(sourceFileEnumerator.MoveNext());
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
	}
}
