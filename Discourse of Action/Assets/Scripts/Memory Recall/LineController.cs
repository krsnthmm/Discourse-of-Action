using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private List<Vector3> _points = new();

    public bool isLineStarted;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetUpLine(Vector3 startPoint, Vector3 endPoint)
    {
        _points.Add(startPoint);
        _points.Add(endPoint);

        _lineRenderer.positionCount = _points.Count;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
            _lineRenderer.SetPosition(i, _points[i]);

        isLineStarted = true;
    }

    // Update is called once per frame
    public void UpdateLine(Vector2 endPoint)
    {
        _lineRenderer.SetPosition((_lineRenderer.positionCount - 1), endPoint);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _points.Clear();

            _lineRenderer.positionCount = 0;
            isLineStarted = false;
        }
    }
}
