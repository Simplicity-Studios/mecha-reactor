﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

// =======================================================================

public class EnemyController : MonoBehaviour
{
    // Health
    public float currentHealth = 100.0f;
    public RectTransform healthBar; 
    // Movement 
    [Header( "Movement" )]
    public float movementSpeed = 2.0f;
    public Rigidbody2D rigidbody; // for moving the enemy's position
    public Transform spriteTransform; // For rotating sprite and firepoint
    private float lastPathFindMoveTime; 
    public float pathFindMoveDuration = 0.5f;
    public Transform infrontPoint;
    public bool canRotate = true;
    // keeps track of the player's position 
    // so that the enemy knows where to shoot and walk
    public GameObject player;
    LayerMask mask; 
    public NavMeshAgent agent;

    // ===================================================================

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        // Enemy can only see players and obstacles
        // Things like bullets are ignored 
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Obstacles");

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        // sync rigidbody speed with nav agent
        agent.speed = movementSpeed;
    }

    // ===================================================================

    // Update is called once per frame
    void Update()
    {
        // Death check
        if(currentHealth <= 0.0f)
            die();

        // Movement 
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // determine if the enemy can follow a straight line 
        // to the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, 100.0f, mask);
        if (hit.collider != null) 
        {

            // Look at player
            if(canRotate)
            {
                var angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
                spriteTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // obstacle infront of enemy
            if (hit.collider.gameObject.name != player.name) 
            {
                // Only set destination every x seconds
                if (Time.time > pathFindMoveDuration + lastPathFindMoveTime)
                {
                    agent.speed = movementSpeed;
                    agent.SetDestination(player.transform.position);
                    // Mark time of random move 
                    lastPathFindMoveTime = Time.time;
                }
            }
            // no obstacles 
            else
            {
                // Move towards player
                agent.speed = 0;
                rigidbody.velocity = directionToPlayer.normalized * movementSpeed;
            }
        }

        // adjust healthbar 
        // healthbar is 200 wide so *2 converts to out of 200
        float barLevel = currentHealth * 2;
        healthBar.sizeDelta = new Vector2(barLevel, healthBar.sizeDelta.y);
        
    }

    // ===================================================================

    // reduces the health of this enemy by a given amount
    public void TakeDamage(float damage)
    {
        // enemy health cannot go lower than 0
        currentHealth = Mathf.Max(currentHealth - damage, 0.0f);
    }

    // ===================================================================

    // Put any logic about this unit being destroyed in here
    // i.e sound fxs, particles, whatever
    public void die()
    {
        Destroy(gameObject);
    }

    // ===================================================================
}
