using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbsorber : MonoBehaviour
{
    public float absorbingTime = 3.0f;

    public float absorbCooldown;
    private float lastAbsorbTime;

    private float absorbedDmg = 0.0f;
    
    public Sprite absorbingSprite;
    public GameObject absorbParticle;

    public AudioSource absorbSFX;
    public AudioSource releaseSFX;

    public GameObject waveAttack;
    public Transform firePoint;

    private bool isAbsorbing = false;
    private Sprite defaultSprite;

    private float defaultMovementSpeed;


    void Start()
    {
        defaultSprite = GameObject.Find("EnemySprite").GetComponent<SpriteRenderer>().sprite;
        defaultMovementSpeed = GetComponent<EnemyController>().movementSpeed;
    }

    void Update()
    {
        if(Time.time > (lastAbsorbTime + absorbCooldown) && !isAbsorbing)
        {
            StartCoroutine(BeginAbsorb());
        }
    }

    public void ProcessDamage(float dmg)
    {
        // If we're not in the process of absorbing, take damage like normal
        if(!isAbsorbing)
            GetComponent<EnemyController>().TakeDamage(dmg);
        else //Otherwise add the damage to the amount absorbed
            absorbedDmg += dmg;
    }   

    public void ReleaseWaveAttack(float totalDmg)
    {
        GameObject wave = Instantiate(waveAttack, firePoint.position, firePoint.rotation);
        wave.GetComponent<EnemyWave>().setDamage(totalDmg);
        Rigidbody2D bulletrigid = wave.GetComponent<Rigidbody2D>();
        bulletrigid.AddForce(firePoint.up * 10f, ForceMode2D.Impulse);
        absorbedDmg = 0.0f;
    }

    IEnumerator BeginAbsorb()
    {
        isAbsorbing = true;
        absorbParticle.SetActive(true);
        absorbSFX.Play();
        GameObject.Find("EnemySprite").GetComponent<SpriteRenderer>().sprite = absorbingSprite;
        GetComponent<EnemyController>().movementSpeed = 0.0f;

        yield return new WaitForSeconds(absorbingTime);

        absorbSFX.Stop();
        GameObject.Find("EnemySprite").GetComponent<SpriteRenderer>().sprite = defaultSprite;
        GetComponent<EnemyController>().movementSpeed = defaultMovementSpeed;
        lastAbsorbTime = Time.time;
        absorbParticle.SetActive(false);
        isAbsorbing = false;
        releaseSFX.Play();
        ReleaseWaveAttack(absorbedDmg);
    }
}
