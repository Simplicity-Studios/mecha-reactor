using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableTile : MonoBehaviour
{
    public GameObject destructionEffect;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.CompareTag("EnemyMissile") || col.transform.CompareTag("Boss"))
        {
            breakTile();
        }
    }

    public void breakTile()
    {
        GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }
}
