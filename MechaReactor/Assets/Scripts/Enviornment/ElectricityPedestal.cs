using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityPedestal : MonoBehaviour
{
    public float dropRate;
    public GameObject electricityPrefab;
    public Sprite NoElectricitySprite;
    private Sprite ElectricitySprite;
    private float timePickedUp;
    private bool isEmpty;

    void Start()
    {   
        ElectricitySprite = GetComponent<SpriteRenderer>().sprite;
        SpawnElectricity();
    }

    void Update()
    {
        if(this.transform.childCount == 0 && !isEmpty)
        {
            timePickedUp = Time.time;
            isEmpty = true;
            GetComponent<SpriteRenderer>().sprite = NoElectricitySprite;
        }
        if(Time.time > timePickedUp + dropRate && isEmpty)
        {
            SpawnElectricity();
        }
    }

    void SpawnElectricity()
    {
        GameObject powerup = Instantiate(electricityPrefab, new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z), Quaternion.identity);
        powerup.transform.SetParent(this.transform);
        isEmpty = false;
        GetComponent<SpriteRenderer>().sprite = ElectricitySprite;
    }
}
