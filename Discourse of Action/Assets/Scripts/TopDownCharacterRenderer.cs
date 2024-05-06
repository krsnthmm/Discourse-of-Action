using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterRenderer : MonoBehaviour
{
    public static readonly string[] idleDirections = { "Idle N", "Idle W", "Idle S", "Idle E" };
    public static readonly string[] walkingDirections = { "Walking N", "Walking W", "Walking S", "Walking E" };

    Animator animator;
    int lastDirection;

    private void Awake()
    {
        // cache the animator component
        animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        // use the Run states by default
        string[] directionArray = null;

        // measure the magnitude of the input
        if (direction.magnitude < .01f)
        {
            // if we are basically standing still, we'll use the Static states
            // we won't be able to calculate a driection if the user isn't pressing one, anyway!
            directionArray = idleDirections;
        }
        else
        {
            // we can calculate each direction we are going in
            // use DirectionToIndex to get the index of the slice from the direction vector
            // save the answer to lastDirection
            directionArray = walkingDirections;
            lastDirection = DirectionToIndex(direction, 4);
        }

        // tell the animator to play the requested state
        animator.Play(directionArray[lastDirection]);
    }

    // this function converts a Vector2 direction to an index to a slice around a circle
    // this goes in a counter-clockwise direction.
    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        // get the normalized direction
        Vector2 normDir = dir.normalized;

        // calculate how many degrees one slide ic
        float step = 360f / sliceCount;

        // calculate how many degrees half a slide is.
        // we need this to offset the pie, so that the North (UP) slide is aligned in the center
        float halfstep = step / 2;

        // get the angle from -180 to 180 of the direction vector relative to the up vector
        // this will return the angle between dir and North.
        float angle = Vector2.SignedAngle(Vector2.up, normDir);

        // add the halfslice offset
        angle += halfstep;

        // if angle is negative, then let's make it positive
        if (angle < 0) angle += 360;

        // calculate the amount of steps required to reach this angle
        float stepCount = angle / step;

        // round it, and we have the answeer!
        return Mathf.FloorToInt(stepCount);
    }
}