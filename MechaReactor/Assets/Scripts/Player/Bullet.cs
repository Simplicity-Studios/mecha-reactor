using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;

    private float dmg;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.transform.CompareTag("Shield"))
        {
            // do nothing
        }
        else if(col.transform.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<EnemyController>().TakeDamage(dmg);
        } 
        else if(col.transform.CompareTag("Absorber"))
        {
            col.gameObject.GetComponent<EnemyAbsorber>().ProcessDamage(dmg);
        } 
        else if(col.transform.CompareTag("Boss"))
        {
            col.gameObject.GetComponent<FinalBoss>().ProcessDamage(dmg);
        }
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }

    public void setBulletDamage(float newDmg)
    {
        dmg = newDmg;
    }
}
