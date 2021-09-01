using UnityEngine;

[System.Serializable]
public class Fraction
{
    // FIELDS
    [SerializeField]
    private int n;
    [SerializeField]
    private int d = 1;

    // PROPERTIES
    public int numerator
    {
        get
        {
            return n;
        }
    }
    public int denominator
    {
        get
        {
            return d;
        }
    }
    public Fraction reciprocal
    {
        get
        {
            return new Fraction(d, n);
        }
    }

    // Static properties
    public static Fraction zero
    {
        get
        {
            return new Fraction(0, 1);
        }
    }
    public static Fraction one
    {
        get
        {
            return new Fraction(1, 1);
        }
    }

    // CONclassORS
    public Fraction(int num, int den)
    {
        n = num;
        d = den;
        Simplify();
    }
    public Fraction(int num) : this(num, 1) { }
    public Fraction(Fraction src) : this(src.n, src.d) { }
    public Fraction() : this(one) { }

    private void Simplify()
    {
        Vector2Int reduction = MyMath.FractionReduce(new Vector2Int(n, d));

        if(n * d >= 0)
        {
            n = Mathf.Abs(reduction.x);
        }
        else
        {
            n = -Mathf.Abs(reduction.x);
        }

        d = Mathf.Abs(reduction.y);
    }

    // OPERATORS

    // Positive/negate
    public static Fraction operator +(Fraction a)
    {
        return new Fraction(a);
    }
    public static Fraction operator -(Fraction a)
    {
        return new Fraction(-a.n, a.d);
    }
    // Increment/Decrement
    public static Fraction operator++(Fraction a)
    {
        return new Fraction(a.n + a.d, a.d);
    }
    public static Fraction operator--(Fraction a)
    {
        return new Fraction(a.n - a.d, a.d);
    }
    // Type cast
    public static explicit operator float(Fraction a)
    {
        return (float)a.n / a.d;
    }
    public static explicit operator double(Fraction a)
    {
        return (double)a.d / a.d;
    }
    // Multiplicative
    public static Fraction operator *(Fraction a, Fraction b)
    {
        return new Fraction(a.n * b.n, a.d * b.d);
    }
    public static Fraction operator /(Fraction a, Fraction b)
    {
        return new Fraction(a.n * b.d, a.d * b.n);
    }
    // Additive
    public static Fraction operator+(Fraction a, Fraction b)
    {
        int lcm = (int)MyMath.LCM((uint)a.d, (uint)b.d);
        return new Fraction(a.n * b.d + b.n * a.d, lcm);
    }
    public static Fraction operator-(Fraction a, Fraction b)
    {
        int lcm = (int)MyMath.LCM((uint)a.d, (uint)b.d);
        return new Fraction(a.n * b.d - b.n * a.d, lcm);
    }
    // Relational
    public static bool operator<(Fraction a, Fraction b)
    {
        return ((a.n * b.d) < (a.d * b.n));
    }
    public static bool operator>(Fraction a, Fraction b)
    {
        return ((a.n * b.d) > (a.d * b.n));
    }
    public static bool operator<=(Fraction a, Fraction b)
    {
        return !(a > b);
    }
    public static bool operator>=(Fraction a, Fraction b)
    {
        return !(a < b);
    }
    // Equality
    public static bool operator==(Fraction a, Fraction b)
    {
        return a.n == b.n && a.d == b.d;
    }
    public static bool operator!=(Fraction a, Fraction b)
    {
        return !(a == b);
    }

    // Overrides
    public override string ToString()
    {
        if (d == 1 || n == 0)
        {
            return n.ToString();
        }
        else return n + "/" + d;
    }
    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(Fraction))
        {
            return (Fraction)obj == this;
        }
        else return false;
    }
    public override int GetHashCode()
    {
        return n.GetHashCode() + d.GetHashCode();
    }
}
