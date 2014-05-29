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
                /// Read a value from XML if the value exists.
                /// </summary>
                /// <param name="node">The XML node.</param>
                /// <param name="attribute">The Attribute of interest.</param>
                /// <param name="target">Location to store the value.</param>
                /// <typeparam name="T">The type of value and target's type.</typeparam>
                public static void FromXmlIfExists<T>(
                        XmlNode node,
                        string attribute,
                        ref T target)
                {
                        T value;
                        XmlAttribute attr = node.Attributes[attribute];
                        if (attr == null)
                        {
                                return;
                        }

                        try
                        {
                                string xmlValue = attr.InnerText.Trim();
                                value = (T)Convert.ChangeType(xmlValue, Convert.GetTypeCode(target));
                        }
                        catch
                        {
                                return;
                        }

                        target = value;
                }
        }
}
