using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // Health
    public int currentHealth = 100;
    public RectTransform healthBar; 
    // Attack
    public float attackRange = 3.0f;
    // Movement 
    public float acceleration = 1.0f;
    public Rigidbody2D rigidbody;
    // keeps track of the player's position 
    // so that the enemy knows where to shoot and walk
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 directionToPlayer = player.transform.position - transform.position;

        // determine if the enemy can follow a straight line 
        // to the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer);
        if (hit.collider != null) 
        {
            // obstacle infront of enemy
            if (hit.collider.gameObject.name != player.name) 
            {
                // move randomly 
                Vector2 randomDirection = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
                rigidbody.AddForce(randomDirection.normalized * acceleration);
            }
            // no obstacles 
            else 
            {
                // reduce velocity to prevent orbiting behavior 
                rigidbody.velocity = rigidbody.velocity / 1.75f; 
                rigidbody.AddForce(directionToPlayer * acceleration);
            }
        }

        // adjust healthbar 
        // healthbar is 200 wide so *2 converts to out of 200
        healthBar.sizeDelta = new Vector2(currentHealth * 2, healthBar.sizeDelta.y);
        
    }

    // reduces the health of this enemy by a given amount
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    // Initiates enemy's attack 
    // If player is within range, player will be hurt
    void Attack()
    {

    }
}
