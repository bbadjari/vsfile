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

namespace VSFile.Solution
{
	/// <summary>
	/// Visual Studio solution file format versions.
	/// </summary>
	internal static class SolutionFileFormatVersion
	{
		/// <summary>
		/// Minimum format version.
		/// </summary>
		public const int Minimum = VisualStudio2002;

		public const int VisualStudio2002 = 7;
		public const int VisualStudio2003 = 8;
		public const int VisualStudio2005 = 9;
		public const int VisualStudio2008 = 10;
		public const int VisualStudio2010 = 11;

		/// <summary>
		/// Format version used in Visual Studio 2012 and later versions.
		/// </summary>
		public const int VisualStudio2012 = 12;
	}
}
