using System;
using System.Collections.Generic;
using System.Xml;

namespace LL_Console
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			XmlDocument gamecfg = new XmlDocument();
			Game g = new Game();
			string player_name = string.Empty;

			foreach (string arg in args) {
				if (arg.ToLower().EndsWith(".xml")) {
					gamecfg.Load(arg);
					break;
				}
			}
			if (gamecfg != null) {
				XmlNodeList dice = gamecfg.GetElementsByTagName("Dice");
				if (dice.Count > 0) {
					int nDice = g.nDice,
					xDice = g.xDice;
					XmlHelper.IntFromXmlIfExists(dice[0], "Count", ref nDice);
					XmlHelper.IntFromXmlIfExists(dice[0], "Sides", ref xDice);
					g.nDice = nDice;
					g.xDice = xDice;
				}
				XmlNodeList props = gamecfg.GetElementsByTagName("Location");
				foreach (XmlNode prop in props) {
					Location l = new Location(prop);
					g.Add(l);
				}
				XmlNodeList peeps = gamecfg.GetElementsByTagName("Player");
				foreach (XmlNode pers in peeps) {
					Player p = new Player(pers);
					p.Where = g.Board[0];
					g.Add(p);
				}
			} else {
				for (int i=0; i<40; i++) {
					Location l = new Location(Location.Zoning.Residential,
					                           "Property " + (i + 1).ToString(),
					                           0, 0, 0, 0, 100, 10);
					g.Add(l);
				}
			}
			if (g.Players.Count < 2) {
				Console.WriteLine("Input player names, empty line when done:");
				while (true) {
					player_name = Console.ReadLine();
					if (string.IsNullOrWhiteSpace(player_name)) {
						break;
					}
					Player p = new Player(player_name);
					p.Where = g.Board[0];
					g.Add(p);
				}
			}

			Player player = g.Who();
			while (g.Continue) {
				Location landing = g.Where();
				string answer = string.Empty;

				Console.WriteLine("* " + player.ToString());
				Console.WriteLine("Rolls a " + g.Roll());
				landing = g.Where();
				Console.WriteLine("Lands on " + landing.Name);

				if (landing.CanBuy) {
					Console.WriteLine("Want to buy " + landing.Name
						+ " for " + landing.PriceSale + "?");
					if (landing.PriceSale < player.Balance) {
						answer = Console.ReadLine();
						if (answer.ToLower().StartsWith("y")) {
							player.Withdraw(landing.PriceSale);
							landing.Owner = player;
						}
					} else {
						Console.WriteLine("Can't.  Not enough money.");
					}
				} else if (landing.Ownable) {
					Console.WriteLine("Owned by " + landing.Owner.Name
						+ ", rent is $"
						+ landing.PriceRent.ToString());
					player.Withdraw(landing.PriceRent);
					landing.Owner.Deposit(landing.PriceRent);
				}
				if (player.Balance <= 0) {
					Console.WriteLine(player.Name + " has gone bankrupt!");
				}
				player = g.Next_Player();
			}
			Player winner = g.Who();
			Console.WriteLine(winner.Name + " wins with $"
				+ winner.Balance.ToString());
		}
	}
}
