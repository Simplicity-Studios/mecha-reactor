using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ReactorAttributes stats;

    //MOVEMENT
    private float horizontal;
    private float vertical;
    private Rigidbody2D r;
    private Vector2 moveVelocity;

    [Header( "Shooting" )]
    [Range(0.0f, 2.0f)]
    public float baseAttackTime;
    public float bulletForce;
    public Transform bulletSpawnLocation;
    public GameObject bulletPrefab;
    private float lastShot = 0.0f;

    private float maxHealth = 300.0f;
    private float currHealth;
    private float currDamage;

    void Start()
    {
        stats = this.GetComponent<ReactorAttributes>();
        r = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        currDamage = 0.0f;
    }

    void Update()
    {
        //Movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = stats["movementSpeed"] * moveInput.normalized * 5;

        //Look at mouse
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Input for Shooting
        if(Input.GetButton("Fire1") && Time.time > (baseAttackTime - stats["attackSpeed"]) + lastShot)
        {
            Shoot();
            lastShot = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            currHealth -= 1;
    }

    void FixedUpdate()
    {
        r.MovePosition(r.position + moveVelocity * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
        bullet.GetComponent<Bullet>().setBulletDamage(stats["attack"]);
        
        Rigidbody2D bulletrigid = bullet.GetComponent<Rigidbody2D>();
        bulletrigid.AddForce(bulletSpawnLocation.up * bulletForce, ForceMode2D.Impulse);
    }

    public float GetHealth()
    {
        return currHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
