using UnityEngine;

[System.Serializable]
public class Input<Type> : Reference<Type>
{
    public Type value
    {
        get
        {
            switch(referenceType)
            {
                case ReferenceType.Direct: return GetDirectValue();
                case ReferenceType.Indirect: return GetIndirectValue();
                case ReferenceType.Redirected: return GetRedirectedValue();
                default: return default;
            }
        }
    }

    private Type GetDirectValue()
    {
        if (evaluationType == EvaluationType.Value)
        {
            return directValue;
        }
        else return directAction.Get();
    }

    private Type GetIndirectValue()
    {
        if (evaluationType == EvaluationType.Value)
        {
            return indirectValue.value;
        }
        else return indirectAction.value.Get();
    }

    private Type GetRedirectedValue()
    {
        System.Type valueType = typeof(Type);
        GameObject redirectObject = GetRedirectGameObject();

        // If the redirect is indirect, or the type is not a GameObject or Component,
        // we'll get a variable with the generic type
        if (redirectType == RedirectType.Indirect ||
            evaluationType == EvaluationType.Function ||
            (!valueType.IsSameAsOrSubclassOf(typeof(GameObject)) &&
            !valueType.IsSameAsOrSubclassOf(typeof(Component))))
        {
            if (evaluationType == EvaluationType.Value)
            {
                return redirectObject.GetComponent<Variable<Type>>().value;
            }
            else return redirectObject.GetComponent<Variable<Supplier<Type>>>().value.Get();
        }
        // If the redirect type is direct, and this is a GameObject,
        // return the exact gameobject
        else if (valueType.IsSameAsOrSubclassOf(typeof(GameObject)))
        {
            return (Type)(object)redirectObject;
        }
        // If the redirect type is direct, and this is a component,
        // return the exact component on the object
        else
        {
            return redirectObject.GetComponent<Type>();
        }
    }
}
