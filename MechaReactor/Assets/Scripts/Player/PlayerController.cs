using System.Collections.Generic;
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

    [Header("Special Attack")]
    public GameObject specialPrefab;
    public float baseSpecialTime;
    private float lastShot = 0.0f;
    private float lastSpecialUse = 0.0f;

    [Header("Other")]
    public float maxHealth = 300.0f;
    private float currHealth;
    
    [HideInInspector]
    public bool isImmuneToEMP = false;
    private bool lastFiredLeft = false;
    private bool lastFiredRight = true;

    void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(10f);
    }

    void FixedUpdate()
    {
        r.MovePosition(r.position + moveVelocity * Time.fixedDeltaTime);
    }

    void Shoot(Transform bulletSpawnLocation)
    {
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currHealth = Mathf.Max(currHealth - (damage - stats["defense"].GetValue()), 0);
    }

    public void HitByEMP(float duration)
    {
        string reactorToDisable = selectRandomReactor();
        int currentlyAllocated = stats[reactorToDisable].pointsAllocated;
        stats[reactorToDisable].pointsAllocated -= currentlyAllocated;
        stats[reactorToDisable].Disable();
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
}
