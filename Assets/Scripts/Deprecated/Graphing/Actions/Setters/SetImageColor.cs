using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SetImageColor : MonoBehaviour
{
    public Input<Image> input;
    public Input<Color> color;

    public UnityEvent output;

    public void Invoke()
    {
        input.value.color = color.value;
        output.Invoke();
    }
}
