using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public GameObject hitEffect;
    public Collider2D collider; 

    private float dmg;
    public float maxSize = 8f;
    public float changeRate = 0.10f;

    private float timeCreated;
    private float maxLifetime = 10f;

    public bool willIgnoreHitstop = false;

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
        if (transform.localScale.x < maxSize)
        {
            transform.localScale = new Vector2(transform.localScale.x + changeRate,
                                               transform.localScale.y + changeRate);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Collisions with the Player 
        if(col.transform.CompareTag("Player"))
        {
            if(willIgnoreHitstop)
                col.gameObject.GetComponent<PlayerController>().TakeDamageIgnoreHitstop(dmg);
            else
                col.gameObject.GetComponent<PlayerController>().TakeDamage(dmg);
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            Destroy(gameObject);
        }
    }

    public void setDamage(float newDmg)
    {
        dmg = newDmg;
    }
}
