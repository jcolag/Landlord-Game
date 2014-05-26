namespace LL_Console
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class Player
    {
        static int StartingBalance = 1500;
        static int PlayerNumber = 1;
        static string BasePlayerName = "Player";
        private string _name = string.Empty;

        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        private int _balance = 0;

        public int Balance
        {
            get
            {
                return this._balance;
            }

            private set
            {
                this._balance = value;
            }
        }

        public bool _inJail = false;

        public bool InJail
        {
            get
            {
                return this._inJail;
            }

            set
            {
                this._inJail = value;
            }
        }

        private List<Asset> _assets = new List<Asset>();

        private List<Asset> Assets
        {
            get
            {
                return this._assets;
            }
        }

        private Location _where;

        public Location Where
        {
            get
            {
                return this._where;
            }

            set
            {
                this._where = value;
            }
        }

        private int AssetIndex = 0;

        public Player(XmlNode node)
        {
            XmlHelper.StringFromXmlIfExists(node, "Name", ref this._name);
            this.Balance = StartingBalance;
            XmlHelper.IntFromXmlIfExists(node, "Balance", ref this._balance);
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

        private void InitPlayer(string name, int balance)
        {
            this.Name = name;
            this.Balance = balance;
            PlayerNumber += 1;
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
            while (this.Balance <= 0 && this.AssetIndex < this.Assets.Count)
            {
                int v = this.Assets[AssetIndex].Value;
                this.AssetIndex += 1;
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
    }
}
