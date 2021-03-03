using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public AbsorbClass Absorb;
    public BulletHellClass Bullet;

    public float attackIntervals;
 
    public Animator anim;

    public GameObject laserLeft;
    public GameObject laserRight;

    private EnemyController enemyController;

    private float originalMS;

    private float lastAttack;

    private bool isAngry = false;
    private bool isAttacking = false;
    private Attacks currentAttack;

    // ABSORPTION PARAMETERS
    [System.Serializable]
    public class AbsorbClass 
    {
        public GameObject absorbEffect;
        public float absorbingTime = 2.0f;
        public float waveSpeed = 8f;
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

        public int maxBullets = 5;

        public int timesToFire = 10;

        public Transform[] bulletSpawnLocations;
        public GameObject bulletPrefab;
    }

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        originalMS = GetComponent<EnemyController>().movementSpeed;
        currentAttack = Attacks.NoAttack;
    }

    void Update()
    {
        if(enemyController.currentHealth < enemyController.maxHealth / 2 && !isAngry)
        {
            isAngry = true;
            activateAngerBuff();
        }
            

        if(isAttacking)
        {
            enemyController.movementSpeed = 0.0f;
        } 
        else
        {
            enemyController.movementSpeed = originalMS;
        }
    
        anim.SetFloat("movementSpeed", enemyController.movementSpeed);
        anim.SetBool("isAngry", isAngry);

        if(!isAttacking && Time.time > attackIntervals + lastAttack)
        {
            setAttack();
            executeAttack();
        }
    }

    public void activateAngerBuff()
    {
        Bullet.attackSpeed -= 0.15f;
        Bullet.timesToFire += 5;
        Bullet.bulletForce += 2.0f;
        Bullet.maxBullets += 2;
    }
    
    public void ProcessDamage(float dmg)
    {
        // If we're not in the process of absorbing, take damage like normal
        if(!Absorb.isAbsorbing)
            GetComponent<EnemyController>().TakeDamage(dmg);
        else   //Otherwise add the damage to the amount absorbed and take a sixth of the damage
        {
            Absorb.absorbedDmg += dmg;
            GetComponent<EnemyController>().TakeDamage(dmg/6);
        }   
    }

    private void setAttack()
    {
        float roll = 0.55f;
        if(roll < 0.25f)
        {
            currentAttack = Attacks.Laser;
        }
        else if(roll >= 0.25f && roll < 0.50f)
        {
            currentAttack = Attacks.Absorb;
        }
        else if(roll >= 0.50f && roll < 0.75f)
        {
            currentAttack = Attacks.BulletHell;
        }
        else
        {
            currentAttack = Attacks.Missiles;
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

    private void laserAttack()
    {

    }

    private void absorbAttack()
    {
        StartCoroutine(BeginAbsorb());
    }

    IEnumerator BeginAbsorb()
    {
        Absorb.isAbsorbing = true;
        Absorb.absorbEffect.SetActive(true);
        Absorb.absorbSFX.Play();
        enemyController.movementSpeed = 0.0f;

        yield return new WaitForSeconds(Absorb.absorbingTime);

        Absorb.absorbSFX.Stop();
        enemyController.movementSpeed = originalMS;
        lastAttack = Time.time;
        Absorb.absorbEffect.SetActive(false);
        Absorb.isAbsorbing = false;
        ReleaseWaveAttack(Absorb.absorbedDmg);
        isAttacking = false;
    }

    private void ReleaseWaveAttack(float totalDmg)
    {
        if(totalDmg != 0.0f)
        {
            Absorb.releaseSFX.Play();
            GameObject wave = Instantiate(Absorb.wavePrefab, Absorb.waveReleasePoint.position, Absorb.waveReleasePoint.rotation);
            wave.GetComponent<EnemyWave>().setDamage(totalDmg / 2);
            Rigidbody2D bulletrigid = wave.GetComponent<Rigidbody2D>();
            bulletrigid.AddForce(Absorb.waveReleasePoint.up * Absorb.waveSpeed, ForceMode2D.Impulse);
            Absorb.absorbedDmg = 0.0f;
        }
    }

    private void bulletHellAttack()
    {
        StartCoroutine(BulletHellBegin());
    }

    IEnumerator BulletHellBegin()
    {
        enemyController.movementSpeed = 0.0f;
        for(int i = 0; i < Bullet.timesToFire; ++i)
        {
            int amount = Random.Range(1, Bullet.maxBullets);
            sprayBullets(amount);
            Bullet.shootSFX.Play();
            yield return new WaitForSeconds(Bullet.attackSpeed);
        }
        enemyController.movementSpeed = originalMS;
        isAttacking = false;
        lastAttack = Time.time;
    }

    private void sprayBullets(int amount)
    {
        foreach(Transform spawn in Bullet.bulletSpawnLocations)
        {
            for(int i = 0; i < amount; ++i)
            {
                GameObject b = Instantiate(Bullet.bulletPrefab, spawn.transform.position, spawn.rotation);
                b.GetComponent<EnemyBullet>().setBulletDamage(Bullet.bulletDamage);
                b.transform.localScale *= 2;
                Rigidbody2D bulletrigid = b.GetComponent<Rigidbody2D>();
                b.transform.Rotate(0, 0, Random.Range(-50, 50));
                bulletrigid.AddForce(b.transform.up * Bullet.bulletForce, ForceMode2D.Impulse);
            }
        }
    }

    private void missileAttack()
    {

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
