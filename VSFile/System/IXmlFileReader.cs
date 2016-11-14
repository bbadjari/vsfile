using System.Xml.Linq;

namespace VSFile.System
{
	/// <summary>
	/// XML file reader contract.
	/// </summary>
	internal interface IXmlFileReader
	{
		/// <summary>
		/// Load XML file from given path.
		/// </summary>
		/// <param name="path">
		/// String representing XML file path.
		/// </param>
		/// <returns>
		/// XDocument representing XML document loaded from file.
		/// </returns>
		XDocument Load(string path);
	}
}
