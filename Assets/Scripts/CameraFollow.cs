using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(1.5f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    public PlayerController pc;

    [SerializeField] private Transform target;

    private float count = 0.0f;
    private bool finishedCount = false;

    private void FixedUpdate()
    {
        /*
        count += Time.fixedDeltaTime;
        finishedCount = false;

        if (pc.isWallSliding)// || pc.canClimb)
        {
            offset = new Vector3(2f, 0f, -10f);
            count = 0.0f;
            finishedCount = true;
        }

        if (!pc.sr.flipX && (Mathf.Abs(pc.movementInputDirection) > 0) && count > 0.5)
        {
            offset = new Vector3(2f, 0f, -10f);
            count = 0.0f;
            finishedCount = true;
        }
        else if (pc.sr.flipX && (Mathf.Abs(pc.movementInputDirection) > 0) && count > 1)
        {
            offset = new Vector3(0f, 0f, -10f);
            count = 0.0f;
            finishedCount = true;
        }
        else if (!pc.sr.flipX && (Mathf.Abs(pc.movementInputDirection) == 0) && count > 0.5)
        {
            offset = new Vector3(2f, 0f, -10f);
            count = 0.0f;
            finishedCount = true;
        }
        else if (pc.sr.flipX && (Mathf.Abs(pc.movementInputDirection) == 0) && count > 1)
        {
            offset = new Vector3(0f, 0f, -10f);
            count = 0.0f;
            finishedCount = true;
        }
        */
    }

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}