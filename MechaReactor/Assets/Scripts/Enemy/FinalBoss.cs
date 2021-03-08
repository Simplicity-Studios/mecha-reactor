using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public AbsorbClass Absorb;
    public BulletHellClass Bullet;
    public LaserClass Laser;
    public MissilesClass Missile;

    public float attackIntervals;
    public float hurtboxDamage;
 
    public Animator anim;
    public AudioSource angryRoar;
    public AudioSource deathSound;
    public int numOfExplosions;
    public GameObject deathEffect;
    public GameObject spriteRender;
    public GameObject healthBar;
    public GameObject finalExplosion;

    private EnemyController enemyController;

    private float originalMS;

    private float lastAttack;

    private bool isAngry = false;
    private bool isAttacking = false;
    private Attacks currentAttack;
    private GameManager gm;

    private float strayMissileAttackRate = 1.5f;
    private float lastMissileAttack;

    // ABSORPTION PARAMETERS
    [System.Serializable]
    public class AbsorbClass 
    {
        public GameObject absorbEffect;
        public float absorbingTime = 2.0f;
        public float waveSpeed = 8f;
        public float waveDamageDivision = 3.0f;
        public Transform waveReleasePoint;
        public GameObject wavePrefab;
        public AudioSource absorbSFX;
        public AudioSource releaseSFX;
        [HideInInspector]
        public float absorbedDmg = 0.0f;
        [HideInInspector]
        public  bool isAbsorbing = false;    
    }

    // BULLET HELL PARAMETERS
    [System.Serializable]
    public class BulletHellClass 
    {
        public AudioSource shootSFX;

        public float attackSpeed = 0.8f;
        public float bulletDamage = 10.0f;
        public float bulletForce = 10.0f;

        public float bulletSize = 2.0f;

        public int maxBullets = 5;

        public int timesToFire = 10;

        public Transform[] bulletSpawnLocations;
        public GameObject bulletPrefab;

        [HideInInspector]
        public bool bulletHellActive;
    }

    // LASER PARAMETERS
    [System.Serializable]
    public class LaserClass 
    {
        public float laserPower;
        public float chargeDuration;
        public int flashesToAttack;

        public AudioSource ChargeSource;
        public AudioSource FireSource;
        public AudioSource lockedOnSource;

        public GameObject impactEffect;

        public Material laserMat;
        public LineRenderer laser;
        public CapsuleCollider2D capsule;
        [HideInInspector]
        public Vector2 finalPosition;
        [HideInInspector]
        public Vector3 finalDirectionToPlayer;
        [HideInInspector]
        public LayerMask mask;
        [HideInInspector]
        public float laserWidth = 0.25f;
        [HideInInspector]
        public bool isCharging = false;
        [HideInInspector]
        public bool hasFired = false;
    }

    // MISSILE PARAMETERS
    [System.Serializable]
    public class MissilesClass
    {
        public float missileImpactDamage = 20f;
        public float missileSpeed = 15f;
        public float missileFireRate = 0.3f;
        public float missileSpread = 6f;
        public int missilesToSpawnMin = 2;
        public int missilesToSpawnMax = 5;
        public GameObject missilePrefab;

        public AudioSource missileLaunchSource;

        public Transform[] missileFirePoints;
    }

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        originalMS = GetComponent<EnemyController>().movementSpeed;
        currentAttack = Attacks.NoAttack;
        Laser.mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Obstacles");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(enemyController.isDying)
        {
            return;
        }

        if(Laser.hasFired) // This check needs to occur before the death check otherwise if the boss dies with a laser out it doesnt go away
        {
            Laser.laserWidth -= Time.deltaTime*2;
            Laser.laser.startWidth = Mathf.Clamp(Laser.laserWidth, 0.0f, 1.0f);
            float capsuleWidth = Laser.capsule.size.x;
            capsuleWidth -= Time.deltaTime*2;
            Laser.capsule.size = new Vector2(Mathf.Clamp(capsuleWidth, 0.0f, 1.0f), Laser.capsule.size.y);
        }

        if(enemyController.currentHealth <= 0.0f && !enemyController.isDying)
        {
            enemyController.isDying = true;
            StopAllCoroutines();
            gm.PauseAllSFX();
            gm.GetComponents<AudioSource>()[0].Stop();
            cinematicDeath();
            return; 
        }

        if(enemyController.currentHealth < enemyController.maxHealth / 3 && !isAngry)
        {
            isAngry = true;
            activateAngerBuff();
        }

        //WE LOVE THE LASER ATTACK :)
        if(Laser.isCharging)
        {
            drawLaser();
        }
            
        if(isAttacking && !isAngry)
        {
            enemyController.movementSpeed = 0.0f;
        } 
        else
        {
            enemyController.movementSpeed = originalMS;
        }

        Vector3 directionToPlayer = GetComponent<EnemyController>().player.transform.position - transform.position;
        RaycastHit2D laserSeek = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, 400, Laser.mask);
        if(isAngry && !laserSeek.collider.CompareTag("Player") && Time.time > strayMissileAttackRate + lastMissileAttack)
        {
            fireMissile(Missile.missileFirePoints[Random.Range(0, 2)]);
            lastMissileAttack = Time.time;
        }
    
        anim.SetFloat("movementSpeed", enemyController.movementSpeed);
        anim.SetBool("isAngry", isAngry);

        if(!isAttacking && Time.time > attackIntervals + lastAttack)
        {
            setAttack();
            executeAttack();
            if(isAngry)
            {
                Invoke("setAttack", 1.0f);
                Invoke("executeAttack", 1.1f);
            }
        }
    }

    public void activateAngerBuff()
    {
        angryRoar.Play();
        Bullet.attackSpeed -= 0.11f;
        Bullet.timesToFire += 4;
        Bullet.maxBullets += 4;
        Bullet.bulletSize = 1.5f;
        Bullet.bulletDamage += 2f;

        Laser.laserPower += 15;
        Laser.flashesToAttack = 3;
        Laser.chargeDuration = 1.7f;

        Absorb.waveSpeed += 3;
        Absorb.waveDamageDivision = 1.8f;
        
        Missile.missileSpread += 6f;
        Missile.missilesToSpawnMin += 2;
        Missile.missileFireRate -= 0.08f;
        Missile.missileImpactDamage += 2f;

        attackIntervals = 2.1f;
        hurtboxDamage += 50f;
        originalMS += 0.68f;
    }
    
    public void ProcessDamage(float dmg)
    {
        // If we're not in the process of absorbing, take damage like normal
        if(!Absorb.isAbsorbing)
            GetComponent<EnemyController>().TakeDamage(dmg);
        else   //Otherwise add the damage to the amount absorbed and take a sixth of the damage
        {
            Absorb.absorbedDmg += dmg;
            var emi = Absorb.absorbEffect.GetComponent<ParticleSystem>().emission;
            var shape = Absorb.absorbEffect.GetComponent<ParticleSystem>().shape;
            shape.radius = Absorb.absorbedDmg / 30;
            emi.rateOverTime = Absorb.absorbedDmg / 2;
            Absorb.absorbEffect.transform.localScale += new Vector3(0.027f, 0.027f, 0.0f);
            GetComponent<EnemyController>().TakeDamage(dmg/6);
        }   
    }

    private void setAttack()
    {
        while(true)
        {
            float roll = Random.Range(0f, 1f);
            if(roll < 0.25f  && currentAttack != Attacks.Laser)
            {
                currentAttack = Attacks.Laser;
                break;
            }
            else if(roll >= 0.25f && roll < 0.50f && currentAttack != Attacks.Absorb)
            {
                currentAttack = Attacks.Absorb;
                break;
            }
            else if(roll >= 0.50f && roll < 0.75f && currentAttack != Attacks.BulletHell && !Bullet.bulletHellActive)
            {
                currentAttack = Attacks.BulletHell;
                break;
            }
            else if(roll >= 0.75f && roll < 1f && currentAttack != Attacks.Missiles)
            {
                currentAttack = Attacks.Missiles;
                break;
            }
        }
    }

    private void executeAttack()
    {
        isAttacking = true;
        switch(currentAttack)
        {
            case Attacks.Laser:
                laserAttack();
                break;
            case Attacks.Absorb:
                absorbAttack();
                break;
            case Attacks.BulletHell:
                bulletHellAttack();
                break;
            case Attacks.Missiles:
                missileAttack();
                break;
        }
    }

    private void cinematicDeath()
    {
        CancelInvoke();
        Laser.laser.enabled = false;
        Absorb.absorbEffect.SetActive(false);
        enemyController.movementSpeed = 0.0f;
        anim.SetFloat("movementSpeed", enemyController.movementSpeed);
        enemyController.player.GetComponent<PlayerController>().isInvulnerable = true;
        enemyController.player.GetComponent<PlayerController>().enabled = false;
        enemyController.player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        enemyController.player.GetComponent<GunRotating>().enabled = false;
        enemyController.player.GetComponent<MaxReactor>().enabled = false;
        StartCoroutine(BossDeathExplosions());
    }

    IEnumerator BossDeathExplosions()
    {
        deathSound.Play();
        yield return new WaitForSeconds(1.0f);
        Vector2 location = transform.position;
        for(int i = 0; i < numOfExplosions; ++i)
        {
            Vector2 particleSpawn = new Vector2(location.x + Random.Range(-4f, 4f), location.y + Random.Range(-1f, 2f));
            GameObject particle = Instantiate(deathEffect, particleSpawn, Quaternion.identity);
            Destroy(particle, 2.0f);
            gm.StartCameraShake();
            yield return new WaitForSeconds(0.2f);
        }
        Instantiate(finalExplosion, location, Quaternion.identity);
        spriteRender.SetActive(false);
        healthBar.SetActive(false);
        yield return new WaitForSeconds(6.5f);
        enemyController.player.GetComponent<PlayerController>().isInvulnerable = false;
        enemyController.player.GetComponent<PlayerController>().enabled = true;
        enemyController.player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        enemyController.player.GetComponent<GunRotating>().enabled = true;
        enemyController.player.GetComponent<MaxReactor>().enabled = true;
        Destroy(gameObject);
    }

    /*
        ***************************************************
        Missile Attack
        ***************************************************
    */

    private void missileAttack()
    {
        StartCoroutine(BeginMissileAttack());
    }

    IEnumerator BeginMissileAttack()
    {
        yield return new WaitForSeconds(0.1f);
        int missilesToSpawn = Random.Range(Missile.missilesToSpawnMin, Missile.missilesToSpawnMax);
        for(int i = 0; i < missilesToSpawn; ++i)
        {
            foreach(Transform t in Missile.missileFirePoints)
            {
                fireMissile(t);
                yield return new WaitForSeconds(Missile.missileFireRate);
            }
        }
        isAttacking = false;
        lastAttack = Time.time;
    }

    private void fireMissile(Transform source)
    {
        Missile.missileLaunchSource.Play();
        GameObject missile = Instantiate(Missile.missilePrefab, source.position, source.rotation);
        missile.GetComponent<EnemyMissile>().setDamage(Missile.missileImpactDamage);
        Rigidbody2D missilerigid = missile.GetComponent<Rigidbody2D>();
        missile.transform.Rotate(0, 0, Random.Range(-Missile.missileSpread, Missile.missileSpread));
        missilerigid.AddForce(missile.transform.up * Missile.missileSpeed, ForceMode2D.Impulse);
    }

    /*
        ***************************************************
        LASER ATTACK

        pain 👉😎👉
        ***************************************************
    */
        

    private void laserAttack()
    {
        StartCoroutine(BeginLaserCharge());
    }

    IEnumerator BeginLaserCharge()
    {
        
        Laser.laser.enabled = true;
        Laser.isCharging = true;
        Laser.ChargeSource.Play();
        yield return new WaitForSeconds(Laser.chargeDuration);
        Laser.ChargeSource.Stop();
        Laser.lockedOnSource.Play();
        Laser.isCharging = false;
        enemyController.canRotate = false;
        for(int i = 0; i < Laser.flashesToAttack; ++i)
        {
            yield return new WaitForSeconds(0.05f);
            Laser.laser.enabled = false;
            yield return new WaitForSeconds(0.05f);
            Laser.laser.enabled = true;
        }
        Laser.lockedOnSource.Stop();
        fireLaser();
        Laser.FireSource.Play();
        enemyController.canRotate = true;
        lastAttack = Time.time;
        yield return new WaitForSeconds(0.8f);
        Laser.capsule.enabled = false;
        Laser.hasFired = false;
        Laser.laserWidth = 0.25f;
        isAttacking = false;
    }

    private void drawLaser()
    {
        Laser.laserMat.color = new Color(1, 0, 0, 0.25f);
        Laser.laser.startWidth = Laser.laserWidth;
        Laser.laserWidth += Time.deltaTime/6;

        Vector3 directionToPlayer = GetComponent<EnemyController>().player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, 400, Laser.mask);
        Laser.laser.SetPosition(0, Laser.laser.transform.position);
        Laser.laser.SetPosition(1, hit.point);
        Laser.finalDirectionToPlayer = directionToPlayer;
        Laser.finalPosition = hit.point;
    }

    private void fireLaser()
    {
        Laser.hasFired = true;
        
        Vector3 directionToPlayer = Laser.finalDirectionToPlayer;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, 400, Laser.mask);
        Laser.laser.SetPosition(1, hit.point);
        Laser.laserMat.color = new Color(1, 0, 0, 1f);
        Laser.laser.startWidth = 1.5f;

        Laser.capsule.transform.position = (hit.point + new Vector2(transform.position.x, transform.position.y)) / 2;
        Laser.capsule.size = new Vector2(0.5f, hit.distance + 3.0f);
        float angle = Mathf.Atan2(transform.position.y - hit.point.y, transform.position.x - hit.point.x) *180/Mathf.PI - 90f;
        Laser.capsule.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        Laser.capsule.enabled = true;

        Debug.Log(angle);
        GameObject particleEffect = Instantiate(Laser.impactEffect, hit.point, Quaternion.identity);
        particleEffect.transform.Rotate(new Vector3(angle-90f, -90f, 0.0f));
        Destroy(particleEffect, 1.5f);
        gm.StartCameraShake();
    }

    /*
        ***************************************************
        ABSORPTION ATTACK
        ***************************************************
    */

    private void absorbAttack()
    {
        StartCoroutine(BeginAbsorb());
    }

    IEnumerator BeginAbsorb()
    {
        Absorb.absorbEffect.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Absorb.isAbsorbing = true;
        Absorb.absorbEffect.SetActive(true);
        Absorb.absorbSFX.Play();

        yield return new WaitForSeconds(Absorb.absorbingTime);

        Absorb.absorbSFX.Stop();
        lastAttack = Time.time;
        Absorb.absorbEffect.SetActive(false);
        Absorb.isAbsorbing = false;
        ReleaseWaveAttack(Absorb.absorbedDmg);
        isAttacking = false;
    }

    private void ReleaseWaveAttack(float totalDmg)
    {
        if(isAngry && totalDmg < 5.0f)
        {
            totalDmg += 5f;
        }
        if(totalDmg != 0.0f)
        {
            Absorb.releaseSFX.Play();
            GameObject wave = Instantiate(Absorb.wavePrefab, Absorb.waveReleasePoint.position, Absorb.waveReleasePoint.rotation);
            wave.GetComponent<EnemyWave>().setDamage(totalDmg / Absorb.waveDamageDivision);
            if(isAngry)
                wave.GetComponent<EnemyWave>().willIgnoreHitstop = true;
            Rigidbody2D bulletrigid = wave.GetComponent<Rigidbody2D>();
            bulletrigid.AddForce(Absorb.waveReleasePoint.up * Absorb.waveSpeed, ForceMode2D.Impulse);
            Absorb.absorbedDmg = 0.0f;
        }
    }

    /*
        ***************************************************
        BULLET HELL ATTACK
        ***************************************************
    */

    private void bulletHellAttack()
    {
        StartCoroutine(BulletHellBegin());
    }

    IEnumerator BulletHellBegin()
    {
        Bullet.bulletHellActive = true;
        for(int i = 0; i < Bullet.timesToFire; ++i)
        {
            int amount = Random.Range(1, Bullet.maxBullets);
            sprayBullets(amount);
            Bullet.shootSFX.Play();
            yield return new WaitForSeconds(Bullet.attackSpeed);
        }
        isAttacking = false;
        lastAttack = Time.time;
        Bullet.bulletHellActive = false;
    }

    private void sprayBullets(int amount)
    {
        foreach(Transform spawn in Bullet.bulletSpawnLocations)
        {
            for(int i = 0; i < amount; ++i)
            {
                GameObject b = Instantiate(Bullet.bulletPrefab, spawn.transform.position, spawn.rotation);
                b.GetComponent<EnemyBullet>().setBulletDamage(Bullet.bulletDamage);
                b.transform.localScale *= Bullet.bulletSize;
                Rigidbody2D bulletrigid = b.GetComponent<Rigidbody2D>();
                b.transform.Rotate(0, 0, Random.Range(-50, 50));
                bulletrigid.AddForce(b.transform.up * Bullet.bulletForce, ForceMode2D.Impulse);
            }
        }
    }

    public enum Attacks
    {
        NoAttack,
        Laser,
        Absorb,
        BulletHell,
        Missiles
    }
}
