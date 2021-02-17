using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] rooms;
    public int currentRoom;

    public GameManager gameManager;

    void Start()
    {
        currentRoom = 0;
        rooms[currentRoom].SetActive(true);
        gameManager.setPlayerTransform(rooms[currentRoom].GetComponent<Room>().spawnLocation);
    }

    public void nextRoom()
    {
        rooms[currentRoom].SetActive(false);
        ++currentRoom;
        //we reached the last room
        if(currentRoom >= rooms.Length)
        {
            gameManager.quitGame();
        }
        //Tell the game manager to set the player's position to the next room's spawn point
        gameManager.setPlayerTransform(rooms[currentRoom].GetComponent<Room>().spawnLocation);
        rooms[currentRoom].SetActive(true);
    }
}
