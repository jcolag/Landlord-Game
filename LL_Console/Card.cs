// <copyright file="Card.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
    using System;

    /// <summary>
    /// Card represents any special actions in the game.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// The text to be shown on the card.
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        /// The amount to add to or remove from the current player's balance.
        /// </summary>
        private int amount = 0;

        /// <summary>
        /// If set, the card sends the player to jail.
        /// </summary>
        private bool jail = false;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LlConsole.Card"/> class.
        /// </summary>
        /// <param name="contents">Text on the card.</param>
        /// <param name="award">Amount to modify player balance.</param>
        /// <param name="sendToJail">If set to <c>true</c>, send player to jail.</param>
        public Card(string contents, int award, bool sendToJail)
        {
            this.Text = contents;
            this.Amount = award;
            this.Jail = sendToJail;
        }

        /// <summary>
        /// Gets or sets the card text.
        /// </summary>
        /// <value>The text.</value>
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

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LlConsole.Card"/> sends the
        /// player to jail.
        /// </summary>
        /// <value><c>true</c> if jail; otherwise, <c>false</c>.</value>
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