using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings")]
public class UISettings : ScriptableObject
{
    #region Private Properties
    private static UISettings Instance
    {
        get
        {
            if(!instance)
            {
                // Load the resource
                instance = Resources.Load<UISettings>(nameof(UISettings));

                // If the instance still could not be loaded then throw an exception
                if (!instance) throw new MissingReferenceException(nameof(UISettings) + ": no instance of type " + nameof(UISettings) + 
                    " could be loaded from the resources folder. Make sure an instance of type " +
                    nameof(UISettings) + " with the name " + nameof(UISettings) + 
                    " exists in a folder named 'Resources'");
            }
            // Return the loaded instance
            return instance;
        }
    }
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

    #region Private Fields
    // Reference to the ui settings singleton instance
    private static UISettings instance;
    #endregion

    #region Public Methods
    public static Color GetOperatorColor(MatrixOperation.Type type) => Instance.operationColors.Get(type);
    #endregion
}
