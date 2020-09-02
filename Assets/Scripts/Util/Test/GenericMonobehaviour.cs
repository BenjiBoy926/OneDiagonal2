using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMonobehaviour<Type> : MonoBehaviour
{
    [SerializeField]
    private Type type;
}
