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

    public bool givesHitstop = true;

    public GameObject player;
    private LineRenderer laser;
    LayerMask mask;

    private float laserWidth = 0.25f;
    private EnemyController controller;

    private float lastFired = 0.0f;
    private float chargeTime = 0.0f;
    private bool isCharging = false;
    private bool hasFired = false;

    void Start()
    {
        controller = this.GetComponent<EnemyController>();
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
        // If the laser tower is in the process of dying don't bother with lasers
        if(controller.isDying)
        {
            laser.enabled = false;
            return;
        }
        // Modifies the width of the laser after it has been fired
        if(hasFired)
        {
            laserWidth -= Time.deltaTime * 2;
            laser.startWidth = Mathf.Clamp(laserWidth, 0.0f, 1.0f);
        }
        // If we can see the player, cooldown is up, and the player isn't invulerable, start charging
        if(canSeePlayer() && Time.time > lastFired + cooldown && !player.GetComponent<PlayerController>().isInvulnerable)
        {
            // If we're not charging, play the sound once
            // This is needed otherwise the charge sound will play 1 million times
            if(!isCharging)
            {
                ChargeSource.Play();
                isCharging = true;
            }
            isCharging = true;
            //If we haven't fired yet, draw the laser and increase the charge time
            if(!hasFired)
            {
                drawLaser();
            }
            chargeTime += Time.deltaTime;
            // If the charge time is up, stop the charge up sound effect and fire the laser
            if(chargeTime >= chargeDuration && !hasFired)
            {
                ChargeSource.Stop();
                StartCoroutine("laserCleanup");
                FireSource.Play();
                shootLaser();
                chargeTime = 0.0f;
                hasFired = true;
            }
        }
        // If we can't see the player, basically reset everything
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

    // Draws the initial line from the tower to the player. 
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
        if(givesHitstop)
        {
            player.GetComponent<PlayerController>().TakeDamage(laserPower);
        }
        else 
        {
            player.GetComponent<PlayerController>().TakeDamageIgnoreHitstop(laserPower);   
        }
    }
    // Actually cleans up the line renderer itself and resets all the boolean values
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