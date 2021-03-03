using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.CompareTag("Player"))
        {
            float dmg = transform.parent.parent.GetComponent<FinalBoss>().Laser.laserPower;
            col.GetComponent<PlayerController>().TakeDamage(dmg);
        }
    }
}
