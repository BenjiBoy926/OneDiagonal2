using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectConveyor : MonoBehaviour
{
    #region Public Typedefs
    public enum EndPointType
    {
        Implicit, Explicit
    }
    #endregion

    #region Public Properties
    public Vector3 LocalEndPoint
    {
        get
        {
            if (endPointType == EndPointType.Implicit)
            {
                return localStartPoint + Length * Direction;
            }
            else return localEndPoint;
        }
    }
    public float Length
    {
        get
        {
            if (endPointType == EndPointType.Implicit)
            {
                return numberOfObjects * objectOffset;
            }
            else return (localEndPoint - localStartPoint).magnitude;
        }
    }
    public Vector3 Direction
    {
        get
        {
            if (endPointType == EndPointType.Implicit)
            {
                return direction.normalized;
            }
            else return (localEndPoint - localStartPoint).normalized;
        }
    }
    public float ObjectOffset
    {
        get
        {
            if (endPointType == EndPointType.Implicit)
            {
                return objectOffset;
            }
            else return Length / numberOfObjects;
        }
    }
    public Vector3 GlobalStartPoint => transform.TransformPoint(localStartPoint);
    public Vector3 GlobalEndPoint => transform.TransformPoint(LocalEndPoint);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Point to start the conveyor relative to this object")]
    private Vector3 localStartPoint;
    [SerializeField]
    [Tooltip("Determine how the end point of the conveyor is calculated")]
    private EndPointType endPointType;
    [SerializeField]
    [Tooltip("Point relative to this object where the conveyor end")]
    private Vector3 localEndPoint;
    [SerializeField]
    [Tooltip("Direction that the objects are moved towards")]
    private Vector3 direction;
    [SerializeField]
    [Tooltip("Distance between each object on the conveyor")]
    private float objectOffset;
    [SerializeField]
    [Tooltip("Object prefab to instantiate into the conveyor")]
    private Transform prefab;
    [SerializeField]
    [Tooltip("The number of objects to be conveyed")]
    private int numberOfObjects;
    #endregion

    #region Private Fields
    private List<Transform> pool = new List<Transform>();
    #endregion

    #region Monobehaviour Messages
    private void OnDrawGizmosSelected()
    {
        // Draw a line from start to finish
        Gizmos.color = Color.white;
        Gizmos.DrawLine(GlobalStartPoint, GlobalEndPoint);

        // Draw start point
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(GlobalStartPoint, 10f);

        // Draw end point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GlobalEndPoint, 10f);

        // Draw each intermediate point
        Gizmos.color = Color.green;
        for (int i = 0; i < numberOfObjects; i++)
        {
            Gizmos.DrawSphere(ObjectGlobalStartPosition(i), 5f);
        }
    }
    #endregion

    #region Public Methods
    public Vector3 ObjectLocalStartPosition(int index)
    {
        return localStartPoint + (index * ObjectOffset * Direction);
    } 
    public Vector3 ObjectGlobalStartPosition(int index) => transform.TransformPoint(ObjectLocalStartPosition(index));
    #endregion
}
