using UnityEngine;
using UnityEngine.Events;

public class Emitter2D : MonoBehaviour
{
    public Input<GameObject> prefab;
    public Input<GameObject> parent;
    public Input<Vector2> origin;
    public Input<Vector2> direction;
    public Input<float> speed;

    public Result<GameObject> clone;

    public UnityEvent output;

    public void Invoke()
    {
        Transform cloneParent = parent.value != null ? parent.value.transform : null;
        GameObject instance = Instantiate(prefab.value, origin.value, prefab.value.transform.rotation, cloneParent);
        Rigidbody2D cloneRigidbody = instance.GetComponent<Rigidbody2D>();

        if(cloneRigidbody != null)
        {
            cloneRigidbody.velocity = direction.value.normalized * speed.value;

            if (clone != null) clone.value = instance;
            output.Invoke();
        }
    }
}
