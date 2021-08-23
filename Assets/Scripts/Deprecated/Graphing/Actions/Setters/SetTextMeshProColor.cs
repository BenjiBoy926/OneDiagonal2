using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SetTextMeshProColor : MonoBehaviour
{
    public Input<TextMeshProUGUI> text;
    public Input<Color> color;

    public UnityEvent output;

    public void Invoke()
    {
        text.value.color = color.value;
        output.Invoke();
    }
}
