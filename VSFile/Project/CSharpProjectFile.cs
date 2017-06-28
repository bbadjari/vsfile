////////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2011 Bernard Badjari
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

using VSFile.Source;
using VSFile.System;

namespace VSFile.Project
{
	/// <summary>
	/// Represents a Visual C# project file.
	/// </summary>
	public class CSharpProjectFile : ProjectFile<CSharpSourceFile>
	{
		/// <summary>
		/// File extension of a Visual C# project file.
		/// </summary>
		public const string ProjectFileExtension = ".csproj";

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor for specifying file path.
		/// </summary>
		/// <param name="filePath">
		/// String representing path to Visual C# project file.
		/// </param>
		public CSharpProjectFile(string filePath)
			: this(null, filePath)
		{
		}

		/// <summary>
		/// Constructor for specifying project name.
		/// </summary>
		/// <param name="projectName">
		/// String representing Visual C# project name.
		/// </param>
		/// <param name="filePath">
		/// String representing path to Visual C# project file.
		/// </param>
		public CSharpProjectFile(string projectName, string filePath)
			: base(projectName, ProjectFileExtension, filePath, CSharpSourceFile.SourceFileExtension)
		{
		}

		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="projectName">
		/// String representing Visual C# project name.
		/// </param>
		/// <param name="filePath">
		/// String representing path to Visual C# project file.
		/// </param>
		/// <param name="xmlFileReader">
		/// IXmlFileReader instance representing XML file reader.
		/// </param>
		/// <param name="fileSystem">
		/// IFileSystem instance representing file system.
		/// </param>
		internal CSharpProjectFile(string projectName, string filePath, IXmlFileReader xmlFileReader, IFileSystem fileSystem)
			: base(projectName, ProjectFileExtension, filePath, CSharpSourceFile.SourceFileExtension, xmlFileReader, fileSystem)
		{
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Create instance of source file with given file path.
		/// </summary>
		/// <param name="filePath">
		/// String representing file path.
		/// </param>
		/// <returns>
		/// Instance of source file with given file path.
		/// </returns>
		protected override CSharpSourceFile CreateSourceFile(string filePath)
		{
			return new CSharpSourceFile(filePath);
		}
	}
}
