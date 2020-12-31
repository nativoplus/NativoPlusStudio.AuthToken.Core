using System.Xml;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class XmlHelperExtensions
    {
        public static XmlDocument GetXmlDocument(this string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                return null;
            }
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            return doc;
        }
        
        public static string GetInnerTextInNode(this XmlDocument xmlDoc, string node)
        {
            if(xmlDoc == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(node))
            {
                return string.Empty;
            }

            var nodes = xmlDoc?.SelectSingleNode(node);
            return nodes?.InnerText ?? string.Empty;
        }
        
        public static XmlNode GetFirstNode(this XmlDocument xmlDoc, string node)
        {
            if(xmlDoc == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(node))
            {
                return null;
            }

            return xmlDoc?.SelectSingleNode(node);
        }
    }
}
