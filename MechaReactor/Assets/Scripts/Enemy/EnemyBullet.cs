using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject hitEffect;
    public Collider2D collider; 

    private float dmg;

    private float timeCreated;
    private float maxLifetime = 5f;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        timeCreated = Time.time;
        maxLifetime += timeCreated;
    }

    void Update()
    {
        if(Time.time > maxLifetime)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Collisions with the Player 
        if(col.transform.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(dmg);
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            Destroy(gameObject);
        }
        
        // Collide with walls and obstacles
        else if (col.gameObject.layer == LayerMask.NameToLayer("Obstacles")) 
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            Destroy(gameObject);
        }
    }

    public void setBulletDamage(float newDmg)
    {
        dmg = newDmg;
    }
}
