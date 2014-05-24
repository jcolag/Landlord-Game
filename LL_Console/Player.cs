using System;
using System.Collections.Generic;
using System.Xml;

namespace LL_Console
{
	public class Player
	{
		static int StartingBalance = 1500;
		static int PlayerNumber = 1;
		static string BasePlayerName = "Player";

		private string _name = string.Empty;
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		private int _balance = 0;
		public int Balance {
			get {
				return _balance;
			}
			private set {
				_balance = value;
			}
		}

		public bool _inJail = false;
		public bool InJail {
			get {
				return _inJail;
			}
			set {
				_inJail = value;
			}
		}

		private List<Asset> _assets = new List<Asset>();
		private List<Asset> Assets {
			get {
				return _assets;
			}
		}

		private Location _where;
		public Location Where {
			get {
				return _where;
			}
			set {
				_where = value;
			}
		}

		private int AssetIndex = 0;

		public Player (XmlNode node)
		{
			XmlHelper.StringFromXmlIfExists (node, "Name", ref _name);
			Balance = StartingBalance;
			XmlHelper.IntFromXmlIfExists (node, "Balance", ref _balance);
			PlayerNumber += 1;
		}

		public Player ()
		{
			InitPlayer(BasePlayerName + " #" + PlayerNumber.ToString(), StartingBalance);
		}

		public Player (string name)
		{
			InitPlayer(name, StartingBalance);
		}

		public Player (string name, int balance)
		{
			InitPlayer(name, balance);
		}

		private void InitPlayer (string name, int balance)
		{
			Name = name;
			Balance = balance;
			PlayerNumber += 1;
		}

		public int AddAsset (string name)
		{
			return AddAsset (name, Asset.BaseValue);
		}

		public int AddAsset(string name, int value)
		{
			Asset a = new Asset(name, value);
			Assets.Add (a);
			Balance -= value;
			return Assets.Count;
		}

		public int AddAssets (List<string> names)
		{
			foreach (string name in names) {
				AddAsset (name);
			}
			return Assets.Count;
		}

		public int Deposit (int amount)
		{
			Balance += amount;
			return Balance;
		}

		public int Withdraw (int amount)
		{
			Balance -= amount;
			while (Balance <= 0 && AssetIndex < Assets.Count) {
				int v = Assets[AssetIndex].Value;
				AssetIndex += 1;
				Balance += v;
			}
			return Balance;
		}

		public override string ToString()
		{
			return string.Format("Player {0} ({1}), starting at {2}", Name, Balance.ToString(), Where.Name);
		}
	}
}
