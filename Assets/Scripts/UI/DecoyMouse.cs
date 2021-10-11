using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DecoyMouse : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Trail renderer that is active while the mouse is dragging")]
    private TrailRenderer trail;
    [SerializeField]
    [Tooltip("Image used to create an expanding/contracting effect")]
    private Image clickIndicator;
    #endregion

    #region Monobehaviour Messages
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // NOTE: have a contracting circle effect

            // Enable the trail while dragging
            trail.enabled = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            // NOTE: have an expanding circle effect

            // Disable the trail since we are no longer dragging
            trail.enabled = false;
        }

        // Move the object to the mouse position always
        transform.position = Input.mousePosition;
    }
    #endregion
}
