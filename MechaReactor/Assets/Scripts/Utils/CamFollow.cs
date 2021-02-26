using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    [Range(1,10)]
    public float smoothFactor;
    private Vector3 offset = new Vector3(0, 0, -10);

    void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 playerPosition = target.position + offset;
        Vector3 targetPosition = playerPosition;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}
