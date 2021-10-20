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

namespace VSFile.Project
{
	/// <summary>
	/// GUIDs used to identify types of Visual Studio projects.
	/// </summary>
	internal static class ProjectTypeGuid
	{
		/// <summary>
		/// Visual Basic project type.
		/// </summary>
		public const string Basic = "F184B08F-C81C-45F6-A57F-5ABD9991F28F";

		/// <summary>
		/// Visual C# project type.
		/// </summary>
		public const string CSharp = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";

		/// <summary>
		/// Visual F# project type.
		/// </summary>
		public const string FSharp = "F2A71F9B-5D33-465A-A702-920D77279786";

		/// <summary>
		/// ASP.NET web site project type.
		/// </summary>
		public const string WebSite = "E24C65DC-7377-472B-9ABA-BC803B73C61A";
	}
}
