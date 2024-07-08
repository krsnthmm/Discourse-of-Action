using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _multiplier = 0.5f;

    public bool isMoving;

    public void UpdatePosition(float horizontalInput, float verticalInput, UnityAction action = null)
    {
        var targetPos = transform.position;
        targetPos.x += horizontalInput * _multiplier;
        targetPos.y += verticalInput * _multiplier;

        if (IsWalkable(targetPos))
            StartCoroutine(Move(targetPos, action));
        else
            action?.Invoke();
    }

    IEnumerator Move(Vector3 targetPos, UnityAction action = null)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        action?.Invoke();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.Instance.SolidLayer | GameLayers.Instance.InteractableLayer | GameLayers.Instance.PlayerLayer) != null)
            return false;

        return true;
    }
}
