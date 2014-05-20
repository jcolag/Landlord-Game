using System;
using System.Collections.Generic;

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

		public Player ()
		{
			Name = BasePlayerName + " #" + PlayerNumber.ToString ();
			PlayerNumber += 1;
			Balance = StartingBalance;
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
	}
}
