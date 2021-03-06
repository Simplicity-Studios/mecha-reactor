using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public FinalBoss finalboss;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.CompareTag("Player"))
        {
            float dmg = finalboss.hurtboxDamage;
            col.transform.GetComponent<PlayerController>().TakeDamage(dmg);
        }
    }
}
