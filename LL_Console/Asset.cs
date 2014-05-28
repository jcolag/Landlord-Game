// <copyright file="Asset.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;

    public class Asset
    {
        private static int baseValue = 100;
        private string name = string.Empty;
        private int value = 0;
        
        public Asset(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

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

        public int Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}