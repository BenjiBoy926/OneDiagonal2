using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetTextMeshProText : MonoBehaviour
{
    public Input<TMPro.TextMeshProUGUI> textMesh;
    public Input<string> text;

    public UnityEvent output;

    public void Invoke()
    {
        textMesh.value.text = text.value;
        output.Invoke();
    }
}
