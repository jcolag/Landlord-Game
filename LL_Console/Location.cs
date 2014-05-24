using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace LL_Console
{
	public class Location
	{
		public delegate string AnswerQuestion(Player p,string ans);

			public enum Zoning
			{
				Residential,
				Railroad,
				Franchise,
				Necessity,
				Luxury,
				Legacy,
				Park,
				Trespassing,
				MotherEarth,
				Unknown}
			;

			private List<Zoning> ownables = new List<Zoning>() { 
				Zoning.Residential, Zoning.Railroad, Zoning.Franchise
			};
			private static List<Location> _board = null;

			public static List<Location> Board {
				private get {
					return _board;
				}
				set {
					_board = value;
				}
			}

			public bool Ownable {
				get {
					return ownables.Contains(PropertyType);
				}
			}

			public bool CanBuy {
				get {
					return Ownable && Owner == null;
				}
			}

			private int _xLeft = -1;

			public int xLeft {
				set {
					_xLeft = value;
				}
			}

			private int _xRight = -1;

			public int xRight {
				set {
					_xRight = value;
				}
			}

			private int _yTop = -1;

			public int yTop {
				set {
					_yTop = value;
				}
			}

			private int _yBottom = -1;

			public int yBottom {
				set {
					_yBottom = value;
				}
			}

			private string _name = string.Empty;

			public string Name {
				get {
					return _name;
				}
			}

			private int _salary = 0;

			public int Salary {
				get {
					return _salary;
				}
				set {
					_salary = value;
				}
			}

			private int _salaryOver = 0;

			public int SalaryOver {
				get {
					return _salaryOver;
				}
				set {
					_salaryOver = value;
				}
			}

			private int _priceSale = 0;

			public int PriceSale {
				get {
					return _priceSale;
				}
				set {
					_priceSale = value;
				}
			}

			private int _priceRent = 0;

			public int PriceRent {
				get {
					return _priceRent;
				}
				set {
					_priceRent = value;
				}
			}

			private int _multiplier = 1;

			public int Multiplier {
				get {
					return _multiplier;
				}
				set {
					_multiplier = value;
				}
			}

			private bool _jail = false;

			public bool Jail {
				get {
					return _jail;
				}
				set {
					_jail = value;
				}
			}

			private Zoning _propertyType = Zoning.Unknown;

			private Zoning PropertyType {
				get {
					return _propertyType;
				}
				set {
					_propertyType = value;
				}
			}

			private Player _owner = null;

			public Player Owner {
				get {
					return _owner;
				}
				set {
					_owner = value;
				}
			}

			public Location()
			{
			}

			public Location(Zoning zone, string name,
			                int left, int right, int top, int bottom,
			                int sale, int rent)
			{
				_propertyType = zone;
				_name = name;
				xLeft = left;
				xRight = right;
				yTop = top;
				yBottom = bottom;
				_priceSale = sale;
				_priceRent = rent;
			}

			public Location(XmlNode prop)
			{
				string type = string.Empty;

				XmlHelper.StringFromXmlIfExists(prop, "Name", ref _name);
				XmlHelper.IntFromXmlIfExists(prop, "PriceSale", ref _priceSale);
				XmlHelper.IntFromXmlIfExists(prop, "PriceRent", ref _priceRent);
				XmlHelper.IntFromXmlIfExists(prop, "Tax", ref _priceRent);
				XmlHelper.IntFromXmlIfExists(prop, "Multiplier", ref _multiplier);
				XmlHelper.IntFromXmlIfExists(prop, "xLeft", ref _xLeft);
				XmlHelper.IntFromXmlIfExists(prop, "xRight", ref _xRight);
				XmlHelper.IntFromXmlIfExists(prop, "yTop", ref _yTop);
				XmlHelper.IntFromXmlIfExists(prop, "yBottom", ref _yBottom);
				XmlHelper.IntFromXmlIfExists(prop, "Salary", ref _salary);
				XmlHelper.IntFromXmlIfExists(prop, "SalaryOver", ref _salaryOver);
				XmlHelper.BoolFromXmlIfExists(prop, "SalaryOver", ref _jail);

				try {
					XmlHelper.StringFromXmlIfExists(prop, "PropertyType", ref type);
					PropertyType = (Zoning)Enum.Parse(Zoning.Park.GetType(), type);
				} catch {
				}
			}

			public string PassBy(Player p)
			{
				string result = string.Empty;
				if (PropertyType == Zoning.MotherEarth && SalaryOver > 0) {
					p.Deposit(SalaryOver);
					result = "Earned $" + SalaryOver + " salary!\n";
				}
				return result;
			}

			public string PrintOnLanding(Player p, ref AnswerQuestion answerer)
			{
				switch (PropertyType) {
					case Zoning.Residential:
					case Zoning.Railroad:
					case Zoning.Franchise:
						if (CanBuy && p.Balance > PriceSale) {
							answerer = BuyLocation;
							return "Want to buy " + Name + " for " + PriceSale + "?";
						} else if (CanBuy) {
							return "Can't buy " + Name + ".  Not enough money.";
						} else if (Ownable && Owner != p) {
							int rent = PriceRent;
							if (PropertyType == Zoning.Franchise || PropertyType == Zoning.Railroad) {
								var rrs = Board.Where(x => x.PropertyType == PropertyType && x.Owner == Owner);
								int nrrs = rrs.Count();
								for (int i=0; i<nrrs; i++) {
									rent *= Multiplier;
								}
							}
							answerer = RentLocation;
							return "Owned by " + Owner.Name + ", rent is $" + PriceRent.ToString()
								+ ". [P]ay?";
						} else if (Ownable) {
							return p.Name + " already owns " + Name + ".";
						}
						break;
					case Zoning.Luxury:
					case Zoning.Necessity:
						p.Withdraw(PriceRent);
						return "Tax of $" + PriceRent.ToString() + " levied on " + p.Name + ".";
					case Zoning.Legacy:
						int due = PriceRent > p.Balance / 10 ? p.Balance / 10 : PriceRent;
						p.Withdraw(due);
						return "Tax of $" + due.ToString() + " levied on " + p.Name + ".";
					case Zoning.Trespassing:
						p.InJail = true;
						return p.Name + " was caught trespassing by Lord Blueblood and is sent to jail!";
					case Zoning.MotherEarth:
						p.Deposit(Salary);
						return "Labor upon Mother Earth produces wages:  $" + Salary + ".";
					case Zoning.Park:
						return "Spending a day at the park.";
				}
				return string.Empty;
			}

			private string BuyLocation(Player p, string answer)
			{
				if (answer.ToLower().StartsWith("y")) {
					p.Withdraw(PriceSale);
					Owner = p;
					return p.Name + " now owns " + Name + ".";
				}
				return string.Empty;
			}

			private string RentLocation(Player p, string answer)
			{
				if (answer.ToLower().StartsWith("p")) {
					p.Withdraw(PriceRent);
					Owner.Deposit(PriceRent);
					return "Owned by " + Owner.Name + ", rent is $" + PriceRent.ToString();
				}
				return string.Empty;
			}
		}
	}

