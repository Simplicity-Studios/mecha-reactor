using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEMPBot : MonoBehaviour
{
    public GameObject emp;
    public float empActivationRate;
    private float lastEMP;

    public float damage;

    public AudioSource empSFX;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > (lastEMP + empActivationRate) && !GetComponent<EnemyController>().isDying)
        {
            activateEMP();
            lastEMP = Time.time;
        }
    }

    void activateEMP()
    {
        empSFX.Play();
        GameObject blast = Instantiate(emp, transform.position, Quaternion.identity);
        blast.GetComponent<EMP>().damage = damage;
    }
}
