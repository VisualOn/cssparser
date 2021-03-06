﻿using System;
using System.Collections.Generic;

namespace CSSParser.StringNavigators
{
	public class StringNavigator : IWalkThroughStrings
	{
		private readonly char[] _value;
		private readonly int _index;
		private StringNavigator(char[] value, int index)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if ((index < 0) || (index >= value.Length))
				throw new ArgumentOutOfRangeException("index");

			_value = value;
			_index = index;
			CurrentCharacter = _value[_index];
		}
		public StringNavigator(string value) : this((value != null) ? value.ToCharArray() : null, 0) { }

		/// <summary>
		/// This return null if the current location in the string has no content (eg. anywhere on an empty string or past the end of a non-empty string)
		/// </summary>
		public char? CurrentCharacter { get; private set; }

		/// <summary>
		/// This return null if the current location in the string has no content (eg. anywhere on an empty string or past the end of a non-empty string)
		/// </summary>
		public IWalkThroughStrings Next
		{
			get
			{
				if (_index == _value.Length - 1)
					return new GoneTooFarStringNavigator();
				return new StringNavigator(_value, _index + 1);
			}
		}

		/// <summary>
		/// This will return true if the content is at least as long as the specified value string and if the next n characters (where n is the length of
		/// the value string) correspond to each of the value string's characters. This testing will be done according to the optionalComparer if non-null
		/// and will apply a simple char comparison (precise) match if a null optionalComparer is specified. An exception will be raised for a null or
		/// blank value. If there is insufficient content available to match the length of the value argument then false will be returned.
		/// </summary>
		public bool DoesCurrentContentMatch(string value, IEqualityComparer<char> optionalComparer)
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentException("Null/blank value specified");

			if ((_index + value.Length) > _value.Length)
				return false;

			for (var index = 0; index < value.Length; index++)
			{
				var contentCharacter = _value[_index + index];
				var compareToCharacter = value[index];
				if (optionalComparer == null)
				{
					if (contentCharacter != compareToCharacter)
						return false;
				}
				else if (!optionalComparer.Equals(contentCharacter, compareToCharacter))
					return false;
			}
			return true;
		}
	}
}
