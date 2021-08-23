using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // TYPEDEFS
    public enum DragType
    {
        VerticalAndHorizontal,
        VerticalOnly,
        HorizontalOnly
    }

    [SerializeField]
    private DragType type;

    [SerializeField]
    private ActionEvents events;

    private Vector2 pointerInRect;
    private Vector2 pointerInParent;

    public virtual void OnBeginDrag(PointerEventData data)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            data.position,
            data.pressEventCamera,
            out pointerInRect);

        events.start.Invoke();
    }

    public virtual void OnDrag(PointerEventData data)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            data.position,
            data.pressEventCamera,
            out pointerInParent);

        DoDrag();

        events.step.Invoke();
    }

    public virtual void OnEndDrag(PointerEventData data)
    {
        events.stop.Invoke();
    }

    private void DoDrag()
    {
        float newX;
        float newY;

        switch (type)
        {
            case DragType.VerticalAndHorizontal:
                newX = pointerInParent.x - pointerInRect.x;
                newY = pointerInParent.y - pointerInRect.y;
                break;
            case DragType.VerticalOnly:
                newX = transform.localPosition.x;
                newY = pointerInParent.y - pointerInRect.y;
                break;
            case DragType.HorizontalOnly:
                newX = pointerInParent.x - pointerInRect.x;
                newY = transform.localPosition.y;
                break;
            default: newX = newY = 0; break;
        }

        transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
    }
}
