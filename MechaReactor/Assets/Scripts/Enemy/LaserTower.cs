using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : MonoBehaviour
{
    public float laserPower;
    public float seekingDistance;
    public Transform firePoint;
    public float chargeDuration = 3.0f;
    public float cooldown = 3.0f;

    public AudioClip laserCharge;
    public AudioClip laserFire;
    public AudioSource ChargeSource;
    public AudioSource FireSource;

    public Material laserMat;

    public GameObject player;
    private LineRenderer laser;
    LayerMask mask;

    private float laserWidth = 0.25f;

    private float lastFired = 0.0f;
    private float chargeTime = 0.0f;
    private bool isCharging = false;
    private bool hasFired = false;

    void Start()
    {
        player = GameObject.Find("Player");
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Obstacles");
        laser = GetComponent<LineRenderer>();
        ChargeSource.clip = laserCharge;
        FireSource.clip = laserFire;
    }
    /*
        WARNING

        ABANDON HOPE ALL YE WHO DARE TRY TO DEBUG THIS SCRIPT
    */
    void Update()
    {
        if(hasFired)
        {
            laserWidth -= Time.deltaTime * 2;
            laser.startWidth = Mathf.Clamp(laserWidth, 0.0f, 1.0f);
        }
        if(canSeePlayer() && Time.time > lastFired + cooldown)
        {
            if(!isCharging)
            {
                ChargeSource.Play();
                isCharging = true;
            }
            isCharging = true;
            if(!hasFired)
            {
                drawLaser();
            }
            chargeTime += Time.deltaTime;
            if(chargeTime >= chargeDuration && !hasFired)
            {
                StartCoroutine("laserCleanup");
                FireSource.Play();
                shootLaser();
                chargeTime = 0.0f;
                hasFired = true;
            }
        }
        else
        {
            laser.enabled = false;
            laserWidth = 0.25f;
            ChargeSource.Stop();
            isCharging = false;
            chargeTime = 0.0f;
        }
    }

    private bool canSeePlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, seekingDistance, mask);
        return (hit.collider != null && hit.collider.CompareTag("Player"));
    }

    private void drawLaser()
    {
        laserMat.color = new Color(1, 0, 0, 0.25f);
        laser.startWidth = laserWidth;
        Vector3 directionToPlayer = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, seekingDistance, mask);
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, hit.point);
        laser.enabled = true;
        laserWidth += Time.deltaTime/3;
    }

    private void shootLaser()
    {
        laserMat.color = new Color(1, 0, 0, 1);
        laser.enabled = true;
        player.GetComponent<PlayerController>().TakeDamage(laserPower);
    }

    IEnumerator laserCleanup()
    {
        yield return new WaitForSeconds(1.0f);
        lastFired = Time.time;
        isCharging = false;
        hasFired = false;
        laserWidth = 0.25f;
    }
}

// Yeah that was pretty bad.