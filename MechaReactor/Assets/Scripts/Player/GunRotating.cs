using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotating : MonoBehaviour
{
    private Transform gunLeft;
    private Transform gunRight;

    void Start()
    {
        gunLeft = transform.Find("GunL");
        gunRight = transform.Find("GunR");
    }

    void Update()
    {    
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 directionLeftGun = new Vector2(
            mousePos.x - gunLeft.position.x,
            mousePos.y - gunLeft.position.y
        );

        Vector2 directionRightGun = new Vector2(
            mousePos.x - gunRight.position.x,
            mousePos.y - gunRight.position.y
        );

        gunLeft.up = directionLeftGun;
        gunRight.up = directionRightGun;
    }
}
