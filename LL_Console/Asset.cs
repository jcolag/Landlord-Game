// <copyright file="Asset.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
        using System;

        /// <summary>
        /// Asset represents non-liquid assets in the game, something that can be
        /// sold to raise additional cash.
        /// </summary>
        public class Asset
        {
                /// <summary>
                /// Default value of any asset.
                /// </summary>
                private static int baseValue = 100;

                /// <summary>
                /// The asset's name.
                /// </summary>
                private string name = string.Empty;

                /// <summary>
                /// The asset's value, defaulting to baseValue, above.
                /// </summary>
                private int exchange = 0;

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Asset"/> class.
                /// </summary>
                /// <param name="assetName">Name of the asset.</param>
                /// <param name="assetValue">Value of the asset.</param>
                public Asset(string assetName, int assetValue)
                {
                        this.Name = assetName;
                        this.Value = assetValue;
                }

                /// <summary>
                /// Gets or sets the default value of future assets.
                /// </summary>
                /// <value>The base value.</value>
                public static int BaseValue
                {
                        get
                        {
                                return baseValue;
                        }

                        set
                        {
                                baseValue = value;
                        }
                }

                /// <summary>
                /// Gets or sets the asset's name.
                /// </summary>
                /// <value>The name.</value>
                public string Name
                {
                        get
                        {
                                return this.name;
                        }

                        set
                        {
                                this.name = value;
                        }
                }

                /// <summary>
                /// Gets or sets the asset's value.
                /// </summary>
                /// <value>The value.</value>
                public int Value
                {
                        get
                        {
                                return this.exchange;
                        }

                        set
                        {
                                this.exchange = value;
                        }
                }
        }
}