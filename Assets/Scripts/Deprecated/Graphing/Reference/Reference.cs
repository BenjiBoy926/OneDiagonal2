using UnityEngine;

[System.Serializable]
public abstract class Reference<Type>
{
    // FIELDS
    [SerializeField]
    [Tooltip("If the reference evaluation is by value, the reference tries to get the direct value." +
        " If the evaluation is by function, the reference tries to get an action that generates the value")]
    protected EvaluationType evaluationType;

    [SerializeField]
    [Tooltip("Type of reference")]
    protected ReferenceType referenceType = ReferenceType.Direct;

    [SerializeField]
    [Tooltip("In the case of a redirected reference, determine" +
        " if the GameObject will be obtained through a variable" +
        " or by invoking an action that returns a Game Object")]
    protected EvaluationType gameObjectReferenceType = EvaluationType.Value;

    [SerializeField]
    [Tooltip("Type of a redirect reference, in the case of a Unity GameObject or Component reference." +
        " A direct redirect searches for the component on the GameObject or gets the GameObject directly," +
        " an indirect redirect searches for a variable with the component type on the GameObject")]
    protected RedirectType redirectType = RedirectType.Direct;

    [SerializeField]
    [Tooltip("Depth of the redirection." +
        " Zero means directly search the GameObject in the variable or with the tag," +
        " one means search the GameObject for a GameObject variable and search on THAT GameObject for the value," +
        " and so on")]
    protected int redirectDepth = 0;

    [SerializeField]
    [Tooltip("Value in the reference")]
    protected Type directValue = default;

    [SerializeField]
    [Tooltip("The action that will generate the value when invoked")]
    protected Supplier<Type> directAction;

    [SerializeField]
    [Tooltip("Variable used to get the value")]
    protected Variable<Type> indirectValue = null;

    [SerializeField]
    [Tooltip("Variable used to get the action that generates the value when invoked")]
    protected Variable<Supplier<Type>> indirectAction;

    [SerializeField]
    [Tooltip("Variable with the Game Object to get the attached variable")]
    protected GameObjectVariable redirectVariable = null;

    [SerializeField]
    [Tooltip("Action used to get the Game Object with the attached variable")]
    protected Supplier<GameObject> redirectAction;

    [SerializeField]
    private bool mainFoldout;
    [SerializeField]
    private bool advancedFoldout;

    protected GameObject GetRedirectGameObject()
    {
        GameObject redirectObject;

        // Get the GameObject to use for the redirect
        switch (gameObjectReferenceType)
        {
            case EvaluationType.Value:
                redirectObject = redirectVariable.value;
                break;
            case EvaluationType.Function:
                redirectObject = redirectAction.Get();
                break;
            default:
                redirectObject = redirectVariable.value;
                break;
        }

        int i = 0;
        Variable<GameObject> currentRedirectVariable = redirectObject.GetComponent<Variable<GameObject>>();

        // Repeately set the redirect object based on the GameObject variable
        // on each GameObject in the search chain
        while(i < redirectDepth && currentRedirectVariable != null)
        {
            // Update the redirect object
            redirectObject = currentRedirectVariable.value;

            // Update loop control variables
            currentRedirectVariable = redirectObject.GetComponent<Variable<GameObject>>();
            i++;
        }

        return redirectObject;
    }
}
