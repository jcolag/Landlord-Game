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
        public class Program
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
                        string player_name = string.Empty;

                        foreach (string arg in args)
                        {
                                if (arg.ToLower().EndsWith(".xml"))
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
                                        XmlHelper.FromXmlIfExists<int>(dice[0], "Count", ref diceCount);
                                        XmlHelper.FromXmlIfExists<int>(dice[0], "Sides", ref diceSides);
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
                        }
                        else
                        {
                                for (int i = 0; i < 40; i++)
                                {
                                        var l = new Location(
                                                Location.Zoning.Residential,
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

                        if (g.Players.Count < 2)
                        {
                                Console.WriteLine("Input player names, empty line when done:");
                                while (true)
                                {
                                        player_name = Console.ReadLine();
                                        if (string.IsNullOrWhiteSpace(player_name))
                                        {
                                                break;
                                        }

                                        var p = new Player(player_name);
                                        p.Where = g.Board[0];
                                        g.Add(p);
                                }
                        }

                        Player player = g.Who();
                        while (g.Continue)
                        {
                                Location.AnswerQuestion question = null;
                                Location landing = g.Where();
                                string answer = string.Empty;

                                Console.WriteLine("* " + player.ToString());
                                if (player.InJail)
                                {
                                        Console.WriteLine(player.Name + " is in jail!");
                                }

                                Console.WriteLine("Rolls a " + g.Roll(ref answer).ToString());
                                Console.Write(answer);
                                landing = g.Where();
                                Console.WriteLine("Lands on " + landing.Name);

                                Console.WriteLine(landing.PrintOnLanding(player, ref question));
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
