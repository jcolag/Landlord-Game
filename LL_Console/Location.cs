// <copyright file="Location.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Xml;

        /// <summary>
        /// Location represents game board positions.
        /// </summary>
        public class Location
        {
                /// <summary>
                /// The board.
                /// </summary>
                private static List<Location> board = null;

                /// <summary>
                /// The left coordinate of the location's box.
                /// </summary>
                private int left = -1;

                /// <summary>
                /// The right coordinate of the location's box.
                /// </summary>
                private int right = -1;

                /// <summary>
                /// The top coordinate of the location's box.
                /// </summary>
                private int top = -1;

                /// <summary>
                /// The bottom coordinate of the location's box.
                /// </summary>
                private int bottom = -1;

                /// <summary>
                /// The name of the location.
                /// </summary>
                private string name = string.Empty;

                /// <summary>
                /// The salary paid to the player landing this location.
                /// </summary>
                private int salary = 0;

                /// <summary>
                /// The salary paid to the player passing over this location.
                /// </summary>
                private int salaryOver = 0;

                /// <summary>
                /// The price to buy the property.
                /// </summary>
                private int priceSale = 0;

                /// <summary>
                /// The price to rent (land on) the property.
                /// </summary>
                private int priceRent = 0;

                /// <summary>
                /// The multiplier to the rental price if multiple properties of
                /// the same type are owned by the player.
                /// </summary>
                private int multiplier = 1;

                /// <summary>
                /// Whether the property also represents a jail.
                /// </summary>
                private bool jail = false;

                /// <summary>
                /// The type of the property.
                /// </summary>
                private Zoning propertyType = Zoning.Unknown;

                /// <summary>
                /// The owner of the property.
                /// </summary>
                private Player owner = null;

                /// <summary>
                /// The kinds of properties that can be owned by a player.
                /// </summary>
                private List<Zoning> ownables = new List<Zoning>()
                { 
                        Zoning.Residential,
                        Zoning.Railroad,
                        Zoning.Franchise
                };

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Location"/> class.
                /// </summary>
                public Location()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Location"/> class.
                /// </summary>
                /// <param name="zone">The kind of property.</param>
                /// <param name="name">The name of the property.</param>
                /// <param name="left">Left coordinate.</param>
                /// <param name="right">Right coordinate.</param>
                /// <param name="top">Top coordinate.</param>
                /// <param name="bottom">Bottom coordinate.</param>
                /// <param name="sale">Sale price.</param>
                /// <param name="rent">Rental price.</param>
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

                /// <summary>
                /// Initializes a new instance of the <see cref="LlConsole.Location"/> class.
                /// </summary>
                /// <param name="prop">XML node with property information.</param>
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

                /// <summary>
                /// Delegate to handle player answers to questions generated by a location.
                /// </summary>
                /// <param name="p">The player.</param>
                /// <param name="ans">The player's answer.</param>
                /// <returns>A printable response to the answer</returns>
                public delegate string AnswerQuestion(Player p, string ans);

                /// <summary>
                /// Different kinds of properties, representing their role in the game.
                /// </summary>
                public enum Zoning
                {
                        /// <summary>
                        /// The residential properties.
                        /// </summary>
                        Residential,

                        /// <summary>
                        /// The railroads.
                        /// </summary>
                        Railroad,

                        /// <summary>
                        /// The franchises, such as utilities.
                        /// </summary>
                        Franchise,

                        /// <summary>
                        /// The necessities or taxes on them.
                        /// </summary>
                        Necessity,

                        /// <summary>
                        /// The luxury taxes.
                        /// </summary>
                        Luxury,

                        /// <summary>
                        /// The legacy income/estate taxes.
                        /// </summary>
                        Legacy,

                        /// <summary>
                        /// The parks.
                        /// </summary>
                        Park,

                        /// <summary>
                        /// The private properties where a player is trespassing.
                        /// </summary>
                        Trespassing,

                        /// <summary>
                        /// The mother earth space that provides revenue.
                        /// </summary>
                        MotherEarth,

                        /// <summary>
                        /// The unknown or otherwise undefined spaces.
                        /// </summary>
                        Unknown
                }

                /// <summary>
                /// Gets or sets the board.  Get is internal-use only.
                /// </summary>
                /// <value>The board.</value>
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

                /// <summary>
                /// Gets a value indicating whether this <see cref="LlConsole.Location"/> is ownable.
                /// </summary>
                /// <value><c>true</c> if ownable; otherwise, <c>false</c>.</value>
                public bool Ownable
                {
                        get
                        {
                                return this.ownables.Contains(this.PropertyType);
                        }
                }

                /// <summary>
                /// Gets a value indicating whether a player can buy this property.
                /// </summary>
                /// <value><c>true</c> if this instance can buy; otherwise, <c>false</c>.</value>
                public bool CanBuy
                {
                        get
                        {
                                return this.Ownable && this.Owner == null;
                        }
                }

                /// <summary>
                /// Sets the left coordinate.
                /// </summary>
                /// <value>The left.</value>
                public int Left
                {
                        set
                        {
                                this.left = value;
                        }
                }

                /// <summary>
                /// Sets the right coordinate.
                /// </summary>
                /// <value>The right.</value>
                public int Right
                {
                        set
                        {
                                this.right = value;
                        }
                }

                /// <summary>
                /// Sets the top coordinate.
                /// </summary>
                /// <value>The top.</value>
                public int Top
                {
                        set
                        {
                                this.top = value;
                        }
                }

                /// <summary>
                /// Sets the bottom coordinate.
                /// </summary>
                /// <value>The bottom.</value>
                public int Bottom
                {
                        set
                        {
                                this.bottom = value;
                        }
                }

                /// <summary>
                /// Gets the property name.
                /// </summary>
                /// <value>The name.</value>
                public string Name
                {
                        get
                        {
                                return this.name;
                        }
                }

                /// <summary>
                /// Gets or sets the salary for landing on this location.
                /// </summary>
                /// <value>The salary.</value>
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

                /// <summary>
                /// Gets or sets the salary for passing over this location.
                /// </summary>
                /// <value>The salary over.</value>
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

                /// <summary>
                /// Gets or sets the price to buy the property.
                /// </summary>
                /// <value>The price sale.</value>
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

                /// <summary>
                /// Gets or sets the price to rent (land on) this property.
                /// </summary>
                /// <value>The price rent.</value>
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

                /// <summary>
                /// Gets or sets the multiplier on rent if the same player owns multiple
                /// properties of the same type.
                /// </summary>
                /// <value>The multiplier.</value>
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

                /// <summary>
                /// Gets or sets a value indicating whether this <see cref="LlConsole.Location"/> is a jail.
                /// </summary>
                /// <value><c>true</c> if jail; otherwise, <c>false</c>.</value>
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

                /// <summary>
                /// Gets or sets the owner of the property.
                /// </summary>
                /// <value>The owner.</value>
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

                /// <summary>
                /// Gets or sets the type of the property.
                /// </summary>
                /// <value>The type of the property.</value>
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

                /// <summary>
                /// Handle situations where a player passes a location but does not land there.
                /// </summary>
                /// <returns>A printable string documenting what happened.</returns>
                /// <param name="p">THe player.</param>
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

                /// <summary>
                /// Message to print when the player lands on the location.
                /// </summary>
                /// <returns>The on-landing message documenting what happened.</returns>
                /// <param name="p">The player.</param>
                /// <param name="answerer">
                /// Routine that can handle the question's answer if the message is a question.
                /// </param>
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
                                } else if (Ownable)
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

                /// <summary>
                /// Deals with the player's decision of whether to buy the location.
                /// </summary>
                /// <returns>A message documenting what happened.</returns>
                /// <param name="p">The player.</param>
                /// <param name="answer">The player's answer.</param>
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

                /// <summary>
                /// Handles the player's response of what to do when landing on another player's property.
                /// </summary>
                /// <returns>A message documenting what happened.</returns>
                /// <param name="p">The player.</param>
                /// <param name="answer">The player's answer.</param>
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