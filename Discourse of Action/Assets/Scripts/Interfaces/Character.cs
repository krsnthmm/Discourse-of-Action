using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterRenderer))]
public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterData _data;
    protected CharacterMovement _movement;
    protected CharacterRenderer _renderer;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _renderer = GetComponent<CharacterRenderer>();
    }

    private void Start()
    {
        HandleFacingDirection(_renderer.defaultDirection);
    }

    protected void HandleMovement(float x, float y, UnityAction action = null)
    {
        _movement.UpdatePosition(x, y, action);

        if (x != 0 || y != 0)
        {
            _renderer.SetFloat("x", Mathf.Clamp(x, -1f, 1f));
            _renderer.SetFloat("y", Mathf.Clamp(y, -1f, 1f));
        }
    }

    public virtual void HandleFacingDirection(FacingDirection dir)
    {
        _renderer.SetFacingDirection(dir);
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xDiff = Mathf.Floor(2 * targetPos.x) - Mathf.Floor(2 * transform.position.x);
        var yDiff = Mathf.Floor(2 * targetPos.y) - Mathf.Floor(2 * transform.position.y);

        if (xDiff == 0 || yDiff == 0)
        {
            _renderer.SetFloat("x", Mathf.Clamp(xDiff, -1f, 1f));
            _renderer.SetFloat("y", Mathf.Clamp(yDiff, -1f, 1f));
        }
        else
            Debug.Log("Error: You can't ask the NPC to look diagonally.");

    }
}
