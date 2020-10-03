﻿using UnityEngine;

public class GetFractionInFractionMatrix : Function<Fraction>
{
    public Input<FractionMatrix> matrix;
    public Input<int> row;
    public Input<int> col;

    public override Fraction Get()
    {
        try
        {
            return matrix.value.Get(row.value, col.value);
        }
        catch(System.Exception)
        {
            Debug.LogError("[" + row.value + ", " + col.value + "]");
            return Fraction.zero;
        }
    }
}
