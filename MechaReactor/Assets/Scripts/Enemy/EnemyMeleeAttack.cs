using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{

    [Range(0.0f, 2.0f)]
    public float attackSpeed = 1.0f;
    public float attackDamage = 10.0f;
    public float attackRange = 1.0f;
    private float lastAttack = 0.0f;

    public GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // if within range of player
        // and not currently on attack cooldown
        if (Vector2.Distance(player.transform.position, transform.position) <= attackRange
            && Time.time > attackSpeed + lastAttack
            && !GetComponent<EnemyController>().isDying)
        {
            MeleeAttack();
            lastAttack = Time.time;
        }
    }

    // Initiates enemy's attack 
    public void MeleeAttack()
    {
        // Attack the player
        player.GetComponent<PlayerController>().TakeDamage(attackDamage);
    }
}
