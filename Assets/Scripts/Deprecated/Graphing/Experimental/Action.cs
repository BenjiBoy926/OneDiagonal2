using UnityEngine;
using UnityEngine.Events;

public class Action<InputPackageType, ResultPacakgeType> : MonoBehaviour
    where InputPackageType : InputPackage 
    where ResultPacakgeType : ResultPacakge
{
    public InputPackageType inputs;
    public ResultPacakgeType results;
}
