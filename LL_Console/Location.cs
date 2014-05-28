// <copyright file="Location.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class Location
    {
        private static List<Location> board = null;
        private int left = -1;
        private int right = -1;
        private int top = -1;
        private int bottom = -1;
        private string name = string.Empty;
        private int salary = 0;
        private int salaryOver = 0;
        private int priceSale = 0;
        private int priceRent = 0;
        private int multiplier = 1;
        private bool jail = false;
        private Zoning propertyType = Zoning.Unknown;
        private Player owner = null;

        private List<Zoning> ownables = new List<Zoning>()
        { 
            Zoning.Residential,
            Zoning.Railroad,
            Zoning.Franchise
        };

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
            this.propertyType = zone;
            this.name = name;
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
            this.priceSale = sale;
            this.priceRent = rent;
        }

        public Location(XmlNode prop)
        {
            string type = string.Empty;

            XmlHelper.StringFromXmlIfExists(prop, "Name", ref this.name);
            XmlHelper.IntFromXmlIfExists(prop, "PriceSale", ref this.priceSale);
            XmlHelper.IntFromXmlIfExists(prop, "PriceRent", ref this.priceRent);
            XmlHelper.IntFromXmlIfExists(prop, "Tax", ref this.priceRent);
            XmlHelper.IntFromXmlIfExists(prop, "Multiplier", ref this.multiplier);
            XmlHelper.IntFromXmlIfExists(prop, "xLeft", ref this.left);
            XmlHelper.IntFromXmlIfExists(prop, "xRight", ref this.right);
            XmlHelper.IntFromXmlIfExists(prop, "yTop", ref this.top);
            XmlHelper.IntFromXmlIfExists(prop, "yBottom", ref this.bottom);
            XmlHelper.IntFromXmlIfExists(prop, "Salary", ref this.salary);
            XmlHelper.IntFromXmlIfExists(prop, "SalaryOver", ref this.salaryOver);
            XmlHelper.BoolFromXmlIfExists(prop, "SalaryOver", ref this.jail);

            try
            {
                XmlHelper.StringFromXmlIfExists(prop, "PropertyType", ref type);
                this.PropertyType = (Zoning)Enum.Parse(Zoning.Park.GetType(), type);
            }
            catch
            {
            }
        }

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

        public static List<Location> Board
        {
            private get
            {
                return board;
            }

            set
            {
                board = value;
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

        public int Left
        {
            set
            {
                this.left = value;
            }
        }

        public int Right
        {
            set
            {
                this.right = value;
            }
        }

        public int Top
        {
            set
            {
                this.top = value;
            }
        }

        public int Bottom
        {
            set
            {
                this.bottom = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int Salary
        {
            get
            {
                return this.salary;
            }

            set
            {
                this.salary = value;
            }
        }

        public int SalaryOver
        {
            get
            {
                return this.salaryOver;
            }

            set
            {
                this.salaryOver = value;
            }
        }

        public int PriceSale
        {
            get
            {
                return this.priceSale;
            }

            set
            {
                this.priceSale = value;
            }
        }

        public int PriceRent
        {
            get
            {
                return this.priceRent;
            }

            set
            {
                this.priceRent = value;
            }
        }

        public int Multiplier
        {
            get
            {
                return this.multiplier;
            }

            set
            {
                this.multiplier = value;
            }
        }

        public bool Jail
        {
            get
            {
                return this.jail;
            }

            set
            {
                this.jail = value;
            }
        }

        public Player Owner
        {
            get
            {
                return this.owner;
            }

            set
            {
                this.owner = value;
            }
        }

        private Zoning PropertyType
        {
            get
            {
                return this.propertyType;
            }

            set
            {
                this.propertyType = value;
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