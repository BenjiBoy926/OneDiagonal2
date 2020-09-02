using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMonobehaviourThatContainsGenericMonoBehaviour<Type> : MonoBehaviour
{
    [SerializeField]
    private GenericMonobehaviour<Type> genericMonobehaviour;
}
