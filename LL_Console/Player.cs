// <copyright file="Player.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class Player
    {
        private static int startingBalance = 1500;
        private static int playerNumber = 1;
        private static string basePlayerName = "Player";
        private List<Asset> assets = new List<Asset>();

        private string name = string.Empty;
        private int balance = 0;
        private bool inJail = false;
        private Location where;
        private int assetIndex = 0;

        public Player(XmlNode node)
        {
            XmlHelper.StringFromXmlIfExists(node, "Name", ref this.name);
            this.Balance = StartingBalance;
            XmlHelper.IntFromXmlIfExists(node, "Balance", ref this.balance);
            PlayerNumber += 1;
        }

        public Player()
        {
            this.InitPlayer(BasePlayerName + " #" + PlayerNumber.ToString(), StartingBalance);
        }

        public Player(string name)
        {
            this.InitPlayer(name, StartingBalance);
        }

        public Player(string name, int balance)
        {
            this.InitPlayer(name, balance);
        }

        public static int StartingBalance
        {
            get
            {
                return this.startingBalance;
            }

            set
            {
                this.startingBalance = value;
            }
        }

        public static int PlayerNumber
        {
            get
            {
                return this.playerNumber;
            }

            set
            {
                this.playerNumber = value;
            }
        }

        public static string BasePlayerName
        {
            get
            {
                return this.basePlayerName;
            }

            set
            {
                this.basePlayerName = value;
            }
        }

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

        private List<Asset> Assets
        {
            get
            {
                return this.assets;
            }
        }

        public int AddAsset(string name)
        {
            return this.AddAsset(name, Asset.BaseValue);
        }

        public int AddAsset(string name, int value)
        {
            Asset a = new Asset(name, value);
            this.Assets.Add(a);
            this.Balance -= value;
            return this.Assets.Count;
        }

        public int AddAssets(List<string> names)
        {
            foreach (string name in names)
            {
                this.AddAsset(name);
            }

            return this.Assets.Count;
        }

        public int Deposit(int amount)
        {
            this.Balance += amount;
            return this.Balance;
        }

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

        public override string ToString()
        {
            return string.Format(
                "Player {0} ({1}), starting at {2}",
                this.Name,
                this.Balance.ToString(),
                this.Where.Name);
        }
        
        private void InitPlayer(string name, int balance)
        {
            this.Name = name;
            this.Balance = balance;
            PlayerNumber += 1;
        }
    }
}
