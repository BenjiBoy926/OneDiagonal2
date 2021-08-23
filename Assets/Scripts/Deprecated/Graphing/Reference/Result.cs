using UnityEngine;

[System.Serializable]
public class Result<Type> : Reference<Type>
{
    public Type value
    {
        set
        {
            switch (referenceType)
            {
                case ReferenceType.Direct:
                case ReferenceType.Indirect:
                    if (indirectValue != null)
                    {
                        indirectValue.value = value;
                    }
                    break;
                case ReferenceType.Redirected:
                    try
                    {
                        GameObject redirectGameObject = GetRedirectGameObject();

                        if (redirectGameObject != null)
                        {
                            Variable<Type> variable = redirectGameObject.GetComponent<Variable<Type>>();

                            if (variable != null)
                            {
                                variable.value = value;
                            }
                        }
                    }
                    catch (System.Exception) { }
                    break;
                default:
                    if (indirectValue != null)
                    {
                        indirectValue.value = value;
                    }
                    break;
            }
        }
    }
}
