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

namespace VSFile.Project
{
	/// <summary>
	/// Represents a Visual F# project file.
	/// </summary>
	public class FSharpProjectFile : ProjectFile<FSharpSourceFile>
	{
		/// <summary>
		/// File extension of a Visual F# project file.
		/// </summary>
		public const string ProjectFileExtension = ".fsproj";

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor for specifying file path.
		/// </summary>
		/// <param name="filePath">
		/// String representing path to Visual F# project file.
		/// </param>
		public FSharpProjectFile(string filePath)
			: base(ProjectFileExtension, filePath, FSharpSourceFile.SourceFileExtension)
		{
		}

		/// <summary>
		/// Constructor for specifying project name.
		/// </summary>
		/// <param name="projectName">
		/// String representing Visual F# project name.
		/// </param>
		/// <param name="filePath">
		/// String representing path to Visual F# project file.
		/// </param>
		public FSharpProjectFile(string projectName, string filePath)
			: base(projectName, ProjectFileExtension, filePath, FSharpSourceFile.SourceFileExtension)
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
		protected override FSharpSourceFile CreateSourceFile(string filePath)
		{
			return new FSharpSourceFile(filePath);
		}
	}
}
