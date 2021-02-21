using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedObstacle : MonoBehaviour
{
    public float speedRequirement;
    public GameObject explosionParticles;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.CompareTag("Player"))
        {
            ReactorAttributes stats = col.transform.GetComponent<ReactorAttributes>();
            if(stats["movementSpeed"] >= speedRequirement)
            {
                breakTile();
            }
        }
    }

    public void breakTile()
    {
        GameObject effect = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
