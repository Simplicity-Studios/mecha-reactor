using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
    [Range(0.05f, 0.1f)]
    public float changeRate = 0.05f;
    public float duration;
    public float maxSize = 30f;
    private Vector2 scaleRatio;

    void Start()
    {
        transform.localScale = new Vector2(1, 1);
        scaleRatio = transform.localToWorldMatrix * transform.localScale;
        scaleRatio = transform.GetChild(0).transform.worldToLocalMatrix * new Vector2(scaleRatio.x, scaleRatio.y);
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
        if(other.transform.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().HitByEMP(duration);
        }
    }
}
