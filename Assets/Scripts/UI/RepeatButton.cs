using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RepeatButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Public Properties
    public UnityEvent RepeatAction => repeatAction;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Script used to create some fun effects for this button")]
    private ButtonEffects effects;
    [SerializeField]
    [Tooltip("How much time to wait before starting the repeat invokation")]
    private float initialWaitTime = 0.3f;
    [SerializeField]
    [Tooltip("Amount of time between each successive invokation")]
    private float repeatWaitTime = 0.1f;
    [SerializeField]
    [Tooltip("Event invoked when the button is clicked/repeated")]
    private UnityEvent repeatAction;
    #endregion

    #region Private Fields
    private Coroutine repeatRoutine;
    #endregion

    #region Public Methods
    public void OnPointerDown(PointerEventData data)
    {
        StopRepeatRoutine();
        repeatRoutine = StartCoroutine(RepeatRoutine());
    }
    public void OnPointerUp(PointerEventData data)
    {
        StopRepeatRoutine();
    }
    #endregion

    #region Private Methods
    private void StopRepeatRoutine()
    {
        if(repeatRoutine != null)
        {
            StopCoroutine(repeatRoutine);
            repeatRoutine = null;
        }
    }
    private IEnumerator RepeatRoutine()
    {
        // Invoke the repeat action immediately
        repeatAction.Invoke();

        // Wait for the initial wait
        yield return new WaitForSeconds(initialWaitTime);

        // Forever, invoke the repeat action every repeat wait seconds
        while(true)
        {
            effects.Flash();
            effects.PunchSize(effects.PointerDownSound);

            repeatAction.Invoke();
            yield return new WaitForSeconds(repeatWaitTime);
        }
    }
    #endregion
}
