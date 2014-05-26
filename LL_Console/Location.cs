namespace LL_Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class Location
    {
        public delegate string AnswerQuestion(Player p, string ans);

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
            Unknown
        }

        private List<Zoning> ownables = new List<Zoning>() { 
            Zoning.Residential, Zoning.Railroad, Zoning.Franchise
        };
        private static List<Location> _board = null;

        public static List<Location> Board
        {
            private get
            {
                return _board;
            }
            
            set
            {
                _board = value;
            }
        }

        public bool Ownable
        {
            get
            {
                return this.ownables.Contains(this.PropertyType);
            }
        }

        public bool CanBuy
        {
            get
            {
                return this.Ownable && this.Owner == null;
            }
        }

        private int _xLeft = -1;

        public int xLeft
        {
            set
            {
                this._xLeft = value;
            }
        }

        private int _xRight = -1;

        public int xRight
        {
            set
            {
                this._xRight = value;
            }
        }

        private int _yTop = -1;

        public int yTop
        {
            set
            {
                this._yTop = value;
            }
        }

        private int _yBottom = -1;

        public int yBottom
        {
            set
            {
                this._yBottom = value;
            }
        }

        private string _name = string.Empty;

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        private int _salary = 0;

        public int Salary
        {
            get
            {
                return this._salary;
            }
            
            set
            {
                this._salary = value;
            }
        }

        private int _salaryOver = 0;

        public int SalaryOver
        {
            get
            {
                return this._salaryOver;
            }
            
            set
            {
                this._salaryOver = value;
            }
        }

        private int _priceSale = 0;

        public int PriceSale
        {
            get
            {
                return this._priceSale;
            }
            
            set
            {
                this._priceSale = value;
            }
        }

        private int _priceRent = 0;

        public int PriceRent
        {
            get
            {
                return this._priceRent;
            }
            
            set
            {
                this._priceRent = value;
            }
        }

        private int _multiplier = 1;

        public int Multiplier
        {
            get
            {
                return this._multiplier;
            }
            
            set
            {
                this._multiplier = value;
            }
        }

        private bool _jail = false;

        public bool Jail
        {
            get
            {
                return this._jail;
            }
            
            set
            {
                this._jail = value;
            }
        }

        private Zoning _propertyType = Zoning.Unknown;

        private Zoning PropertyType
        {
            get
            {
                return this._propertyType;
            }
            
            set
            {
                this._propertyType = value;
            }
        }

        private Player _owner = null;

        public Player Owner
        {
            get
            {
                return this._owner;
            }
            
            set
            {
                this._owner = value;
            }
        }

        public Location()
        {
        }

        public Location(
            Zoning zone,
            string name,
            int left,
            int right,
            int top,
            int bottom,
            int sale,
            int rent)
        {
            this._propertyType = zone;
            this._name = name;
            this.xLeft = left;
            this.xRight = right;
            this.yTop = top;
            this.yBottom = bottom;
            this._priceSale = sale;
            this._priceRent = rent;
        }

        public Location(XmlNode prop)
        {
            string type = string.Empty;

            XmlHelper.StringFromXmlIfExists(prop, "Name", ref this._name);
            XmlHelper.IntFromXmlIfExists(prop, "PriceSale", ref this._priceSale);
            XmlHelper.IntFromXmlIfExists(prop, "PriceRent", ref this._priceRent);
            XmlHelper.IntFromXmlIfExists(prop, "Tax", ref this._priceRent);
            XmlHelper.IntFromXmlIfExists(prop, "Multiplier", ref this._multiplier);
            XmlHelper.IntFromXmlIfExists(prop, "xLeft", ref this._xLeft);
            XmlHelper.IntFromXmlIfExists(prop, "xRight", ref this._xRight);
            XmlHelper.IntFromXmlIfExists(prop, "yTop", ref this._yTop);
            XmlHelper.IntFromXmlIfExists(prop, "yBottom", ref this._yBottom);
            XmlHelper.IntFromXmlIfExists(prop, "Salary", ref this._salary);
            XmlHelper.IntFromXmlIfExists(prop, "SalaryOver", ref this._salaryOver);
            XmlHelper.BoolFromXmlIfExists(prop, "SalaryOver", ref this._jail);

            try
            {
                XmlHelper.StringFromXmlIfExists(prop, "PropertyType", ref type);
                this.PropertyType = (Zoning)Enum.Parse(Zoning.Park.GetType(), type);
            }
            catch
            {
            }
        }

        public string PassBy(Player p)
        {
            string result = string.Empty;
            if (this.PropertyType == Zoning.MotherEarth && this.SalaryOver > 0)
            {
                p.Deposit(this.SalaryOver);
                result = "Earned $" + this.SalaryOver + " salary!\n";
            }
            
            return result;
        }

        public string PrintOnLanding(Player p, ref AnswerQuestion answerer)
        {
            switch (this.PropertyType)
            {
            case Zoning.Residential:
            case Zoning.Railroad:
            case Zoning.Franchise:
                if (this.CanBuy && p.Balance > this.PriceSale)
                {
                    answerer = this.BuyLocation;
                    return "Want to buy " + this.Name + " for " + this.PriceSale + "?";
                }
                else if (this.CanBuy)
                {
                    return "Can't buy " + Name + ".  Not enough money.";
                }
                else if (Ownable && Owner != p)
                {
                    int rent = PriceRent;
                    if (this.PropertyType == Zoning.Franchise || this.PropertyType == Zoning.Railroad)
                    {
                        var rrs = Board.Where(x => x.PropertyType == PropertyType && x.Owner == Owner);
                        int nrrs = rrs.Count();
                        for (int i = 0; i < nrrs; i++)
                        {
                            rent *= Multiplier;
                        }
                    }

                    answerer = RentLocation;
                    return "Owned by " + Owner.Name + ", rent is $" + PriceRent.ToString()
                        + ". [P]ay?";
                }
                else if (Ownable)
                {
                    return p.Name + " already owns " + Name + ".";
                }

                break;
            case Zoning.Luxury:
            case Zoning.Necessity:
                p.Withdraw(this.PriceRent);
                return "Tax of $" + this.PriceRent.ToString() + " levied on " + p.Name + ".";
            case Zoning.Legacy:
                int due = this.PriceRent > p.Balance / 10 ? p.Balance / 10 : this.PriceRent;
                p.Withdraw(due);
                return "Tax of $" + due.ToString() + " levied on " + p.Name + ".";
            case Zoning.Trespassing:
                p.InJail = true;
                return p.Name + " was caught trespassing by Lord Blueblood and is sent to jail!";
            case Zoning.MotherEarth:
                p.Deposit(this.Salary);
                return "Labor upon Mother Earth produces wages:  $" + this.Salary + ".";
            case Zoning.Park:
                return "Spending a day at the park.";
            }

        return string.Empty;
        }

        private string BuyLocation(Player p, string answer)
        {
            if (answer.ToLower().StartsWith("y"))
            {
                p.Withdraw(this.PriceSale);
                this.Owner = p;
                return p.Name + " now owns " + this.Name + ".";
            }

            return string.Empty;
        }

        private string RentLocation(Player p, string answer)
        {
            if (answer.ToLower().StartsWith("p"))
            {
                p.Withdraw(this.PriceRent);
                this.Owner.Deposit(this.PriceRent);
                return "Owned by " + this.Owner.Name + ", rent is $" + this.PriceRent.ToString();
            }

            return string.Empty;
        }
    }
}