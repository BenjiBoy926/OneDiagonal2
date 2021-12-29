using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixOperationUI : MatrixUIChild
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Main canvas to instantiate all lines under")]
    private Canvas canvas;
    [SerializeField]
    [Tooltip("Line prefab to use for each operation type")]
    private ArrayOnEnum<MatrixOperation.Type, UILine> linePrefabs;
    #endregion

    #region Private Fields
    private ArrayOnEnum<MatrixOperation.Type, List<UILine>> lineInstances = new ArrayOnEnum<MatrixOperation.Type, List<UILine>>();
    #endregion

    #region Child Overrides
    protected override void Start()
    {
        base.Start();

        MatrixOperation.Type[] types = (MatrixOperation.Type[])System.Enum.GetValues(typeof(MatrixOperation.Type));
        foreach(MatrixOperation.Type type in types)
        {
            // Set this list of line instances to a new list
            lineInstances.Set(type, new List<UILine>());

            // Create a line for each column in the matrix
            for(int i = 0; i < MatrixParent.Cols; i++)
            {
                UILine lineInstance = Instantiate(linePrefabs.Get(type), canvas.transform);
                lineInstances.Get(type)[i] = lineInstance;
                lineInstance.gameObject.SetActive(false);
            }
        }

        //MatrixParent.OnOp
    }
    #endregion
}
