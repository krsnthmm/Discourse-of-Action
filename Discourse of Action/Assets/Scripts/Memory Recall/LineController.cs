using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public bool isDragging;
    public Vector3 endPoint;

    private RevelationNode _revelationNode;
    private int _correctNodeID;

    public bool isMatched;
    public bool isEverythingCorrect;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
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

                AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);
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
            if (hit.collider != null && hit.collider.TryGetComponent(out _revelationNode) && ((!isEverythingCorrect && _correctNodeID == _revelationNode.GetID()) || isEverythingCorrect))
            {
                isMatched = true;
                _lineRenderer.positionCount = 0;
            }
            else
            {
                isMatched = false;
                _lineRenderer.positionCount = 0;
            }

            _lineRenderer.positionCount = 2;
            AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);
        }
    }

    public void SetCorrectNodeID(int index)
    {
        _correctNodeID = index;
    }
}
