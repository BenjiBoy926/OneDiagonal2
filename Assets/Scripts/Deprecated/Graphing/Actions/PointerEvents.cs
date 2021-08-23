using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerEvents : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public UnityEvent click;
    public UnityEvent down;
    public UnityEvent enter;
    public UnityEvent exit;
    public UnityEvent up;

    public virtual void OnPointerClick(PointerEventData data)
    {
        click.Invoke();
    }
    public virtual void OnPointerDown(PointerEventData data)
    {
        down.Invoke();
    }
    public virtual void OnPointerEnter(PointerEventData data)
    {
        enter.Invoke();
    }
    public virtual void OnPointerExit(PointerEventData data)
    {
        exit.Invoke();
    }
    public virtual void OnPointerUp(PointerEventData data)
    {
        up.Invoke();
    }
}
