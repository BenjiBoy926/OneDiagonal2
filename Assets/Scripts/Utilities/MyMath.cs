using UnityEngine;

public static class MyMath
{
    public static uint GCD(uint a, uint b)
    {
        while(a != 0 && b != 0)
        {
            if (a > b) a %= b;
            else b %= a;
        }
        return a == 0 ? b : a;
    }

    public static uint LCM(uint a, uint b)
    {
        return (a / GCD(a, b)) * b;
    }

    public static Vector2Int FractionReduce(Vector2Int numbers)
    {
        uint gcd = GCD((uint)Mathf.Abs(numbers.x), (uint)Mathf.Abs(numbers.y));

        if (gcd > 1)
        {
            return new Vector2Int((int)(numbers.x / gcd), (int)(numbers.y / gcd));
        }
        else return numbers;
    }

    public static Vector2Int FractionSimplify(Vector2Int numbers)
    {
        Vector2Int reduction = FractionReduce(numbers);
        Vector2Int result = new Vector2Int();

        if (numbers.x * numbers.y >= 0)
        {
            result.x = Mathf.Abs(reduction.x);
        }
        else
        {
            result.x = -Mathf.Abs(reduction.x);
        }

        result.y = Mathf.Abs(reduction.y);

        return result;
    }

    public static int Index2Dto1D(int i, int j, int cols)
    {
        return (i * cols) + j;
    }
    public static Vector2Int Index1Dto2D(int index, int rows, int cols)
    {
        return new Vector2Int(index / cols, index % rows);
    }
}
