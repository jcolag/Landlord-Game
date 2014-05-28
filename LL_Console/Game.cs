// <copyright file="Game.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        private List<Location> board = new List<Location>();
        private List<Player> players = new List<Player>();
        private List<Card> pile1 = new List<Card>();
        private List<Card> pile2 = new List<Card>();
        private string pile1Name = "Chance";
        private string pile2Name = "Community Chest";
        private int diceCount = 2;
        private int diceSides = 6;
        private Random rand = new Random();
        private Player currentPlayer = null;

        public bool Continue
        {
            get
            {
                int solvent = 0;
                foreach (Player p in this.Players)
                {
                    if (p.Balance > 0)
                    {
                        ++solvent;
                    }
                }
                
                return solvent > 1;
            }
        }

        public Game()
        {
            Location.Board = this.Board;
        }

        public List<Location> Board
        {
            get
            {
                return this.board;
            }

            set
            {
                this.board = value;
            }
        }

        public List<Player> Players
        {
            get
            {
                return this.players;
            }

            set
            {
                this.players = value;
            }
        }

        public List<Card> Pile1
        {
            get
            {
                return this.pile1;
            }

            set
            {
                this.pile1 = value;
            }
        }

        public List<Card> Pile2
        {
            get
            {
                return this.pile2;
            }

            set
            {
                this.pile2 = value;
            }
        }

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

        public int Roll(ref string notices)
        {
            return this.Roll(this.currentPlayer, ref notices);
        }

        public int Roll(Player p, ref string notices)
        {
            int dice = 0,
            i;
            Location l = p.Where;
            string note = string.Empty;

            for (i = 0; i < this.diceCount; i++)
            {
                dice += this.rand.Next(1, this.diceSides);
            }
            
            for (i = 0; i < dice; i++)
            {
                l = this.Next_Location(l, ref note, i != dice - 1);
                notices += note;
            }
            
            p.Where = l;
            return dice;
        }

        public Player Who()
        {
            return this.currentPlayer;
        }

        public Location Where()
        {
            return this.currentPlayer.Where;
        }

        public void Add(Player p)
        {
            this.Players.Add(p);
            if (this.currentPlayer == null)
            {
                this.currentPlayer = p;
            }
        }

        public void Add(Location l)
        {
            this.Board.Add(l);
        }

        public Player Next_Player()
        {
            int idx = (this.Players.IndexOf(this.currentPlayer) + 1) % this.Players.Count;
            this.currentPlayer = this.Players[idx];
            if (this.currentPlayer.Balance <= 0 && this.Players.Count > 1)
            {
                this.currentPlayer = this.Next_Player();
            }
            
            return this.currentPlayer;
        }

        public Player Prev_Player()
        {
            int idx = (this.Players.IndexOf(this.currentPlayer) + this.Players.Count - 1)
                % this.Players.Count;
            return this.Players[idx];
        }

        public Location Next_Location(Location l, ref string notices, bool transit)
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

        public Location Prev_Location(Location l)
        {
            int idx = this.Board.IndexOf(l) - 1;
            if (idx < 0)
            {
                idx = this.Board.Count - 1;
            }
            
            return this.Board[idx];
        }
    }
}