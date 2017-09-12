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

using System.Diagnostics;
using System.Linq;

namespace VSFile
{
	/// <summary>
	/// Wildcard characters contained in file paths.
	/// </summary>
	internal static class Wildcard
	{
		/// <summary>
		/// Zero or more characters.
		/// </summary>
		public const string Asterisk = "*";

		/// <summary>
		/// Zero or one character.
		/// </summary>
		public const string Question = "?";

		/// <summary>
		/// Wildcard characters.
		/// </summary>
		private static readonly string[] Wildcards = new string[]
		{
			Asterisk,
			Question
		};

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Add asterisk wildcard character to given file extension.
		/// </summary>
		/// <param name="fileExtension">
		/// String representing file extension.
		/// </param>
		/// <returns>
		/// String representing file extension prefixed with asterisk wildcard character.
		/// </returns>
		public static string AddAsterisk(string fileExtension)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fileExtension), "Invalid file extension.");

			return Asterisk + fileExtension;
		}

		/// <summary>
		/// Determine if given file path contains any wildcard characters.
		/// </summary>
		/// <param name="filePath">
		/// String representing file path.
		/// </param>
		/// <returns>
		/// True if file path contains any wildcard characters, false otherwise.
		/// </returns>
		public static bool HasWildcard(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
				return false;

			return Wildcards.Any(wildcard => filePath.Contains(wildcard));
		}
	}
}
