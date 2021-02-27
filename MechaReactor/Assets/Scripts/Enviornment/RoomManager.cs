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
        rooms[currentRoom].GetComponent<Room>().InitializeWithEnemies();
        gameManager.setPlayerTransform(rooms[currentRoom].GetComponent<Room>().spawnLocation);
        gameManager.moveCameraToPosition(gameManager.player.GetComponent<Transform>());
        gameManager.setCameraSize(rooms[currentRoom].GetComponent<Room>().cameraDistance);
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

        gameManager.moveCameraToPosition(gameManager.player.GetComponent<Transform>());
        gameManager.setCameraSize(rooms[currentRoom].GetComponent<Room>().cameraDistance);

        rooms[currentRoom].SetActive(true);
        rooms[currentRoom].GetComponent<Room>().InitializeWithEnemies();
    }

    public void goBack()
    {
        rooms[currentRoom].SetActive(false);
        if(currentRoom > 0)
        {
            --currentRoom;
        }
        //Tell the game manager to set the player's position to the next room's spawn point
        gameManager.setPlayerTransform(rooms[currentRoom].GetComponent<Room>().spawnLocation);
        gameManager.moveCameraToPosition(gameManager.player.GetComponent<Transform>());
        gameManager.setCameraSize(rooms[currentRoom].GetComponent<Room>().cameraDistance);

        rooms[currentRoom].SetActive(true);
        rooms[currentRoom].GetComponent<Room>().InitializeWithoutEnemies();
    }
}
