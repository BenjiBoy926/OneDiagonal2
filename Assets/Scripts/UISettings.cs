using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Audio;

[CreateAssetMenu]
public class UISettings : ScriptableObjectSingleton<UISettings>
{
    #region Public Properties
    public static MatrixItemColor DiagonalColors => Instance.diagonalColors;
    public static MatrixItemColor NotDiagonalColors => Instance.notDiagnonalColors;
    public static float OperatorPunch => Instance.operatorPunch;
    public static float OperatorPunchTime => Instance.operatorPunchTime;
    #endregion

    #region Private Editor Fields

    [SerializeField]
    [Tooltip("Color of matrix items on the diagonal")]
    private MatrixItemColor diagonalColors;
    [SerializeField]
    [Tooltip("Color of the matrix items not on the diagonal")]
    private MatrixItemColor notDiagnonalColors;

    [Space]

    [SerializeField]
    [Tooltip("Colors that the operators use when performing row operations on the matrix")]
    private ArrayOnEnum<MatrixOperation.Type, Color> operationColors;
    [SerializeField]
    [Tooltip("Amount that an operator should punch it's scale when set as the source/destination")]
    private float operatorPunch = 0.1f;
    [SerializeField]
    [Tooltip("Time that the operator should take to punch it's scale when set as the source/destination")]
    private float operatorPunchTime = 0.2f;

    [Space]

    [SerializeField]
    [Tooltip("Sound played when a window appears")]
    private AudioClip windowOpenSound;
    [SerializeField]
    [Tooltip("Time it takes for a window to grow into view")]
    private float windowOpenTime = 0.3f;
    [SerializeField]
    [Tooltip("Sound played when a window disappears")]
    private AudioClip windowCloseSound;
    [SerializeField]
    [Tooltip("Time it takes for a window shrink out of view")]
    private float windowCloseTime = 0.3f;
    #endregion

    #region Public Methods
    public static void PunchOperator(Transform op)
    {
        op.DOComplete();
        op.DOPunchScale(Vector3.one * OperatorPunch, OperatorPunchTime);
    }
    public static Color GetOperatorColor(MatrixOperation.Type type) => Instance.operationColors.Get(type);
    public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> OpenWindow(RectTransform windowTransform)
    {
        // Play the appear sound
        AudioManager.PlaySFX(Instance.windowOpenSound);

        // Set scale to zero at first
        windowTransform.localScale = Vector3.zero;
        // Return the tween that makes the window appear
        return windowTransform.DOScale(1f, Instance.windowOpenTime).SetEase(Ease.OutBack);
    }
    public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> CloseWindow(RectTransform windowTransform)
    {
        // Play the appear sound
        AudioManager.PlaySFX(Instance.windowCloseSound);

        // Set scale to zero at first
        windowTransform.localScale = Vector3.one;
        // Return the tween that makes the window disappear
        return windowTransform.DOScale(0f, Instance.windowCloseTime).SetEase(Ease.OutQuint);
    }
    #endregion
}
