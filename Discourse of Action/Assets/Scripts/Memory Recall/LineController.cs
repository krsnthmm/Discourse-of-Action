using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    public int _nodeID;

    private LineRenderer _lineRenderer;

    public bool isDragging;
    public Vector3 endPoint;

    private RevelationNode _revelationNode;

    public bool isMatched;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }

    public void UpdateLine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                _lineRenderer.SetPosition(0, mousePosition);
            }
        }
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _lineRenderer.SetPosition(1, mousePosition);
            endPoint = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
            if (hit.collider != null && hit.collider.TryGetComponent(out _revelationNode) && _nodeID == _revelationNode.GetID())
            {
                Debug.Log("Correct node!");
                enabled = false;
                isMatched = true;
            }
            else
            {
                _lineRenderer.positionCount = 0;
                isMatched = false;
            }

            _lineRenderer.positionCount = 2;
        }
    }
}
