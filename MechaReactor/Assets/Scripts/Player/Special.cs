using UnityEngine;

public class Special : MonoBehaviour
{
    private GameObject playerPrefab;
    [Range(0.01f, 0.05f)]
    public float changeRate = 0.05f;
    public float damage = 100.0f;
    private float maxSize = 5.0f;

    void Start()
    {
        playerPrefab = GameObject.Find("Player");
        transform.localScale = new Vector2(1, 1);
    }

    void Update()
    {
        if (transform.localScale.x < maxSize)
        {
            transform.localScale = new Vector2(transform.localScale.x + changeRate,
                                               transform.localScale.y + changeRate);
        }
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            other.transform.GetComponent<EnemyController>().TakeDamage(damage * 
                playerPrefab.GetComponent<ReactorAttributes>()["special"].GetValue());
        }
        else if (other.transform.CompareTag("Boss"))
        {
            other.transform.parent.GetComponent<EnemyController>().TakeDamage(damage * 
                playerPrefab.GetComponent<ReactorAttributes>()["special"].GetValue());
        }
        else if(other.transform.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
