using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbsorber : MonoBehaviour
{
    public float absorbingTime = 3.0f;

    public float waveSpeed = 10f;
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
    public SpriteRenderer renderer;

    private Sprite defaultSprite;

    private float defaultMovementSpeed;


    void Start()
    {
        defaultSprite = renderer.sprite;
        defaultMovementSpeed = GetComponent<EnemyController>().movementSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, GetComponent<EnemyController>().player.transform.position);
        if(Time.time > (lastAbsorbTime + absorbCooldown) && !isAbsorbing && !GetComponent<EnemyController>().isDying && distanceToPlayer <= 6.5f)
        {
            StartCoroutine(BeginAbsorb());
        }
    }

    public void ProcessDamage(float dmg)
    {
        // If we're not in the process of absorbing, take damage like normal
        if(!isAbsorbing)
            GetComponent<EnemyController>().TakeDamage(dmg);
        else   //Otherwise add the damage to the amount absorbed and take a sixth of the damage
        {
            absorbedDmg += dmg;
            var emi = absorbParticle.GetComponent<ParticleSystem>().emission;
            var shape = absorbParticle.GetComponent<ParticleSystem>().shape;
            shape.radius = absorbedDmg / 40;
            emi.rateOverTime = absorbedDmg / 4;
            absorbParticle.transform.localScale += new Vector3(0.027f, 0.027f, 0.0f);
            GetComponent<EnemyController>().TakeDamage(dmg/6);
        } 
            
    }   

    public void ReleaseWaveAttack(float totalDmg)
    {
        if(totalDmg != 0.0f)
        {
            releaseSFX.Play();
            GameObject wave = Instantiate(waveAttack, firePoint.position, firePoint.rotation);
            wave.GetComponent<EnemyWave>().setDamage(totalDmg);
            Rigidbody2D bulletrigid = wave.GetComponent<Rigidbody2D>();
            bulletrigid.AddForce(firePoint.up * waveSpeed, ForceMode2D.Impulse);
            absorbedDmg = 0.0f;
        }
    }

    IEnumerator BeginAbsorb()
    {
        isAbsorbing = true;
        absorbParticle.SetActive(true);
        absorbSFX.Play();
        renderer.sprite = absorbingSprite;
        GetComponent<EnemyController>().movementSpeed = 0.0f;

        yield return new WaitForSeconds(absorbingTime);

        absorbSFX.Stop();
        renderer.sprite = defaultSprite;
        GetComponent<EnemyController>().movementSpeed = defaultMovementSpeed;
        lastAbsorbTime = Time.time;
        absorbParticle.SetActive(false);
        absorbParticle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        isAbsorbing = false;
        var emi = absorbParticle.GetComponent<ParticleSystem>().emission;
        var shape = absorbParticle.GetComponent<ParticleSystem>().shape;
        emi.rateOverTime = 5.0f;
        shape.radius = 0.81f;
        ReleaseWaveAttack(absorbedDmg);
    }
}
