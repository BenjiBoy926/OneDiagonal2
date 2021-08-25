using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellmade.Sound;

public class MatrixRowAddSignSwitch : MatrixUIChild
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the button that acts as the toggle")]
    private Button button;
    [SerializeField]
    [Tooltip("Sound that plays when the button is clicked")]
    private AudioClip switchSound;
    #endregion

    #region Private Methods
    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(ToggleAdding);

        // Add listeners for operation started and finished events
        MatrixParent.OnOperationStart.AddListener(OnMatrixOperationStarted);
        MatrixParent.OnOperationFinish.AddListener(OnMatrixOperationFinished);
    }
    private void ToggleAdding()
    {
        MatrixRowAddWidget[] widgets = FindObjectsOfType<MatrixRowAddWidget>();
        foreach(MatrixRowAddWidget widget in widgets)
        {
            widget.ToggleAdding();
        }
        EazySoundManager.PlayUISound(switchSound);
    }
    private void OnMatrixOperationStarted()
    {
        button.interactable = false;
    }
    private void OnMatrixOperationFinished()
    {
        button.interactable = true;
    }
    #endregion
}
