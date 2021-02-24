using UnityEngine;

public class Special : MonoBehaviour
{
    public GameObject playerPrefab;
    [Range(0.05f, 0.1f)]
    public float changeRate = 0.05f;
    public float damage;
    private Vector2 scaleRatio;

    void Start()
    {
        transform.localScale = new Vector2(1, 1);
        scaleRatio = transform.localToWorldMatrix * transform.localScale;
        scaleRatio = transform.GetChild(0).transform.worldToLocalMatrix * new Vector2(scaleRatio.x, scaleRatio.y);
    }

    void Update()
    {
        if (transform.localScale.x < 15)
        {
            transform.localScale = new Vector2(transform.localScale.x + changeRate,
                                               transform.localScale.y + changeRate);
        }
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print(other.transform.tag);
        if (other.transform.CompareTag("Enemy"))
            other.transform.GetComponent<EnemyController>().TakeDamage(damage);
    }
}
