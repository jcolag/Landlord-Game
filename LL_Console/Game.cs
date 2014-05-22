using System;
using System.Collections.Generic;

namespace LL_Console
{
	public class Game
	{
		public List<Location> Board = new List<Location> ();
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
			get {
				int solvent = 0;
				foreach (Player p in Players) {
					if (p.Balance > 0) {
						++ solvent;
					}
				}
				return solvent > 1;
			}
		}

		public Game () {
		}

		public int Roll () {
			return Roll (current_player);
		}

		public int Roll (Player p) {
			int dice = 0,
				i;
			Location l = p.Where;

			for (i=0; i<nDice; i++) {
				dice += rand.Next (1, xDice);
			}
			for (i=0; i<dice; i++) {
				l = Next_Location (l);
			}
			p.Where = l;
			return dice;
		}

		public Player Who () {
			return current_player;
		}

		public Location Where () {
			return current_player.Where;
		}

		public void Add (Player p) {
			Players.Add (p);
			if (current_player == null) {
				current_player = p;
			}
		}

		public void Add (Location l) {
			Board.Add (l);
		}

		public Player Next_Player ()
		{
			int idx = (Players.IndexOf (current_player) + 1) % Players.Count;
			current_player = Players [idx];
			if (current_player.Balance <= 0 && Players.Count > 1) {
				current_player = Next_Player ();
			}
			return current_player;
		}

		public Player Prev_Player ()
		{
			int idx = (Players.IndexOf (current_player) + Players.Count - 1)
				% Players.Count;
			return Players[idx];
		}
		
		public Location Next_Location (Location l)
		{
			int idx = Board.IndexOf (l) + 1;
			if (idx >= Board.Count) {
				idx = 0;
			}
			return Board[idx];
		}

		public Location Prev_Location (Location l)
		{
			int idx = Board.IndexOf (l) - 1;
			if (idx < 0) {
				idx = Board.Count - 1;
			}
			return Board[idx];
		}
	}
}

