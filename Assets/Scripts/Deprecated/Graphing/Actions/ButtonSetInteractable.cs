using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonSetInteractable : MonoBehaviour
{
    public Input<Button> button;
    public Input<bool> interactable;

    public UnityEvent output;

    public void Invoke()
    {
        button.value.interactable = interactable.value;
        output.Invoke();
    }
}
