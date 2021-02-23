using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{

    // keeps track of the player's position 
    // so that the enemy knows where to shoot
    public GameObject player;
    LayerMask mask; 

    [Range(0.0f, 2.0f)]
    public float attackSpeed = 1.0f;
    public float attackDamage = 10.0f;
    private float lastAttack = 0.0f;

    public float bulletForce = 15.0f;
    public Transform bulletSpawnLocation;
    public GameObject bulletPrefab;

    void Start()
    {
        player = GameObject.Find("Player");
        // Enemy can only see players and obstacles
        // Things like bullets/enemies are ignored 
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Obstacles");
    }

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, 100.0f, mask);
        // Only shoot at player if they are in line of sight
        if (hit.collider != null 
            && hit.collider.gameObject.name == player.name 
            && Time.time > attackSpeed + lastAttack)
        {
            Shoot();
            lastAttack = Time.time;
        }
    }

    void Shoot()
    {
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
        bullet.GetComponent<EnemyBullet>().setBulletDamage(attackDamage);
        // Send bullet
        Rigidbody2D bulletrigid = bullet.GetComponent<Rigidbody2D>();
        bulletrigid.AddForce(bulletSpawnLocation.up * bulletForce, ForceMode2D.Impulse);
    }

}
