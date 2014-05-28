// <copyright file="Card.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;

    public class Card
    {
        private string text = string.Empty;
        private int amount = 0;
        private bool jail = false;
        
        public Card(string text, int amount, bool jail)
        {
            this.Text = text;
            this.Amount = amount;
            this.Jail = jail;
        }

        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
            }
        }

        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public bool Jail
        {
            get
            {
                return this.jail;
            }

            set
            {
                this.jail = value;
            }
        }
    }
}