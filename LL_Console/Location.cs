using System;
using System.Collections.Generic;

namespace LL_Console
{
	public class Location
	{
		public enum Zoning {
			Residential,
			Railroad,
			Franchise,
			Necessity,
			Luxury,
			Legacy,
			Park,
			Trespassing,
			MotherEarth,
			Unknown
		};

		private int xLeft = -1;
		private int xRight = -1;
		private int yTop = -1;
		private int yBottom = -1;
		private string _name = string.Empty;
		public string Name {
			get {
				return _name;
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

		private Zoning _propertyType = Zoning.Unknown;
		private Player _owner = null;
		public Player Owner {
			get {
				return _owner;
			}
			set {
				_owner = value;
			}
		}

		public Location (Zoning zone, string name,
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
	}
}

