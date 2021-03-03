using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : MonoBehaviour
{

    public GameObject player;
    public Transform shieldPivot;
    public float shieldRotateSpeed = 10f; 
    
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 desiredDirection = (player.transform.position - shieldPivot.position);
        // Calculate desired angle 
        // - 90f because by default, 0 degrees is pointing to the right
        // the enemy's 0 degrees is considered North because that is the direction 
        // the sprite faces and shoots 
        float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion desiredRotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
        // slowly rotate towards player
        float steps = Time.deltaTime * shieldRotateSpeed;
        shieldPivot.rotation = Quaternion.Slerp(shieldPivot.rotation, desiredRotation, steps);
    }
}
