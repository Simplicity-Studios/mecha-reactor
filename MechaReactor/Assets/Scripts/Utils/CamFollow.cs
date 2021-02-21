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

    public void setBounds(float x, float y)
    {
        boundsX = x / 2;
        boundsY = y / 2;
    }

    void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = ((target.position + offset) + Camera.main.ScreenToWorldPoint(Input.mousePosition)) / 2;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        smoothedPosition = new Vector3(
            Mathf.Clamp(smoothedPosition.x, -boundsX, boundsX),
            Mathf.Clamp(smoothedPosition.y, -boundsY, boundsY),
            smoothedPosition.z
        );
        transform.position = smoothedPosition;
    }
}
