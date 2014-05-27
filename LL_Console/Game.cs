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
        public List<Location> Board = new List<Location>();
        public List<Player> Players = new List<Player>();
        public List<Card> Pile1 = new List<Card>();
        public List<Card> Pile2 = new List<Card>();
        public string Pile1Name = "Chance";
        public string Pile2Name = "Community Chest";
        public int nDice = 2;
        public int xDice = 6;
        private Random rand = new Random();
        private Player current_player = null;

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

        public int Roll(ref string notices)
        {
            return this.Roll(this.current_player, ref notices);
        }

        public int Roll(Player p, ref string notices)
        {
            int dice = 0,
            i;
            Location l = p.Where;
            string note = string.Empty;

            for (i = 0; i < this.nDice; i++)
            {
                dice += this.rand.Next(1, this.xDice);
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
            return this.current_player;
        }

        public Location Where()
        {
            return this.current_player.Where;
        }

        public void Add(Player p)
        {
            this.Players.Add(p);
            if (this.current_player == null)
            {
                this.current_player = p;
            }
        }

        public void Add(Location l)
        {
            this.Board.Add(l);
        }

        public Player Next_Player()
        {
            int idx = (this.Players.IndexOf(this.current_player) + 1) % this.Players.Count;
            this.current_player = this.Players[idx];
            if (this.current_player.Balance <= 0 && this.Players.Count > 1)
            {
                this.current_player = this.Next_Player();
            }
            
            return this.current_player;
        }

        public Player Prev_Player()
        {
            int idx = (this.Players.IndexOf(this.current_player) + this.Players.Count - 1)
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
                notices = this.Board[idx].PassBy(this.current_player);
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