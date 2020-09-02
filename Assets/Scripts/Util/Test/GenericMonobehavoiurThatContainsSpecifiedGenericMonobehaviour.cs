using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMonobehavoiurThatContainsSpecifiedGenericMonobehaviour<Type> : MonoBehaviour
{
    [SerializeField]
    private SpecifiedGenericMonobehaviour specifiedGenericMonobehaviour;
}
