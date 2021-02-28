using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ReactorAttributes stats;

    //MOVEMENT
    private float horizontal;
    private float vertical;
    private Rigidbody2D r;
    private Vector2 moveVelocity;

    //ANIMTOR
    private Animator anim;

    //SHOOTING
    [Header( "Shooting" )]
    [Range(0.0f, 2.0f)]
    public float baseAttackTime;
    public float bulletForce;
    public Transform bulletSpawnLocationLeft;
    public Transform bulletSpawnLocationRight;
    public GameObject bulletPrefab;
    public AudioSource shootSound;

    [Header("Special Attack")]
    public GameObject specialPrefab;
    public float baseSpecialTime;
    private float lastShot = 0.0f;
    private float lastSpecialUse = 0.0f;

    [Header("Other")]
    public float maxHealth = 300.0f;
    private float currHealth;
    public float invincibilityDuration;
    public float invicinibilityDelta;
    private GameManager gameManager;
    public AudioSource damageSound;
    public AudioSource EMPSound;
    
    [HideInInspector]
    public bool isImmuneToEMP = false;
    private bool lastFiredLeft = false;
    private bool lastFiredRight = true;
    private bool isInvulnerable = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stats = this.GetComponent<ReactorAttributes>();
        r = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = stats["movementSpeed"].GetValue() * moveInput.normalized * 3.0f;

        //Animation
        anim.SetBool("isMoving", moveInput != Vector2.zero);
        anim.SetFloat("walkMultiplier", stats["movementSpeed"].GetValue());

        //Look at mouse
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Input for Shooting
        if (!MouseOverReactorButton() && Input.GetButton("Fire1")  && Time.time > (baseAttackTime - stats["attackSpeed"].GetValue()) + lastShot)
        {
            
            if(lastFiredLeft)
            {
                Shoot(bulletSpawnLocationRight);
                lastFiredLeft = false;
                lastFiredRight = true;
            }
            else if(lastFiredRight)
            {
                Shoot(bulletSpawnLocationLeft);
                lastFiredRight = false;
                lastFiredLeft = true;
            }
            lastShot = Time.time;
        }

        // Input for Special attack
        if (Input.GetButton("Fire2") && Time.time > baseSpecialTime + lastSpecialUse)
        {
            UseSpecial();
            lastSpecialUse = Time.time;
        }
    }

    void FixedUpdate()
    {
        r.MovePosition(r.position + moveVelocity * Time.fixedDeltaTime);
    }

    void Shoot(Transform bulletSpawnLocation)
    {
        shootSound.pitch = (Random.Range(0.9f, 1.1f));
        shootSound.Play();
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
        bullet.GetComponent<Bullet>().setBulletDamage(stats["attack"].GetValue());
        
        Rigidbody2D bulletrigid = bullet.GetComponent<Rigidbody2D>();
        bulletrigid.AddForce(bulletSpawnLocation.up * bulletForce, ForceMode2D.Impulse);
    }

    void UseSpecial()
    {
        GameObject shockwave = Instantiate(specialPrefab, transform.position, Quaternion.identity);
    }

    public float GetHealth()
    {
        return currHealth;
    }

    public void setHealth(float health)
    {
        currHealth = health;
    }

    public void AddHealth(float health)
    {
        currHealth = Mathf.Min(currHealth + health, maxHealth);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if(!isInvulnerable)
        {
            currHealth = Mathf.Max(currHealth - (damage / stats["defense"].GetValue()), 0);
            StartCoroutine("DamageFlash");
            gameManager.StartCameraShake();
            damageSound.Play();
        }
    }

    public void TakeDamageIgnoreHitstop(float damage)
    {
        currHealth = Mathf.Max(currHealth - (damage / stats["defense"].GetValue()), 0);
        gameManager.StartCameraShake();
        damageSound.Play();
    }

    public void HitByEMP(float duration)
    {
        if(!isImmuneToEMP && stats.GetElectricity() > 0)
        {
            string reactorToDisable = selectRandomReactor();
            stats[reactorToDisable].Disable();
            EMPSound.Play();
            StartCoroutine(RecoverFromEMP(duration, reactorToDisable));
        }
        else
        {
            Debug.Log("plink");
        }
    }

    IEnumerator RecoverFromEMP(float duration, string disabledReactor)
    {
        yield return new WaitForSeconds(duration);
        if (stats.GetElectricity() > 0)
            stats[disabledReactor].Enable();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthItem"))
            currHealth = Mathf.Min(currHealth + maxHealth * 0.10f, maxHealth);
        else if (other.CompareTag("ElectricityItem"))
            stats.AddElectricity(stats.GetMaxElectricity() * 0.10f);
        else
            return;
        
        Destroy(other.gameObject);
    }

    public string selectRandomReactor()
    {
        int rand = Random.Range(0, 5);
        string reactorToDisable = "";
        switch(rand)
        {
            case 0:
                reactorToDisable = "attack";
                break;
            case 1:
                reactorToDisable = "defense";
                break;
            case 2:
                reactorToDisable = "movementSpeed";
                break;
            case 3:
                reactorToDisable = "attackSpeed";
                break;
            case 4:
                reactorToDisable = "special";
                break;
            default:
                reactorToDisable = "attack";
                break;
        }

        return reactorToDisable;
    }

    public bool MouseOverReactorButton()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        foreach (RaycastResult r in results)
        {
            if (r.gameObject.CompareTag("ReactorButton"))
                return true;
        }
        return false;
    }

    IEnumerator DamageFlash()
    {
        isInvulnerable = true;
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        Color normal = new Color(1, 1, 1, 1);
        Color transparent = new Color(1, 1, 1, 0);
        for(float i = 0; i < invincibilityDuration; i += invicinibilityDelta)
        {
            s.color = transparent;
            yield return new WaitForSeconds(invicinibilityDelta / 2);
            s.color = normal;
            yield return new WaitForSeconds(invicinibilityDelta / 2);
        }
        isInvulnerable = false;
    }
}
