using System;
using System.Collections.Generic;

namespace LL_Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Game g = new Game ();
			string player_name = string.Empty;

			for (int i=0; i<40; i++) {
				Location l = new Location (Location.Zoning.Residential,
				                           "Property " + (i + 1).ToString (),
				                           0, 0, 0, 0, 100, 10);
				g.Add (l);
			}
			Console.WriteLine ("Input player names, empty line when done:");
			while (true) {
				player_name = Console.ReadLine ();
				if (string.IsNullOrWhiteSpace (player_name)) {
					break;
				}
				Player p = new Player ();
				p.Name = player_name;
				p.Where = g.Board [0];
				g.Add (p);
			}
			Player player = g.Who ();
			while (true) {
				Location landing = g.Where ();
				string answer = string.Empty;

				Console.WriteLine ("* " + player.Name + " ("
				                   + player.Balance.ToString() + "):");
				Console.WriteLine ("Starting from " + landing.Name);
				Console.WriteLine ("Rolls a " + g.Roll ());
				landing = g.Where ();
				Console.WriteLine ("Lands on " + landing.Name);

				if (landing.Owner == null) {
					Console.WriteLine ("Want to buy " + landing.Name + "?");
					if (landing.PriceSale < player.Balance) {
						answer = Console.ReadLine ();
						if (answer.ToLower ().StartsWith ("y")) {
							player.Withdraw (landing.PriceSale);
							landing.Owner = player;
						}
					} else {
						Console.WriteLine ("Can't.  Not enough money.");
					}
				} else {
					player.Withdraw (landing.PriceRent);
					landing.Owner.Deposit (landing.PriceRent);
				}
				Console.WriteLine ("Continue?");
				answer = Console.ReadLine ();
				if (!answer.ToLower ().StartsWith ("y")) {
					break;
				}
				player = g.Next_Player ();
			}
		}
	}
}
