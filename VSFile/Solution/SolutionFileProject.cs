////////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2021 Bernard Badjari
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

using System.Diagnostics;

namespace VSFile.Solution
{
	/// <summary>
	/// Represents project reference contained in a Visual Studio solution file.
	/// </summary>
	internal class SolutionFileProject
	{
		/// <summary>
		/// No project reference.
		/// </summary>
		public const SolutionFileProject None = null;

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">
		/// String representing project name.
		/// </param>
		/// <param name="path">
		/// String representing project relative path.
		/// </param>
		/// <param name="typeGuid">
		/// String representing project type GUID.
		/// </param>
		/// <param name="uniqueGuid">
		/// String representing project unique GUID.
		/// </param>
		public SolutionFileProject(string name, string path, string typeGuid, string uniqueGuid)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(name), "Invalid name.");
			Debug.Assert(!string.IsNullOrWhiteSpace(path), "Invalid path.");
			Debug.Assert(!string.IsNullOrWhiteSpace(typeGuid), "Invalid type GUID.");
			Debug.Assert(!string.IsNullOrWhiteSpace(uniqueGuid), "Invalid unique GUID.");

			Name = name;
			Path = path;
			TypeGuid = typeGuid;
			UniqueGuid = uniqueGuid;
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get project name.
		/// </summary>
		/// <value>
		/// String representing project name.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		/// Get project relative path.
		/// </summary>
		/// <value>
		/// String representing project relative path.
		/// </value>
		public string Path { get; private set; }

		/// <summary>
		/// Get project type GUID.
		/// </summary>
		/// <value>
		/// String representing project type GUID.
		/// </value>
		public string TypeGuid { get; private set; }

		/// <summary>
		/// Get project unique GUID.
		/// </summary>
		/// <value>
		/// String representing project unique GUID.
		/// </value>
		public string UniqueGuid { get; private set; }
	}
}
