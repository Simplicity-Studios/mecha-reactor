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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(
            Mathf.Clamp(mousePosition.x, -boundsX, boundsX),
            Mathf.Clamp(mousePosition.y, -boundsY, boundsY),
            mousePosition.z
        );
        
        Vector3 playerPosition = target.position + offset;
        playerPosition = new Vector3(
            Mathf.Clamp(playerPosition.x, -boundsX, boundsX),
            Mathf.Clamp(playerPosition.y, -boundsY, boundsY),
            playerPosition.z
        );

        Vector3 targetPosition = (playerPosition + mousePosition) / 2;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}
