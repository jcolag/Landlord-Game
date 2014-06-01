// <copyright file="Game.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
        using System;
        using System.Collections.Generic;
        using System.Collections.ObjectModel;

        /// <summary>
        /// Game represents a full game.
        /// </summary>
        public class Game
        {
                /// <summary>
                /// The board, specifically the locations on the board.
                /// </summary>
                private Collection<Location> board = new Collection<Location>();

                /// <summary>
                /// The players.
                /// </summary>
                private List<Player> players = new List<Player>();

                /// <summary>
                /// The first pile of cards.
                /// </summary>
                private List<Card> pile1 = new List<Card>();

                /// <summary>
                /// The second pile of cards.
                /// </summary>
                private List<Card> pile2 = new List<Card>();

                /// <summary>
                /// The name of the first pile.
                /// </summary>
                private string pile1Name = "Chance";

                /// <summary>
                /// The name of the second pile.
                /// </summary>
                private string pile2Name = "Community Chest";

                /// <summary>
                /// The dice count.
                /// </summary>
                private int diceCount = 2;

                /// <summary>
                /// The number of sides on the dice.
                /// </summary>
                private int diceSides = 6;

                /// <summary>
                /// The random number generator for the dice.
                /// </summary>
                private Random rand = new Random();

                /// <summary>
                /// The current player.
                /// </summary>
                private Player currentPlayer;

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Game"/> class.
                /// </summary>
                public Game()
                {
                        Location.SetBoard(this.Board);
                }

                /// <summary>
                /// Gets a value indicating whether this <see cref="LlConsole.Game"/> can continue.
                /// </summary>
                /// <value><c>true</c> if the game can continue; otherwise, <c>false</c>.</value>
                public bool Continue
                {
                        get
                        {
                                int solvent = 0;
                                foreach (Player p in this.Players)
                                {
                                        if (p.Balance >= 0)
                                        {
                                                ++solvent;
                                        }
                                }

                                return solvent > 1;
                        }
                }

                /// <summary>
                /// Gets the board locations.
                /// </summary>
                /// <value>The board.</value>
                public Collection<Location> Board
                {
                        get
                        {
                                return this.board;
                        }
                }

                /// <summary>
                /// Gets or sets the name of the first card pile.
                /// </summary>
                /// <value>The name of the pile1.</value>
                public string Pile1Name
                {
                        get
                        {
                                return this.pile1Name;
                        }

                        set
                        {
                                this.pile1Name = value;
                        }
                }

                /// <summary>
                /// Gets or sets the name of the second card pile.
                /// </summary>
                /// <value>The name of the pile2.</value>
                public string Pile2Name
                {
                        get
                        {
                                return this.pile2Name;
                        }

                        set
                        {
                                this.pile2Name = value;
                        }
                }

                /// <summary>
                /// Gets or sets the dice count.
                /// </summary>
                /// <value>The dice count.</value>
                public int DiceCount
                {
                        get
                        {
                                return this.diceCount;
                        }

                        set
                        {
                                this.diceCount = value;
                        }
                }

                /// <summary>
                /// Gets or sets the number of sides on the dice.
                /// </summary>
                /// <value>The dice sides.</value>
                public int DiceSides
                {
                        get
                        {
                                return this.diceSides;
                        }

                        set
                        {
                                this.diceSides = value;
                        }
                }

                /// <summary>
                /// Gets the player count.
                /// </summary>
                /// <value>The player count.</value>
                public int PlayerCount
                {
                        get
                        {
                                return this.Players.Count;
                        }
                }

                /// <summary>
                /// Gets the player list.
                /// </summary>
                /// <value>The players.</value>
                private List<Player> Players
                {
                        get
                        {
                                return this.players;
                        }
                }

                /// <summary>
                /// Gets the first card pile.
                /// </summary>
                /// <value>The pile1.</value>
                private List<Card> Pile1
                {
                        get
                        {
                                return this.pile1;
                        }
                }

                /// <summary>
                /// Gets the second card pile.
                /// </summary>
                /// <value>The pile2.</value>
                private List<Card> Pile2
                {
                        get
                        {
                                return this.pile2;
                        }
                }

                /// <summary>
                /// Roll the dice, returning the value and a printable notice.
                /// </summary>
                /// <param name="notices">Return value for printable notices.</param>
                /// <returns>The rolled value.</returns>
                public int Roll(out string notices)
                {
                        return this.Roll(this.currentPlayer, out notices);
                }

                /// <summary>
                /// Roll the dice, returning the value and a printable notice.
                /// </summary>
                /// <param name="p">The current player.</param>
                /// <param name="notices">Return value for printable notices.</param>
                /// <returns>The rolled value.</returns>
                public int Roll(Player p, out string notices)
                {
                        int dice = 0,
                        i;
                        Location l;
                        string note = string.Empty;

                        notices = string.Empty;
                        for (i = 0; i < this.diceCount; i++)
                        {
                                dice += this.rand.Next(1, this.diceSides);
                        }

                        if (p == null)
                        {
                                return dice;
                        }

                        l = p.Where;
                        for (i = 0; i < dice; i++)
                        {
                                l = this.NextLocation(l, ref note, i != dice - 1);
                                notices += note;
                        }
            
                        p.Where = l;
                        return dice;
                }

                /// <summary>
                /// Who is the current player?
                /// </summary>
                /// <returns>The current player.</returns>
                public Player Who()
                {
                        return this.currentPlayer;
                }

                /// <summary>
                /// Where is the current player?
                /// </summary>
                /// <returns>The location of the current player.</returns>
                public Location Where()
                {
                        return this.currentPlayer.Where;
                }

                /// <summary>
                /// Add the specified player.
                /// </summary>
                /// <param name="p">The new player.</param>
                public void Add(Player p)
                {
                        this.Players.Add(p);
                        if (this.currentPlayer == null)
                        {
                                this.currentPlayer = p;
                        }
                }

                /// <summary>
                /// Add the specified location to the board.
                /// </summary>
                /// <param name="l">The new location.</param>
                public void Add(Location l)
                {
                        this.Board.Add(l);
                }

                /// <summary>
                /// Add the specified card to the correct.
                /// </summary>
                /// <param name="c">The new card.</param>
                /// <param name="secondPile">If set to <c>true</c>, use the second pile.</param>
                public void Add(Card c, bool secondPile)
                {
                        List<Card> pile = secondPile ? this.Pile2 : this.Pile1;
                        pile.Add(c);
                }

                /// <summary>
                /// Update the current player to the next.
                /// </summary>
                /// <returns>The player.</returns>
                public Player NextPlayer()
                {
                        int idx = (this.Players.IndexOf(this.currentPlayer) + 1) % this.Players.Count;
                        this.currentPlayer = this.Players[idx];
                        if (this.currentPlayer.Balance <= 0 && this.Players.Count > 1)
                        {
                                this.currentPlayer = this.NextPlayer();
                        }
            
                        return this.currentPlayer;
                }

                /// <summary>
                /// Find the previous player.
                /// </summary>
                /// <returns>The player.</returns>
                public Player PrevPlayer()
                {
                        int idx = (this.Players.IndexOf(this.currentPlayer) + this.Players.Count - 1)
                                % this.Players.Count;
                        return this.Players[idx];
                }

                /// <summary>
                /// Find the next location.
                /// </summary>
                /// <returns>The location.</returns>
                /// <param name="l">The current location.</param>
                /// <param name="notices">Return value for printable notices.</param>
                /// <param name="transit">If set to <c>true</c> transit.</param>
                public Location NextLocation(Location l, ref string notices, bool transit)
                {
                        int idx = this.Board.IndexOf(l) + 1;
                        if (idx >= this.Board.Count)
                        {
                                idx = 0;
                        }
            
                        if (transit)
                        {
                                notices = this.Board[idx].PassBy(this.currentPlayer);
                        }
            
                        return this.Board[idx];
                }

                /// <summary>
                /// Find the previous location.
                /// </summary>
                /// <returns>The location.</returns>
                /// <param name="l">The current location.</param>
                public Location PrevLocation(Location l)
                {
                        int idx = this.Board.IndexOf(l) - 1;
                        if (idx < 0)
                        {
                                idx = this.Board.Count - 1;
                        }
            
                        return this.Board[idx];
                }

                /// <summary>
                /// Draws the card.
                /// </summary>
                /// <returns>The card.</returns>
                /// <param name="secondPile">If set to <c>true</c>, use second pile.</param>
                /// <param name="reshuffle">If set to <c>true</c>, reshuffle the deck.</param>
                /// <param name="replace">If set to <c>true</c>, replace the card.</param>
                public Card DrawCard(bool secondPile, bool reshuffle, bool replace)
                {
                        List<Card> pile = secondPile ? this.Pile2 : this.Pile1;
                        int index = reshuffle ? this.rand.Next(pile.Count) : 0;
                        Card c = pile[index];
                        pile.Remove(c);
                        if (replace)
                        {
                                pile.Add(c);
                        }

                        return c;
                }
        }
}