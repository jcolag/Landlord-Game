// <copyright file="XmlHelper.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
    using System;
    using System.Xml;

    /// <summary>
    /// Xml helper class, containing easier input routines.
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="LlConsole.XmlHelper"/> class from being created.
        /// The methods are all static, so you probably never need this.
        /// </summary>
        private XmlHelper()
        {
        }

        /// <summary>
        /// Read boolean from xml if it exists.
        /// </summary>
        /// <param name="node">The XML node.</param>
        /// <param name="attribute">The Attribute of interest.</param>
        /// <param name="target">Location to store the boolean.</param>
        public static void BoolFromXmlIfExists(
            XmlNode node,
            string attribute,
            ref bool target)
        {
            bool value;
            XmlAttribute attr = node.Attributes[attribute];
            if (attr == null)
            {
                return;
            }

            try
            {
                value = attr.InnerText.ToLower().Trim() == "true";
            }
            catch
            {
                return;
            }

            target = value;
        }

        /// <summary>
        /// Read integer from xml if it exists.
        /// </summary>
        /// <param name="node">The XML node.</param>
        /// <param name="attribute">The Attribute of interest.</param>
        /// <param name="target">Location to store the integer.</param>
        public static void IntFromXmlIfExists(
            XmlNode node,
            string attribute,
            ref int target)
        {
            int value;
            XmlAttribute attr = node.Attributes[attribute];
            if (attr == null)
            {
                return;
            }

            try
            {
                value = int.Parse(attr.InnerText);
            }
            catch
            {
                return;
            }

            target = value;
        }

        /// <summary>
        /// Read string from xml if it exists.
        /// </summary>
        /// <param name="node">The XML node.</param>
        /// <param name="attribute">The Attribute of interest.</param>
        /// <param name="target">Location to store the string.</param>
        public static void StringFromXmlIfExists(
            XmlNode node,
            string attribute,
            ref string target)
        {
            string value;
            XmlAttribute attr = node.Attributes[attribute];
            if (attr == null)
            {
                return;
            }

            try
            {
                value = attr.InnerText;
            }
            catch
            {
                return;
            }

            target = value;
        }
    }
}
