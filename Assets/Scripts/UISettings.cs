using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UISettings : ScriptableObjectSingleton<UISettings>
{
    #region Private Properties
    private static UISettings Instance => GetOrCreateInstance(nameof(UISettings), nameof(UISettings));
    #endregion

    #region Public Properties
    public static MatrixItemColor DiagonalColors => Instance.diagonalColors;
    public static MatrixItemColor NotDiagonalColors => Instance.notDiagnonalColors;
    public static MatrixFlashEffect FlashEffect => Instance.flashEffect;
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
    [SerializeField]
    [Tooltip("Reference to the prefab to use for the operator flash effect")]
    private MatrixFlashEffect flashEffect;
    [SerializeField]
    [Tooltip("Colors that the operators use when performing row operations on the matrix")]
    private ArrayOnEnum<MatrixOperation.Type, Color> operationColors;
    [SerializeField]
    [Tooltip("Amount that an operator should punch it's scale when set as the source/destination")]
    private float operatorPunch = 0.1f;
    [SerializeField]
    [Tooltip("Time that the operator should take to punch it's scale when set as the source/destination")]
    private float operatorPunchTime = 0.2f;
    #endregion

    #region Public Methods
    public static Color GetOperatorColor(MatrixOperation.Type type) => Instance.operationColors.Get(type);
    #endregion
}
