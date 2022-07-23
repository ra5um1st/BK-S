using System.Collections.Generic;
using System.Globalization;

namespace Calc.Core
{
    public class Number
    {
        public enum Digit
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine
        }

        public static readonly Dictionary<Digit, string> Digits = new Dictionary<Digit, string>() 
        {
            { Digit.Zero, "0" },
            { Digit.One, "1" },
            { Digit.Two, "2" },
            { Digit.Three, "3" },
            { Digit.Four, "4" },
            { Digit.Five, "5" },
            { Digit.Six, "6" },
            { Digit.Seven, "7" },
            { Digit.Eight, "8" },
            { Digit.Nine, "9" },
        };

        public Number()
        {
            Reset();
        }

        public string Sign { get; private set; }
        public string IntPart { get; private set; }
        public string FloatPart { get; private set; }

        public bool EndsWithZero
        {
            get
            {
                if(IntPart == "0" && !IsFloating)
                {
                    return true;
                }
                else if(FloatPart == "0")
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsFloating
        {
            get
            {
                if(FloatPart == null || FloatPart == string.Empty)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsSigned => Sign == "-";

        public Number AddDigit(Digit digit)
        {
            if (IsFloating)
            {
                FloatPart += Digits[digit];
            }
            else
            {
                if (EndsWithZero)
                {
                    IntPart = Digits[digit];
                }
                else
                {
                    IntPart += Digits[digit];
                }
            }

            return this;
        }

        public Number RemoveDigit()
        {
            if (IsFloating)
            {
                FloatPart = FloatPart.Substring(0, FloatPart.Length - 1);
            }
            else
            {
                IntPart = IntPart.Substring(0, IntPart.Length - 1);
            }

            return this;
        }

        public Number Reset()
        {
            IntPart = "0";
            FloatPart = string.Empty;
            Sign = string.Empty;

            return this;
        }

        public Number Reset(double number, NumberFormatInfo format)
        {
            string str = number.ToString(format);
            var parts = str.Split('.');

            if (str.StartsWith("-"))
            {
                Sign = "-";
                IntPart = parts[0].Substring(1, parts[0].Length - 1);
            }
            else
            {
                Sign = string.Empty;
                IntPart = parts[0];
            }

            if(parts.Length > 1)
            {
                FloatPart = parts[1];
            }

            return this;
        }

        public Number InvertSign()
        {
            if (IsSigned)
            {
                Sign = string.Empty;
            }
            else
            {
                Sign = "-";
            }

            return this;
        }

        public Number MakeFloating(Digit digit = Digit.Zero)
        {
            if (!IsFloating)
            {
                FloatPart = Digits[digit];
            }

            return this;
        }

        public override string ToString()
        {
            if (IsFloating)
            {
                return Sign + IntPart + "." + FloatPart;
            }
            else
            {
                return Sign + IntPart;
            }
        }

        public static implicit operator string(Number number)
        {
            return number.ToString();
        }
    }
}
