using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatrixUIChild : UIBehaviour
{
    #region Public Properties
    public MatrixUI MatrixParent => matrixParent;
    #endregion

    #region Private Fields
    private MatrixUI matrixParent;
    #endregion

    protected override void Start()
    {
        base.Start();

        matrixParent = GetComponentInParent<MatrixUI>();

        if(!matrixParent)
        {
            Debug.LogWarning("MatrixUIChild: expected to find a component of type 'MatrixUI' in this object or one of its parents, " +
                "but no such component was found.  Did you place the object in the heirarchy correctly?", gameObject);
        }
    }
}
