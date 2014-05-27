// <copyright file="Asset.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;

    public class Asset
    {
        public static int BaseValue = 100;
        private string _name = string.Empty;

        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        private int _value = 0;

        public int Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;
            }
        }

        public Asset(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}