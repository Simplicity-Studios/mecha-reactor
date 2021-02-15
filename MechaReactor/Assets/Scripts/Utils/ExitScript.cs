using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private Room roomParent;

    void Start()
    {
        roomParent = transform.parent.parent.GetComponent<Room>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            roomParent.goToNextRoom();
        }
    }
}
