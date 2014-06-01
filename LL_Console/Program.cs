// <copyright file="Program.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
        using System;
        using System.Xml;

        /// <summary>
        /// Program.
        /// If that isn't self-explanatory, you shouldn't be reading this.
        /// </summary>
        internal static class Program
        {
                /// <summary>
                /// The entry point of the program, where the program control starts and ends.
                /// Initializes the game, preferably from an XML file, and goes into the main
                /// loop.
                /// </summary>
                /// <param name="args">The command-line arguments.</param>
                public static void Main(string[] args)
                {
                        var gamecfg = new XmlDocument();
                        var g = new Game();
                        string playerName = string.Empty;

                        foreach (string arg in args)
                        {
                                if (arg.ToLower(System.Globalization.CultureInfo.CurrentCulture).EndsWith(".xml", StringComparison.CurrentCulture))
                                {
                                        gamecfg.Load(arg);
                                        break;
                                }
                        }

                        if (gamecfg != null)
                        {
                                XmlNodeList dice = gamecfg.GetElementsByTagName("Dice");
                                if (dice.Count > 0)
                                {
                                        int diceCount = g.DiceCount;
                                        int diceSides = g.DiceSides;
                                        diceCount = XmlHelper.FromXmlIfExists<int>(dice[0], "Count", diceCount);
                                        diceSides = XmlHelper.FromXmlIfExists<int>(dice[0], "Sides", diceSides);
                                        g.DiceCount = diceCount;
                                        g.DiceSides = diceSides;
                                }

                                XmlNodeList props = gamecfg.GetElementsByTagName("Location");
                                foreach (XmlNode prop in props)
                                {
                                        var l = new Location(prop);
                                        g.Add(l);
                                }

                                XmlNodeList peeps = gamecfg.GetElementsByTagName("Player");
                                foreach (XmlNode pers in peeps)
                                {
                                        var p = new Player(pers);
                                        p.Where = g.Board[0];
                                        g.Add(p);
                                }

                                XmlNodeList cards = gamecfg.GetElementsByTagName("FirstCard");
                                foreach (XmlNode card in cards)
                                {
                                        var c = new Card(card);
                                        g.Add(c, false);
                                }
                                
                                cards = gamecfg.GetElementsByTagName("SecondCard");
                                foreach (XmlNode card in cards)
                                {
                                        var c = new Card(card);
                                        g.Add(c, true);
                                }
                        }
                        else
                        {
                                for (int i = 0; i < 40; i++)
                                {
                                        var l = new Location(
                                                Zoning.Residential,
                                                "Property " + (i + 1).ToString(),
                                                0,
                                                0,
                                                0,
                                                0,
                                                100,
                                                10);
                                        g.Add(l);
                                }
                        }

                        if (g.PlayerCount < 2)
                        {
                                Console.WriteLine("Input player names, empty line when done:");
                                while (true)
                                {
                                        playerName = Console.ReadLine();
                                        if (string.IsNullOrWhiteSpace(playerName))
                                        {
                                                break;
                                        }

                                        var p = new Player(playerName);
                                        p.Where = g.Board[0];
                                        g.Add(p);
                                }
                        }

                        Player player = g.Who();
                        while (g.Continue)
                        {
                                Func<Player, string, string> question = null;
                                Location landing = g.Where();
                                string answer = string.Empty;

                                Console.WriteLine("* " + player.ToString());
                                if (player.InJail)
                                {
                                        Console.WriteLine(player.Name + " is in jail!");
                                }

                                Console.WriteLine("Rolls a " + g.Roll(out answer).ToString());
                                Console.Write(answer);
                                landing = g.Where();
                                Console.WriteLine("Lands on " + landing.Name);

                                Console.WriteLine(landing.PrintOnLanding(player, ref question));
                                answer = string.Empty;
                                while (question != null && string.IsNullOrWhiteSpace(answer))
                                {
                                        answer = Console.ReadLine();
                                        answer = question(player, answer);
                                        Console.WriteLine(answer);
                                }

                                if (player.Balance <= 0)
                                {
                                        Console.WriteLine(player.Name + " has gone bankrupt!");
                                }

                                player = g.NextPlayer();
                        }

                        Player winner = g.Who();
                        Console.WriteLine(winner.Name + " wins with $"
                                + winner.Balance.ToString());
                }
        }
}
