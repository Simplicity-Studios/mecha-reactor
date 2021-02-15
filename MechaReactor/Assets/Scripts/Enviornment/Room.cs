using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public GameObject exitTrigger;
    public GameObject door;
    public Transform spawnLocation;

    private BoxCollider2D exitCollisionTrigger;

    public RoomManager roomManager;

    //These have to be the same size
    //I would've used a tuple if I could've gotten it to work :(
    public GameObject[] enemiesToSpawn;
    public Transform[] locationsToSpawn;

    void Start()
    {
        if(enemiesToSpawn.Length != locationsToSpawn.Length)
            Debug.LogError("enemiesToSpawn and locationsToSpawn are not the same length.");
    }

    public void goToNextRoom()
    {
        roomManager.nextRoom();
    }
}