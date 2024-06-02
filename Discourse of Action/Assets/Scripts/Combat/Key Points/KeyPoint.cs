using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyPointDisplay))]
public class KeyPoint : MonoBehaviour
{
    public KeyPointData keyPointData;
    public GameObject target;

    public int displayIndex;

    private KeyPointManager _keyPointManager;

    private void Start()
    {
        _keyPointManager = FindObjectOfType<KeyPointManager>();
    }

    public void SetKeyPoint()
    {
        _keyPointManager.selectedKeyPoint = this;
        _keyPointManager.SetTarget(this);
    }
}
