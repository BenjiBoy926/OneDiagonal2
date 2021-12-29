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

        // The matrix UI seems too congested for all the fun operation lines to fit
        // There must be some other way to add clarity to some of the more confusing math concepts...

        // Get all matrix operation types
        //MatrixOperation.Type[] types = (MatrixOperation.Type[])System.Enum.GetValues(typeof(MatrixOperation.Type));
        //foreach(MatrixOperation.Type type in types)
        //{
        //    // Set this list of line instances to a new list
        //    lineInstances.Set(type, new List<UILine>());

        //    // Create a line for each column in the matrix
        //    for(int i = 0; i < MatrixParent.Cols; i++)
        //    {
        //        UILine lineInstance = Instantiate(linePrefabs.Get(type), canvas.transform);
        //        lineInstances.Get(type).Add(lineInstance);
        //        lineInstance.gameObject.SetActive(false);
        //    }
        //}

        //// Listen for operation events
        //MatrixParent.OnOperationDestinationSet.AddListener(OnOperationSet);
        //MatrixParent.OnOperationDestinationUnset.AddListener(OnOperationUnset);
        //MatrixParent.OnOperationFinish.AddListener(x => OnOperationFinished());
    }
    #endregion

    #region Event Listeners
    private void OnOperationSet()
    {
        MatrixOperation.Type intendedType = MatrixParent.IntendedNextOperationType;

        if (intendedType == MatrixOperation.Type.Scale)
        {
            // Not implemented yet
        }
        // For anything other than scale, just make the source point to the destination
        else PointFromSourceToDestination(lineInstances.Get(intendedType));
    }
    // Operation unset is the same as operation confirm -
    // just disable the lines
    private void OnOperationUnset()
    {
        OnOperationFinished();
    }
    private void OnOperationFinished()
    {
        // Disable all of the lines
        foreach (List<UILine> instances in lineInstances.Data)
        {
            foreach (UILine line in instances)
            {
                line.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Helper Methods
    // Point each of the lines in the list from each matrix item in the source
    // to each matrix item in the destination
    private void PointFromSourceToDestination(List<UILine> lines)
    {
        MatrixOperation intendedOperation = MatrixParent.IntendedNextOperation;
        MatrixRowUI source = MatrixParent.RowUIs[intendedOperation.sourceRow];
        MatrixRowUI destination = MatrixParent.RowUIs[intendedOperation.destinationRow];

        // Set the start/end points of each ui line in the list
        // to point from each source item to each destination item
        for(int i = 0; i < MatrixParent.Cols; i++)
        {
            Vector3 start = source.ItemUIs[i].transform.position;
            Vector3 end = destination.ItemUIs[i].transform.position;

            // Enable the line and set the points
            lines[i].gameObject.SetActive(true);
            lines[i].SetPoints(start, end);
        }
    }
    #endregion
}
