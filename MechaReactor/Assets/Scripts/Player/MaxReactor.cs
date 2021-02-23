using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxReactor : MonoBehaviour
{
    private ReactorAttributes stats;
    private PlayerController playerController;
    
    public GameObject missile;
    public float missileBaseAttackTime = 1.5f;
    public float missileForce = 20.0f;
    public Transform missileFirePoint;
    private float lastMissileShot = 0.0f;


    void Start()
    {
        stats = this.GetComponent<ReactorAttributes>();
        playerController = this.GetComponent<PlayerController>();
    }

    void Update()
    {
        // attack
        if(stats["attack"].pointsAllocated == 3)
        {
            AddMissileAttack();
        }
        // defense
        MakeImmuneToEMP(stats["defense"].pointsAllocated == 3);
    }

    void AddMissileAttack()
    {
        bool reactorButtonMouseOver = GetComponent<PlayerController>().MouseOverReactorButton();
        if(!reactorButtonMouseOver && Input.GetButton("Fire1") && Time.time > (missileBaseAttackTime + lastMissileShot))
        {
            GameObject m = Instantiate(missile, missileFirePoint.position, missileFirePoint.rotation);
            m.GetComponent<Bullet>().setBulletDamage(stats["attack"].GetValue() + 5f);
            Rigidbody2D bulletrigid = m.GetComponent<Rigidbody2D>();
            bulletrigid.AddForce(missileFirePoint.up * missileForce, ForceMode2D.Impulse);
            lastMissileShot = Time.time;
        }
    }

    void MakeImmuneToEMP(bool state)
    {
        if(state)
        {
            playerController.isImmuneToEMP = state;
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
        }
        else
        {
            playerController.isImmuneToEMP = state;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
