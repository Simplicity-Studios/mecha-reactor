using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // Health
    public float currentHealth = 100.0f;
    public RectTransform healthBar; 
    // Attack
    public float attackRange = 1.0f;
    public float attackSpeed = 1.0f;
    bool allowedAttack = true;
    // Movement 
    public float acceleration = 1.0f;
    public Rigidbody2D rigidbody;
    // keeps track of the player's position 
    // so that the enemy knows where to shoot and walk
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
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

        // try to attack player
        // if within range of player
        // and not currently on attack cooldown
        if (Vector2.Distance(player.transform.position, transform.position) <= attackRange
          && allowedAttack)
        {
            StartCoroutine(Attack());
        }

        // adjust healthbar 
        // healthbar is 200 wide so *2 converts to out of 200
        float barLevel = currentHealth * 2;
        healthBar.sizeDelta = new Vector2(barLevel, healthBar.sizeDelta.y);
        
    }

    // reduces the health of this enemy by a given amount
    void TakeDamage(float damage)
    {
        // enemy health cannot go lower than 0
        currentHealth = Mathf.Max(currentHealth - damage, 0.0f);
    }

    // Initiates enemy's attack 
    // If player is within range, player will be hurt
    IEnumerator Attack()
    {
        // prevent from attacking again until this is done
        allowedAttack = false;
        // Attack the player
        Debug.Log("Hiyah! You ded (enemy attacks)");
        // wait until allowed to attack again
        yield return new WaitForSeconds(attackSpeed);
        allowedAttack = true;
    }
}
