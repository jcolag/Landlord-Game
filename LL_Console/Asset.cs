using System;

namespace LL_Console
{
	public class Asset
	{
		public static int BaseValue = 100;

		private string _name = string.Empty;
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		private int _value = 0;
		public int Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}
		public Asset (string name, int value)
		{
			Name = name;
			Value = value;
		}
	}
}

