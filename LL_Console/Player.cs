// <copyright file="Player.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
        using System;
        using System.Collections.Generic;
        using System.Xml;

        /// <summary>
        /// Player represents a player of the game.
        /// </summary>
        public class Player
        {
                /// <summary>
                /// The default starting balance for each player.
                /// </summary>
                private static int startingBalance = 1500;

                /// <summary>
                /// The player's internal number.
                /// </summary>
                private static int playerNumber = 1;

                /// <summary>
                /// The default base name of the each player.
                /// </summary>
                private static string basePlayerName = "Player";

                /// <summary>
                /// The assets owned by the player.
                /// </summary>
                private List<Asset> assets = new List<Asset>();

                /// <summary>
                /// The player's name.
                /// </summary>
                private string name = string.Empty;

                /// <summary>
                /// The player's cash balance.
                /// </summary>
                private int balance;

                /// <summary>
                /// Whether the player is in jail.
                /// </summary>
                private bool inJail;

                /// <summary>
                /// The location where the player currently is.
                /// </summary>
                private Location where;

                /// <summary>
                /// The index of the asset of interest.
                /// </summary>
                private int assetIndex;

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Player"/> class.
                /// </summary>
                /// <param name="node">XML node with player information.</param>
                public Player(XmlNode node)
                {
                        this.Name = XmlHelper.FromXmlIfExists<string>(node, "Name", this.name);
                        this.Balance = XmlHelper.FromXmlIfExists<int>(node, "Balance", StartingBalance);
                        PlayerNumber += 1;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Player"/> class.
                /// </summary>
                public Player()
                {
                        this.InitPlayer(BasePlayerName + " #" + PlayerNumber.ToString(), StartingBalance);
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Player"/> class.
                /// </summary>
                /// <param name="playerName">The player's name.</param>
                public Player(string playerName)
                {
                        this.InitPlayer(playerName, StartingBalance);
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Player"/> class.
                /// </summary>
                /// <param name="playerName">The player's name.</param>
                /// <param name="playerBalance">The player's starting balance.</param>
                public Player(string playerName, int playerBalance)
                {
                        this.InitPlayer(playerName, playerBalance);
                }

                /// <summary>
                /// Gets or sets the starting balance.
                /// </summary>
                /// <value>The starting balance.</value>
                public static int StartingBalance
                {
                        get
                        {
                                return Player.startingBalance;
                        }

                        set
                        {
                                Player.startingBalance = value;
                        }
                }

                /// <summary>
                /// Gets or sets the player number.
                /// </summary>
                /// <value>The player number.</value>
                public static int PlayerNumber
                {
                        get
                        {
                                return Player.playerNumber;
                        }

                        set
                        {
                                Player.playerNumber = value;
                        }
                }

                /// <summary>
                /// Gets or sets the base name of the players.
                /// </summary>
                /// <value>The name of the base player.</value>
                public static string BasePlayerName
                {
                        get
                        {
                                return Player.basePlayerName;
                        }

                        set
                        {
                                Player.basePlayerName = value;
                        }
                }

                /// <summary>
                /// Gets or sets the player's name.
                /// </summary>
                /// <value>The player's name.</value>
                public string Name
                {
                        get
                        {
                                return this.name;
                        }

                        set
                        {
                                this.name = value;
                        }
                }

                /// <summary>
                /// Gets the player's balance.
                /// </summary>
                /// <value>The player's balance.</value>
                public int Balance
                {
                        get
                        {
                                return this.balance;
                        }

                        private set
                        {
                                this.balance = value;
                        }
                }

                /// <summary>
                /// Gets or sets a value indicating whether this <see cref="LlConsole.Player"/> is in jail.
                /// </summary>
                /// <value><c>true</c> if in jail; otherwise, <c>false</c>.</value>
                public bool InJail
                {
                        get
                        {
                                return this.inJail;
                        }

                        set
                        {
                                this.inJail = value;
                        }
                }

                /// <summary>
                /// Gets or sets the player's current location.
                /// </summary>
                /// <value>The current location.</value>
                public Location Where
                {
                        get
                        {
                                return this.where;
                        }

                        set
                        {
                                this.where = value;
                        }
                }

                /// <summary>
                /// Gets a list of the player's assets.
                /// </summary>
                /// <value>The assets.</value>
                private List<Asset> Assets
                {
                        get
                        {
                                return this.assets;
                        }
                }

                /// <summary>
                /// Adds the asset to the player's stash.
                /// </summary>
                /// <returns>The number of assets.</returns>
                /// <param name="assetName">The asset's name.</param>
                public int AddAsset(string assetName)
                {
                        return this.AddAsset(assetName, Asset.BaseValue);
                }

                /// <summary>
                /// Adds the asset to the player's stash.
                /// </summary>
                /// <returns>The number of assets.</returns>
                /// <param name="assetName">The asset's name.</param>
                /// <param name="value">The asset's value.</param>
                public int AddAsset(string assetName, int value)
                {
                        Asset a = new Asset(assetName, value);
                        this.Assets.Add(a);
                        this.Balance -= value;
                        return this.Assets.Count;
                }

                /// <summary>
                /// Deposit the specified amount to the player's balance.
                /// </summary>
                /// <returns>The new balance.</returns>
                /// <param name="amount">The amount to deposit.</param>
                public int Deposit(int amount)
                {
                        this.Balance += amount;
                        return this.Balance;
                }

                /// <summary>
                /// Withdraw the specified amount from the player's balance.
                /// </summary>
                /// <returns>The new balance.</returns>
                /// <param name="amount">The amount to withdraw.</param>
                public int Withdraw(int amount)
                {
                        this.Balance -= amount;
                        while (this.Balance <= 0 && this.assetIndex < this.Assets.Count)
                        {
                                int v = this.Assets[this.assetIndex].Value;
                                this.assetIndex += 1;
                                this.Balance += v;
                        }

                        return this.Balance;
                }

                /// <summary>
                /// Returns a <see cref="System.String"/> that represents the
                /// current <see cref="LlConsole.Player"/>.
                /// </summary>
                /// <returns>A <see cref="System.String"/> that represents the
                /// current <see cref="LlConsole.Player"/>.</returns>
                public override string ToString()
                {
                        return string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                "Player {0} ({1}), starting at {2}",
                                this.Name,
                                this.Balance.ToString(),
                                this.Where.Name);
                }

                /// <summary>
                /// Initializess the player.
                /// </summary>
                /// <param name="playerName">The player's name.</param>
                /// <param name="playerBalance">The player's starting balance.</param>
                private void InitPlayer(string playerName, int playerBalance)
                {
                        this.Name = playerName;
                        this.Balance = playerBalance;
                        PlayerNumber += 1;
                }
        }
}
