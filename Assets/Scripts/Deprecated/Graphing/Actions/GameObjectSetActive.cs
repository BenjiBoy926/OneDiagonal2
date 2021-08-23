using UnityEngine;
using UnityEngine.Events;

public class GameObjectSetActive : MonoBehaviour
{
    public Input<GameObject> gameObjectInput;
    public Input<bool> active;

    public UnityEvent output;

    public void Invoke()
    {
        gameObjectInput.value.SetActive(active.value);
        output.Invoke();
    }
}
