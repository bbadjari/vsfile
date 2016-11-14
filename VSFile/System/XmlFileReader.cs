using System.Xml.Linq;

namespace VSFile.System
{
	/// <summary>
	/// XML file reader implementation.
	/// </summary>
	internal class XmlFileReader : IXmlFileReader
	{
		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Load XML file from given path.
		/// </summary>
		/// <param name="path">
		/// String representing XML file path.
		/// </param>
		/// <returns>
		/// XDocument representing XML document loaded from file.
		/// </returns>
		public XDocument Load(string path)
		{
			return XDocument.Load(path);
		}
	}
}
