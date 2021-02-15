using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ReactorAttributes stats;

    //MOVEMENT
    private float horizontal;
    private float vertical;
    private Rigidbody2D r;
    private Vector2 moveVelocity;

    void Start()
    {
        stats = this.GetComponent<ReactorAttributes>();
        r = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * stats["movementSpeed"];
        //Look at mouse
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FixedUpdate()
    {
        r.MovePosition(r.position + moveVelocity * Time.fixedDeltaTime);
    }
}
