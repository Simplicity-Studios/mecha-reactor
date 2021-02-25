using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEMPBot : MonoBehaviour
{
    public GameObject emp;
    public float empActivationRate;
    private float lastEMP;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > (lastEMP + empActivationRate))
        {
            activateEMP();
            lastEMP = Time.time;
        }
    }

    void activateEMP()
    {
        Instantiate(emp, transform.position, Quaternion.identity);
    }
}
