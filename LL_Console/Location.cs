using System;
using System.Collections.Generic;
using System.Xml;

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

		public Location () {
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

		public Location (XmlNode prop) {
			string type = string.Empty;

			XmlHelper.StringFromXmlIfExists (prop, "Name", ref _name);
			XmlHelper.IntFromXmlIfExists (prop, "PriceSale", ref _priceSale);
			XmlHelper.IntFromXmlIfExists (prop, "PriceRent", ref _priceRent);
			XmlHelper.IntFromXmlIfExists (prop, "xLeft", ref _xLeft);
			XmlHelper.IntFromXmlIfExists (prop, "xRight", ref _xRight);
			XmlHelper.IntFromXmlIfExists (prop, "yTop", ref _yTop);
			XmlHelper.IntFromXmlIfExists (prop, "yBottom", ref _yBottom);

			XmlHelper.StringFromXmlIfExists (prop, "PropertyType", ref type);
			if (!string.IsNullOrWhiteSpace (type)) {
				Enum.Parse (Location.Zoning.Park.GetType(), type);
			}
		}
	}
}

