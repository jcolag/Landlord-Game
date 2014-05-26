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
            Text = text;
            Amount = amount;
            Jail = jail;
        }
    }
}