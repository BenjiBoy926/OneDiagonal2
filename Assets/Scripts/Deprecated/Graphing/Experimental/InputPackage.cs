[System.Serializable]
public class InputPackage { }

[System.Serializable]
public class InputPackage<T> : InputPackage
{
    public Input<T> input;
}

[System.Serializable]
public class InputPackage<T1, T2> : InputPackage
{
    public Input<T1> input1;
    public Input<T2> input2;
}

[System.Serializable]
public class InputPackage<T1, T2, T3> : InputPackage
{
    public Input<T1> input1;
    public Input<T2> input2;
    public Input<T3> input3;
}

[System.Serializable]
public class InputPackage<T1, T2, T3, T4> : InputPackage
{
    public Input<T1> input1;
    public Input<T2> input2;
    public Input<T3> input3;
    public Input<T4> input4;
}

[System.Serializable]
public class InputPackage<T1, T2, T3, T4, T5> : InputPackage
{
    public Input<T1> input1;
    public Input<T2> input2;
    public Input<T3> input3;
    public Input<T4> input4;
    public Input<T5> input5;
}

