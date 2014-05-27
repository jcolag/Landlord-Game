// <copyright file="Card.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;

    public class Card
    {
        public string Text = string.Empty;
        public int Amount = 0;
        public bool Jail = false;

        public Card(string text, int amount, bool jail)
        {
            this.Text = text;
            this.Amount = amount;
            this.Jail = jail;
        }
    }
}