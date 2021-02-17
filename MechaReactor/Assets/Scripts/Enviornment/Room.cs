using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // The actual trigger that causes the next room to be active
    public GameObject exitTrigger;
    // The game object with a collider that prevents the player from hitting the exit trigger
    public GameObject door;
    // The location the player should spawn when the room becomes active
    public Transform spawnLocation;

    private BoxCollider2D exitCollisionTrigger;

    public RoomManager roomManager;

    //These have to be the same size
    //I would've used a tuple if I could've gotten it to work :(
    public GameObject[] enemiesToSpawn;
    public Transform[] locationsToSpawn;

    // The object that holds all enemies
    private Transform currentEnemies;


    void Start()
    {
        currentEnemies = this.transform.Find("CurrentEnemies");
        if(enemiesToSpawn.Length != locationsToSpawn.Length)
            Debug.LogError("enemiesToSpawn and locationsToSpawn are not the same length.");
        spawnEnemies();
    }

    void Update()
    {
        // If the amount of children of currentEnemies goes to zero, all the enemies are dead
        if(currentEnemies.childCount == 0) 
        {
            openDoor();
        }
    }

    // Spawns all the enemies in the enemiesToSpawn array and adds them to the list
    void spawnEnemies()
    {
        for(int i = 0; i < enemiesToSpawn.Length; ++i)
        {
            GameObject enemy = Instantiate(enemiesToSpawn[i], new Vector3(locationsToSpawn[i].position.x, locationsToSpawn[i].position.y, 0.0f), Quaternion.identity);
            //Enemies are held in the currentEnemies transform within a room.
            enemy.transform.SetParent(currentEnemies);
        }
    }

    public void goToNextRoom()
    {
        roomManager.nextRoom();
    }

    private void openDoor()
    {
        door.SetActive(false);
    }
}