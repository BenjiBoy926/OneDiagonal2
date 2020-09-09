using UnityEngine;

public class Test<T>
{
    public T Get()
    {
        T item = default;
        System.Type genericType = item.GetType();

        GameObject gameObject = new GameObject("Test GameObject");

        if (genericType.IsSubclassOf(typeof(GameObject)))
        {
            return (T)(object)gameObject;
        }
        else return default;
    }
}

public class Test2<T> where T : Object
{
    public T Get()
    {
        GameObject gameObject = new GameObject("Test GameObject");
        return (T)(Object)gameObject;
    }
}
