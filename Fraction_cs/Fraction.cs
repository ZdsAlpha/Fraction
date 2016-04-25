using System;
using System.Numerics;
using System.Text;
public struct Fraction : IComparable, IComparable<Fraction>, IEquatable<Fraction>
{
    private BigInteger _Numerator;
    private BigInteger _Denominator;
    public BigInteger Numerator
    {
        get { return _Numerator; }
        set
        {
            _Numerator = value;
            Simplify();
        }
    }
    public BigInteger Denominator
    {
        get { return _Denominator; }
        set
        {
            _Denominator = value;
            Simplify();
        }
    }
    public BigInteger WholeNumber
    {
        get
        {
            if (Denominator == 0)
                return 0;
            return BigInteger.Divide(Numerator, Denominator);
        }
        set
        {
            Fraction pf = ProperFraction;
            Fraction Fraction = pf + value;
            _Numerator = Fraction.Numerator;
            _Denominator = Fraction.Denominator;
            Simplify();
        }
    }
    public Fraction ProperFraction
    {
        get
        {
            if (Denominator == 0)
                return new Fraction(0, 0);
            return new Fraction(Numerator % Denominator, Denominator);
        }
        set
        {
            BigInteger  wn = WholeNumber;
            Fraction Fraction = wn + value;
            _Numerator = Fraction.Numerator;
            _Denominator = Fraction.Denominator;
            Simplify();
        }
    }
    public bool IsProper
    {
        get { return BigInteger.Abs(Numerator) < Denominator; }
    }
    public bool IsImproper
    {
        get { return !IsProper; }
    }
    public bool IsNull
    {
        get { return Numerator == 0; }
    }
    public bool IsUnit
    {
        get
        {
            return Numerator == 1 && Denominator == 1;
        }
    }
    public bool IsNegitive
    {
        get { return Numerator < 0; }
    }
    public bool IsUndefined
    {
        get { return Denominator == 0; }
    }
    public Fraction Absolute
    {
        get { return new Fraction(BigInteger.Abs(Numerator), BigInteger.Abs(Denominator)); }
    }

    public new string ToString()
    {
        return _Numerator.ToString() + "/" + _Denominator.ToString();
    }
    public string ToString(ulong DigitsAfterPoint)
    {
        StringBuilder sb = new StringBuilder();
        BigInteger Whole = BigInteger.Abs(WholeNumber);
        Fraction Proper = ProperFraction.Absolute;
        sb.AppendFormat("{0}.", Whole.ToString());
        BigInteger Numerator = Proper.Numerator;
        BigInteger Denominator = Proper.Denominator;
        for (ulong i = 0; i <= DigitsAfterPoint - 1; i++)
        {
            Numerator = Numerator * 10;
            sb.Append(BigInteger.Divide(Numerator, Denominator).ToString());
            Numerator = Numerator % Denominator;
        }
        if (IsNegitive)
            sb.Insert(0, "-");
        return sb.ToString();
    }

    public void SetValue(BigInteger Numerator, BigInteger Denominator)
    {
        _Numerator = Numerator;
        _Denominator = Denominator;
        Simplify();
    }
    public void Simplify()
    {
        if (Denominator == 0)
        {
            _Numerator = 0;
            _Denominator = 0;
        }
        else if (Denominator < 0)
        {
            _Denominator = BigInteger.Abs(Denominator);
            _Numerator = -Numerator;
        }
        else if (Numerator == 0 & Denominator != 0)
        {
            _Denominator = 1;
        }
        else if (Numerator == Denominator & Denominator != 0)
        {
            _Numerator = 1;
            _Denominator = 1;
        }
        else {
            BigInteger GDC = Fraction.GCD(Numerator, Denominator);
            _Numerator = Numerator / GDC;
            _Denominator = Denominator / GDC;
        }
    }
    public void Reduce(long Change = 1)
    {
        Reduce(Change, Change);
    }
    public void Reduce(long ChangeInNumerator, long ChangeInDenominator)
    {
        bool IsNegitive = false;
        Fraction Proper = ProperFraction;
        if (Proper < 0)
        {
            IsNegitive = true;
            Proper = Proper.Absolute;
        }
        Fraction Reduced = Proper;
        for (long CIN = -ChangeInNumerator; CIN <= ChangeInNumerator; CIN++)
        {
            for (long CID = -ChangeInDenominator; CID <= ChangeInDenominator; CID++)
            {
                Fraction Fraction = (new Fraction(Proper.Numerator + CIN, Proper.Denominator + CID)).Absolute;
                if (Fraction.Numerator < Reduced.Numerator && Fraction.Denominator < Reduced.Denominator && !Fraction.IsUndefined)
                    Reduced = Fraction;
            }
        }
        if (IsNegitive == true)
        {
            ProperFraction = -Reduced;
        }
        else {
            ProperFraction = Reduced;
        }
    }
    public static Fraction Power(Fraction Fraction, int Power)
    {
        if (Power > 0)
        {
            return new Fraction(BigInteger.Pow(Fraction.Numerator, Power), BigInteger.Pow(Fraction.Denominator, Power));
        }
        else if (Power < 0)
        {
            return new Fraction(BigInteger.Pow(Fraction.Denominator, Power), BigInteger.Pow(Fraction.Numerator, Power));
        }
        return new Fraction(1, 1);
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;
        if (obj is Fraction)
        {
            return CompareTo((Fraction)obj);
        }
        else {
            throw new ArgumentException(string.Format("{0} is not a {1}", nameof(obj), GetType().FullName));
        }
    }
    public int CompareTo(Fraction other)
    {
        if (this == other)
            return 0;
        if (this > other)
        {
            return 1;
        }
        else {
            return -1;
        }
    }
    public bool Equals(Fraction other)
    {
        return this == other;
    }

    public Fraction(BigInteger Numerator, BigInteger Denominator)
    {
        _Numerator = Numerator;
        _Denominator = Denominator;
        Simplify();
    }
    public Fraction(BigInteger Number)
    {
        _Numerator = Number;
        _Denominator = 1;
    }
    public Fraction(BigInteger WholeNumber, Fraction ProperFraction)
    {
        _Numerator = WholeNumber * ProperFraction.Denominator + ProperFraction.Numerator;
        _Denominator = ProperFraction.Denominator;
    }
    public Fraction(string Value)
    {
        Fraction Fraction = Parse(Value);
        _Numerator = Fraction.Numerator;
        _Denominator = Fraction.Denominator;
        Simplify();
    }
    public Fraction(decimal Number) : this(Number.ToString())
    {
    }
    public Fraction(double Number) : this(Number.ToString())
    {
    }
    public Fraction(float Number) : this(Number.ToString())
    {
    }

    public static Fraction operator +(Fraction Fraction1, Fraction Fraction2)
    {
        return new Fraction(BigInteger.Add(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Denominator), BigInteger.Multiply(Fraction2.Numerator, Fraction1.Denominator)), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Denominator));
    }
    public static Fraction operator -(Fraction Fraction1, Fraction Fraction2)
    {
        return new Fraction(BigInteger.Subtract(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Denominator), BigInteger.Multiply(Fraction2.Numerator, Fraction1.Denominator)), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Denominator));
    }
    public static Fraction operator *(Fraction Fraction1, Fraction Fraction2)
    {
        return new Fraction(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Numerator), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Denominator));
    }
    public static Fraction operator /(Fraction Fraction1, Fraction Fraction2)
    {
        return new Fraction(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Denominator), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Numerator));
    }
    public static Fraction operator -(Fraction Fraction)
    {
        return 0 - Fraction;
    }

    public static bool operator ==(Fraction Fraction1, Fraction Fraction2)
    {
        if (Fraction1.IsNull & Fraction2.IsNull)
            return true;
        if ((Fraction1 / Fraction2).IsUnit)
            return true;
        return false;
    }
    public static bool operator !=(Fraction Fraction1, Fraction Fraction2)
    {
        if (Fraction1 == Fraction2)
            return false;
        return true;
    }
    public static bool operator >(Fraction Fraction1, Fraction Fraction2)
    {
        if (Fraction2.IsNull)
        {
            if (Fraction1.IsNegitive)
            {
                return false;
            }
            else {
                return true;
            }
        }
        Fraction Division = Fraction1 / Fraction2;
        if (Division.Numerator > Division.Denominator)
            return true;
        return false;
    }
    public static bool operator <(Fraction Fraction1, Fraction Fraction2)
    {
        if (Fraction2.IsNull)
        {
            if (Fraction1.IsNegitive)
            {
                return true;
            }
            else {
                return false;
            }
        }
        Fraction Division = Fraction1 / Fraction2;
        if (Division.Numerator < Division.Denominator)
            return true;
        return false;
    }
    public static bool operator >=(Fraction Fraction1, Fraction Fraction2)
    {
        if (Fraction1 > Fraction2 | Fraction1 == Fraction2)
            return true;
        return false;
    }
    public static bool operator <=(Fraction Fraction1, Fraction Fraction2)
    {
        if (Fraction1 < Fraction2 | Fraction1 == Fraction2)
            return true;
        return false;
    }

    public static implicit operator Fraction(byte Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(sbyte Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(short Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(ushort Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(int Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(uint Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(long Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(ulong Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(BigInteger Number)
    {
        return new Fraction(Number, 1);
    }
    public static implicit operator Fraction(float Number)
    {
        return Fraction.Parse(Number.ToString());
    }
    public static implicit operator Fraction(double Number)
    {
        return Fraction.Parse(Number.ToString());
    }
    public static implicit operator Fraction(decimal Number)
    {
        return Fraction.Parse(Number.ToString());
    }

    public static explicit operator byte(Fraction Fraction)
    {
        return Convert.ToByte(Fraction.WholeNumber);
    }
    public static explicit operator sbyte(Fraction Fraction)
    {
        return Convert.ToSByte(Fraction.WholeNumber);
    }
    public static explicit operator short(Fraction Fraction)
    {
        return Convert.ToInt16(Fraction.WholeNumber);
    }
    public static explicit operator ushort(Fraction Fraction)
    {
        return Convert.ToUInt16(Fraction.WholeNumber);
    }
    public static explicit operator int(Fraction Fraction)
    {
        return Convert.ToInt32(Fraction.WholeNumber);
    }
    public static explicit operator uint(Fraction Fraction)
    {
        return Convert.ToUInt32(Fraction.WholeNumber);
    }
    public static explicit operator long(Fraction Fraction)
    {
        return Convert.ToInt64(Fraction.WholeNumber);
    }
    public static explicit operator ulong(Fraction Fraction)
    {
        return Convert.ToUInt64(Fraction.WholeNumber);
    }
    public static explicit operator float(Fraction Fraction)
    {
        return float.Parse(Fraction.ToString(10));
    }
    public static explicit operator double(Fraction Fraction)
    {
        return float.Parse(Fraction.ToString(20));
    }

    public static Fraction Unit
    {
        get { return new Fraction(1, 1); }
    }
    public static Fraction Null
    {
        get { return new Fraction(0, 1); }
    }
    public static Fraction Undefined
    {
        get { return new Fraction(0, 0); }
    }
    public static BigInteger GCD(BigInteger First, BigInteger Second)
    {
        First = BigInteger.Abs(First);
        Second = BigInteger.Abs(Second);
        BigInteger Reminder = default(BigInteger);
        while (Second > 0)
        {
            Reminder = First % Second;
            First = Second;
            Second = Reminder;
        }
        return First;
    }
    public static bool TryParse(string Value, ref Fraction Result)
    {
        if (Value == null || string.IsNullOrEmpty(Value))
            return false;
        Value = Value.Trim(' ');
        bool IsConverted = false;
        if (Value.Contains("/"))
        { 
            string[] tmp = Value.Split('/');
			if (tmp.Length != 2)
				return false;
			BigInteger Numerator = 0;
            BigInteger Denominator = 0;
            IsConverted = BigInteger.TryParse(tmp[0],out Numerator);
			if (IsConverted == false)
				return false;
			IsConverted = BigInteger.TryParse(tmp[1],out Denominator);
			if (IsConverted == false)
				return false;
			Result = new Fraction(Numerator, Denominator);
        } else if (Value.Contains(".")) {
			bool IsNegitive = false;
			if (Value[0]== "-"[0])
				IsNegitive = true;
                string[] tmp = Value.Split('.');
			if (tmp.Length != 2)
				return false;
			if (tmp[1].Contains("-"))
				return false;
			BigInteger WholeNumber = 0;
			if (!string.IsNullOrEmpty(tmp[0])) {
				IsConverted = BigInteger.TryParse(tmp[0],out WholeNumber);
				if (IsConverted == false)
					return false;
				WholeNumber = BigInteger.Abs(WholeNumber);
			}
			BigInteger ProperNumerator = 0;
            IsConverted = BigInteger.TryParse(tmp[1],out ProperNumerator);
			if (IsConverted == false)
				return false;
			char[] ProperDenominator = new char[tmp[1].Length + 1];
            ProperDenominator[0] = '1';
			for (int i = 1; i <= tmp[1].Length; i++) {
                ProperDenominator[i] = '0';
			}
			Fraction Proper = new Fraction(ProperNumerator, BigInteger.Parse(new string(ProperDenominator)));
			if (IsNegitive) {
				Result = -new Fraction(WholeNumber, Proper);
			} else {
				Result = new Fraction(WholeNumber, Proper);
			}
		} else {
			BigInteger Number = 0;
            IsConverted = BigInteger.TryParse(Value,out Number);
			if (IsConverted == false)
				return false;
			Result = new Fraction(Number);
		}
		return true;
	}
	public static Fraction Parse(string Value)
{
    Fraction Fraction = 0;
    bool IsConverted = false;
    IsConverted = TryParse(Value, ref Fraction);
    if (IsConverted == false)
    {
        throw new FormatException("Invalid format. Unable to convert " + Value.ToString() + " to fraction.");
    }
    else {
        return Fraction;
    }
}
}