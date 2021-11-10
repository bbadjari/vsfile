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

using System;
using System.Diagnostics;

namespace VSFile.Solution
{
	/// <summary>
	/// Attribute representing project reference values contained in a Visual Studio solution file.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	internal class SolutionFileProjectAttribute : Attribute
	{
		/// <summary>
		/// Default minimum solution file format version.
		/// </summary>
		private const int DefaultMinimumFormatVersion = SolutionFileFormatVersion.Minimum;

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="typeGuid">
		/// String representing project type GUID.
		/// </param>
		/// <param name="minimumFormatVersion">
		/// Integer representing minimum solution file format version.
		/// </param>
		public SolutionFileProjectAttribute(string typeGuid, int minimumFormatVersion = DefaultMinimumFormatVersion)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(typeGuid), "Invalid type GUID.");

			MinimumFormatVersion = Math.Max(minimumFormatVersion, DefaultMinimumFormatVersion);
			TypeGuid = typeGuid;
		}

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Determine whether this attribute matches given project reference values.
		/// </summary>
		/// <param name="typeGuid">
		/// String representing project type GUID.
		/// </param>
		/// <param name="formatVersion">
		/// Integer representing solution file format version.
		/// </param>
		/// <returns>
		/// True if this attribute matches given project reference values, false otherwise.
		/// </returns>
		public bool IsMatch(string typeGuid, int formatVersion)
		{
			return (typeGuid == TypeGuid) && (formatVersion >= MinimumFormatVersion);
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get minimum solution file format version.
		/// </summary>
		/// <value>
		/// Integer representing minimum solution file format version.
		/// </value>
		public int MinimumFormatVersion { get; private set; }

		/// <summary>
		/// Get project type GUID.
		/// </summary>
		/// <value>
		/// String representing project type GUID.
		/// </value>
		public string TypeGuid { get; private set; }
	}
}
