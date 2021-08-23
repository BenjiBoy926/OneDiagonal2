[System.Serializable]
public class ResultPacakge { }

[System.Serializable]
public class ResultPacakge<T> : ResultPacakge
{
    public Result<T> result;
}

[System.Serializable]
public class ResultPacakge<T1, T2> : ResultPacakge
{
    public Result<T1> result1;
    public Result<T2> result2;
}

[System.Serializable]
public class ResultPacakge<T1, T2, T3> : ResultPacakge
{
    public Result<T1> result1;
    public Result<T2> result2;
    public Result<T3> result3;
}

[System.Serializable]
public class ResultPacakge<T1, T2, T3, T4> : ResultPacakge
{
    public Result<T1> result1;
    public Result<T2> result2;
    public Result<T3> result3;
    public Result<T4> result4;
}

[System.Serializable]
public class ResultPacakge<T1, T2, T3, T4, T5> : ResultPacakge
{
    public Result<T1> result1;
    public Result<T2> result2;
    public Result<T3> result3;
    public Result<T4> result4;
    public Result<T5> result5;
}

