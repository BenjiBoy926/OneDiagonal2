using UnityEngine;

[System.Serializable]
public class Fraction
{
    [SerializeField]
    private int n;
    [SerializeField]
    private int d = 1;

    public int numerator
    {
        get
        {
            return n;
        }
        set
        {
            n = value;
            Simplify();
        }
    }

    public int denominator
    {
        get
        {
            return d;
        }
        set
        {
            d = value;
            Simplify();
        }
    }

    public Fraction(int num, int den)
    {
        n = num;
        d = den;
        Simplify();
    }

    public void Simplify()
    {
        Vector2Int reduction = MyMath.Reduce(new Vector2Int(n, d));

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

    public static Fraction operator*(Fraction a, Fraction b)
    {
        return new Fraction(a.n * b.n, a.d * b.d);
    }

    public static Fraction operator/(Fraction a, Fraction b)
    {
        return new Fraction(a.n * b.d, a.d * b.n);
    }
}
