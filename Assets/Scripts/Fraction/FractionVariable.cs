﻿using UnityEngine;
using UnityEngine.Events;

public class FractionVariable : Variable<Fraction>
{
    // VARIABLES
    [SerializeField]
    [Tooltip("The name of the variable this component represents")]
    private string variableName;

    [SerializeField]
    [Tooltip("The default value of the variable component")]
    private Fraction defaultValue;

    [SerializeField]
    [Tooltip("Event invoked when the value of the component changes")]
    private UnityEvent onValueChanged;

    // FUNCITONS

    // Override getters
    public override string GetName()
    {
        return variableName;
    }
    public override Fraction GetDefaultValue()
    {
        return defaultValue;
    }
    public override UnityEvent GetValueChangedEvent()
    {
        return onValueChanged;
    }

    private void Awake()
    {
        value = defaultValue;
    }
}
