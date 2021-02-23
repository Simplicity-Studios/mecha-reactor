using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D roomBounds;
    [Range(1,10)]
    public float smoothFactor;
    private Vector3 offset = new Vector3(0, 0, -10);

    private float boundsX;
    private float boundsY;

    void Start()
    {
        boundsX = roomBounds.size.x / 2;
        boundsY = roomBounds.size.y / 2;
    }

    public void setBounds(float x, float y, float xOffset, float yOffset)
    {
        boundsX = (x / 2) + Mathf.Abs(xOffset);
        boundsY = (y / 2) + Mathf.Abs(yOffset);
    }

    void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 playerPosition = target.position + offset;
        playerPosition = new Vector3(
            Mathf.Clamp(playerPosition.x, -boundsX, boundsX),
            Mathf.Clamp(playerPosition.y, -boundsY, boundsY),
            playerPosition.z
        );

        Vector3 targetPosition = playerPosition;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}
