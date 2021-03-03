using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    [Range(1,10)]
    public float smoothFactor;
    private Vector3 offset = new Vector3(0, 0, -10);

    public Vector2 mouseBounds;

    void FixedUpdate()
    {
        AverageFollow();
    }

    void HardFollow()
    {
        Vector3 playerPosition = target.position + offset;
        Vector3 targetPosition = playerPosition;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }

    void AverageFollow()
    {
        Vector3 playerPosition = (target.position + offset);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(
            Mathf.Clamp(mousePosition.x, playerPosition.x - mouseBounds.x, playerPosition.x + mouseBounds.x),
            Mathf.Clamp(mousePosition.y, playerPosition.y - mouseBounds.y, playerPosition.y + mouseBounds.y),
            mousePosition.z
        );
        Vector3 targetPosition = (playerPosition + mousePosition) / 2;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}
