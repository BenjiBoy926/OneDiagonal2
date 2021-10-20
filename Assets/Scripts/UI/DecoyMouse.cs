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
    [SerializeField]
    [Tooltip("Time it takes for the indicator to animate to its target")]
    private float clickIndicatorAnimationTime = 0.3f;
    [SerializeField]
    [Tooltip("Size of the click indicator when expanded")]
    private float clickIndicatorExpandedSize = 3f;
    [SerializeField]
    [Tooltip("Color of the click indicator")]
    private Color clickIndicatorColor = Color.gray;
    #endregion

    #region Monobehaviour Messages
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Animate the click indicator from expanded/see through to normal/visible
            AnimateClickIndicator(
                new Color(clickIndicatorColor.r, clickIndicatorColor.g, clickIndicatorColor.b, 0f), 
                clickIndicatorColor, 
                clickIndicatorExpandedSize, 
                1f);

            // Enable the trail while dragging
            trail.enabled = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            // Animate the click indicator from normal/visible to expanded/see through
            AnimateClickIndicator(
                clickIndicatorColor,
                new Color(clickIndicatorColor.r, clickIndicatorColor.g, clickIndicatorColor.b, 0f),
                1f,
                clickIndicatorExpandedSize);

            // Disable the trail since we are no longer dragging
            trail.enabled = false;
        }

        // Move the object to the mouse position always
        transform.position = Input.mousePosition;
    }
    #endregion

    #region Private Methods
    private void AnimateClickIndicator(Color startColor, Color endColor, float startScale, float endScale)
    {
        // Complete existing animations
        clickIndicator.DOComplete();
        clickIndicator.transform.DOComplete();

        // Set the starting values
        clickIndicator.color = startColor;
        clickIndicator.transform.localScale = Vector3.one * startScale;

        // Animate to the end scale
        clickIndicator.DOColor(endColor, clickIndicatorAnimationTime);
        clickIndicator.transform.DOScale(endScale, clickIndicatorAnimationTime);

    }
    #endregion
}
