using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    /*
        This script purely exists for the exit trigger to tell it's "room" parent
        to go to the next room. 

        I wouldn't put any additional logic around exiting a room into this script
    */
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
