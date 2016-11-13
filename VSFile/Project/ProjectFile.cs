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

using System;
using System.Collections.Generic;
using System.Xml;
using VSFile.Properties;
using VSFile.Source;

namespace VSFile.Project
{
	/// <summary>
	/// Represents a Visual Studio project file.
	/// </summary>
	/// <typeparam name="T">
	/// Type of Visual Studio source file referenced in this Visual Studio project file.
	/// </typeparam>
	public abstract class ProjectFile<T> : VisualStudioFile
		where T : SourceFile
	{
		/// <summary>
		/// XPath expressions used to select elements and attributes in project file.
		/// </summary>
		private static class XPath
		{
			public const string AutoGenElement = "msb:AutoGen";
			public const string CompileElement = "/msb:Project/msb:ItemGroup/msb:Compile";
			public const string IncludeAttribute = "@Include";
		}

		/// <summary>
		/// Namespace used in project file.
		/// </summary>
		private const string Namespace = "http://schemas.microsoft.com/developer/msbuild/2003";

		/// <summary>
		/// Namespace prefix used in XPath expressions.
		/// </summary>
		private const string NamespacePrefix = "msb";

		////////////////////////////////////////////////////////////////////////

		private List<T> sourceFiles;

		////////////////////////////////////////////////////////////////////////
		// Constructors

		/// <summary>
		/// Constructor for specifying file path.
		/// </summary>
		/// <param name="fileExtension">
		/// String representing file extension of Visual Studio project file.
		/// </param>
		/// <param name="filePath">
		/// String representing path to Visual Studio project file.
		/// </param>
		/// <param name="sourceFileExtension">
		/// String representing file extension of Visual Studio source files
		/// referenced in this Visual Studio project file.
		/// </param>
		protected ProjectFile(string fileExtension, string filePath, string sourceFileExtension)
			: this(null, fileExtension, filePath, sourceFileExtension)
		{
		}

		/// <summary>
		/// Constructor for specifying project name.
		/// </summary>
		/// <param name="projectName">
		/// String representing Visual Studio project name.
		/// </param>
		/// <param name="fileExtension">
		/// String representing file extension of Visual Studio project file.
		/// </param>
		/// <param name="filePath">
		/// String representing path to Visual Studio project file.
		/// </param>
		/// <param name="sourceFileExtension">
		/// String representing file extension of Visual Studio source files
		/// referenced in this Visual Studio project file.
		/// </param>
		protected ProjectFile(string projectName, string fileExtension, string filePath,
			string sourceFileExtension)
			: base(fileExtension, filePath)
		{
			if (string.IsNullOrWhiteSpace(sourceFileExtension))
				throw new ArgumentException(ExceptionMessages.InvalidSourceFileExtension);

			ProjectName = string.IsNullOrWhiteSpace(projectName) ? FileNameNoExtension : projectName;
			SourceFileExtension = sourceFileExtension;
			sourceFiles = new List<T>();
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
		protected abstract T CreateSourceFile(string filePath);

		/// <summary>
		/// Read file.
		/// </summary>
		protected override void ReadFile()
		{
			ClearFiles();

			XmlDocument document = new XmlDocument();

			document.Load(FilePath);

			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);

			namespaceManager.AddNamespace(NamespacePrefix, Namespace);

			XmlNodeList nodeList = document.SelectNodes(XPath.CompileElement, namespaceManager);

			foreach (XmlNode node in nodeList)
				AddSourceFile(node, namespaceManager);
		}

		/// <summary>
		/// Add source file contained within given XML node and namespace manager.
		/// </summary>
		/// <param name="node">
		/// XmlNode containing source file.
		/// </param>
		/// <param name="namespaceManager">
		/// XmlNamespaceManager managing XML namespace used in project file.
		/// </param>
		private void AddSourceFile(XmlNode node, XmlNamespaceManager namespaceManager)
		{
			XmlNode childNode = node.SelectSingleNode(XPath.AutoGenElement, namespaceManager);

			if (childNode != null)
			{
				string autoGenerated = childNode.InnerText;

				// Ignore auto-generated files.
				if (autoGenerated != null &&
					autoGenerated.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}

			XmlNode attribute = node.SelectSingleNode(XPath.IncludeAttribute, namespaceManager);

			if (attribute != null)
			{
				string filePath = attribute.Value;

				if (filePath != null &&
					filePath.EndsWith(SourceFileExtension, StringComparison.OrdinalIgnoreCase))
				{
					T sourceFile = CreateSourceFile(GetFullPath(filePath));

					sourceFiles.Add(sourceFile);
				}
			}
		}

		/// <summary>
		/// Clear referenced source files.
		/// </summary>
		private void ClearFiles()
		{
			sourceFiles.Clear();
		}

		////////////////////////////////////////////////////////////////////////
		// Properties

		/// <summary>
		/// Get project name.
		/// </summary>
		/// <value>
		/// String representing project name.
		/// </value>
		public string ProjectName { get; private set; }

		/// <summary>
		/// Get file extension of Visual Studio source files referenced in this project file.
		/// </summary>
		/// <value>
		/// String representing Visual Studio source file extension.
		/// </value>
		public string SourceFileExtension { get; private set; }

		/// <summary>
		/// Get Visual Studio source files referenced in this project file.
		/// </summary>
		/// <value>
		/// Enumerable collection of objects representing Visual Studio source
		/// files referenced in this project file.
		/// </value>
		public IEnumerable<T> SourceFiles
		{
			get { return sourceFiles; }
		}
	}
}
