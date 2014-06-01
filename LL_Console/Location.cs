// <copyright file="Location.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
        using System;
        using System.Collections.Generic;
        using System.Collections.ObjectModel;
        using System.Linq;
        using System.Xml;

        /// <summary>
        /// Different kinds of properties, representing their role in the game.
        /// </summary>
        [Serializable]
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
        /// Location represents game board positions.
        /// </summary>
        public class Location
        {
                /// <summary>
                /// The board.
                /// </summary>
                private static Collection<Location> board;

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
                private int salary;

                /// <summary>
                /// The salary paid to the player passing over this location.
                /// </summary>
                private int salaryOver;

                /// <summary>
                /// The price to buy the property.
                /// </summary>
                private int priceSale;

                /// <summary>
                /// The price to rent (land on) the property.
                /// </summary>
                private int priceRent;

                /// <summary>
                /// The multiplier to the rental price if multiple properties of
                /// the same type are owned by the player.
                /// </summary>
                private int multiplier = 1;

                /// <summary>
                /// Whether the property also represents a jail.
                /// </summary>
                private bool jail;

                /// <summary>
                /// The type of the property.
                /// </summary>
                private Zoning propertyType = Zoning.Unknown;

                /// <summary>
                /// The owner of the property.
                /// </summary>
                private Player owner;

                /// <summary>
                /// The kinds of properties that can be owned by a player.
                /// </summary>
                private List<Zoning> ownables = new List<Zoning>
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
                /// <param name="propertyName">The name of the property.</param>
                /// <param name="leftCoord">Left coordinate.</param>
                /// <param name="rightCoord">Right coordinate.</param>
                /// <param name="topCoord">Top coordinate.</param>
                /// <param name="bottomCoord">Bottom coordinate.</param>
                /// <param name="sale">Sale price.</param>
                /// <param name="rent">Rental price.</param>
                public Location(
                        Zoning zone,
                        string propertyName,
                        int leftCoord,
                        int rightCoord,
                        int topCoord,
                        int bottomCoord,
                        int sale,
                        int rent)
                {
                        this.propertyType = zone;
                        this.name = propertyName;
                        this.SetLeft(leftCoord);
                        this.SetRight(rightCoord);
                        this.SetTop(topCoord);
                        this.SetBottom(bottomCoord);
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

                        XmlHelper.FromXmlIfExists<string>(prop, "Name", ref this.name);
                        XmlHelper.FromXmlIfExists<int>(prop, "PriceSale", ref this.priceSale);
                        XmlHelper.FromXmlIfExists<int>(prop, "PriceRent", ref this.priceRent);
                        XmlHelper.FromXmlIfExists<int>(prop, "Tax", ref this.priceRent);
                        XmlHelper.FromXmlIfExists<int>(prop, "Multiplier", ref this.multiplier);
                        XmlHelper.FromXmlIfExists<int>(prop, "xLeft", ref this.left);
                        XmlHelper.FromXmlIfExists<int>(prop, "xRight", ref this.right);
                        XmlHelper.FromXmlIfExists<int>(prop, "yTop", ref this.top);
                        XmlHelper.FromXmlIfExists<int>(prop, "yBottom", ref this.bottom);
                        XmlHelper.FromXmlIfExists<int>(prop, "Salary", ref this.salary);
                        XmlHelper.FromXmlIfExists<int>(prop, "SalaryOver", ref this.salaryOver);
                        XmlHelper.FromXmlIfExists<bool>(prop, "SalaryOver", ref this.jail);

                        XmlHelper.FromXmlIfExists<string>(prop, "PropertyType", ref type);
                        try
                        {
                                this.PropertyType = (Zoning)Enum.Parse(Zoning.Park.GetType(), type);
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }
                        catch (OverflowException)
                        {
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
                /// Sets the board.
                /// </summary>
                /// <param name="theBoard">The board.</param>
                public static void SetBoard(Collection<Location> theBoard)
                {
                        Location.board = theBoard;
                }

                /// <summary>
                /// Sets the left coordinate.
                /// </summary>
                /// <param name="leftCoord">Left coordinate.</param>
                public void SetLeft(int leftCoord)
                {
                        this.left = leftCoord;
                }

                /// <summary>
                /// Sets the right coordinate.
                /// </summary>
                /// <param name="rightCoord">Right coordinate.</param>
                public void SetRight(int rightCoord)
                {
                        this.right = rightCoord;
                }

                /// <summary>
                /// Sets the top coordinate.
                /// </summary>
                /// <param name="topCoord">Top coordinate.</param>
                public void SetTop(int topCoord)
                {
                        this.top = topCoord;
                }

                /// <summary>
                /// Sets the bottom coordinate.
                /// </summary>
                /// <param name="bottomCoord">Bottom coordinate.</param>
                public void SetBottom(int bottomCoord)
                {
                        this.bottom = bottomCoord;
                }

                /// <summary>
                /// Handle situations where a player passes a location but does not land there.
                /// </summary>
                /// <returns>A printable string documenting what happened.</returns>
                /// <param name="p">THe player.</param>
                public string PassBy(Player p)
                {
                        string result = string.Empty;

                        if (p == null)
                        {
                                return result;
                        }

                        if (this.PropertyType == Zoning.MotherEarth && this.SalaryOver > 0)
                        {
                                p.Deposit(this.SalaryOver);
                                result = "Earned $" + this.SalaryOver.ToString() + " salary!" +
                                        Environment.NewLine;
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
                public string PrintOnLanding(Player p, ref Func<Player, string, string> answerer)
                {
                        bool canBuy = this.CanBuy;
                        bool ownable = this.Ownable;

                        if (p == null)
                        {
                                return string.Empty;
                        }

                        switch (this.PropertyType)
                        {
                        case Zoning.Residential:
                        case Zoning.Railroad:
                        case Zoning.Franchise:
                                if (canBuy && p.Balance > this.PriceSale)
                                {
                                        answerer = this.BuyLocation;
                                        return "Want to buy " + this.Name + " for " +
                                                this.PriceSale.ToString() + "?";
                                }
                                else if (canBuy)
                                {
                                        return "Can't buy " + Name + ".  Not enough money.";
                                }
                                else if (ownable && Owner != p)
                                {
                                        int rent = PriceRent;
                                        if (this.PropertyType == Zoning.Franchise ||
                                            this.PropertyType == Zoning.Railroad)
                                        {
                                                var rrs = board.Where(x =>
                                                                      x.PropertyType == PropertyType &&
                                                                      x.Owner == Owner);
                                                int nrrs = rrs.Count();
                                                for (int i = 0; i < nrrs; i++)
                                                {
                                                        rent *= Multiplier;
                                                }
                                        }

                                        answerer = RentLocation;
                                        return "Owned by " + Owner.Name + ", rent is $" +
                                                PriceRent.ToString() + ". [P]ay?";
                                } else if (ownable)
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
                                return "Labor upon Mother Earth produces wages:  $" +
                                        this.Salary.ToString() + ".";
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
                        if (answer.ToLower(System.Globalization.CultureInfo.CurrentCulture).StartsWith("y", StringComparison.CurrentCulture))
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
                        if (answer.ToLower(System.Globalization.CultureInfo.CurrentCulture).StartsWith("p", StringComparison.CurrentCulture))
                        {
                                p.Withdraw(this.PriceRent);
                                this.Owner.Deposit(this.PriceRent);
                                return "Owned by " + this.Owner.Name + ", rent is $" + this.PriceRent.ToString();
                        }

                        return string.Empty;
                }
        }
}