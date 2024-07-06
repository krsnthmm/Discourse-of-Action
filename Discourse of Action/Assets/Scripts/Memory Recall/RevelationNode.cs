using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class RevelationNode : MonoBehaviour
{
    [SerializeField] private int _nodeID;

    public int GetID()
    {
        return _nodeID;
    }
}
