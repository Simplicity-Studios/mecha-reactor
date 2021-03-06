using UnityEngine;
using System.IO;

public class RoomManager : MonoBehaviour
{
    public GameObject[] rooms;
    public int currentRoom;

    public GameManager gameManager;
    public AudioClip doorOpening;
    private AudioSource audio;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = doorOpening;

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
        cleanUpPickups();
        //we reached the last room
        if(currentRoom >= rooms.Length)
        {
            File.WriteAllText("savegame.txt", gameManager.timeValue.ToString());
            gameManager.quitGame();
        }
        //Tell the game manager to set the player's position to the next room's spawn point
        gameManager.setPlayerTransform(rooms[currentRoom].GetComponent<Room>().spawnLocation);

        gameManager.moveCameraToPosition(gameManager.player.GetComponent<Transform>());
        gameManager.setCameraSize(rooms[currentRoom].GetComponent<Room>().cameraDistance);

        rooms[currentRoom].SetActive(true);
        rooms[currentRoom].GetComponent<Room>().InitializeWithEnemies();
        rooms[currentRoom].GetComponent<Room>().InitializeBossRoomHazards();
    }

    public void goBack()
    {
        rooms[currentRoom].SetActive(false);
        // Kill all the enemies so theres not double enemies when you come back from previous room.
        foreach(Transform child in rooms[currentRoom].transform.Find("CurrentEnemies"))
        {
            Destroy(child.gameObject);
        }

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

    public void resetCurrentRoom()
    {
        foreach(Transform child in rooms[currentRoom].transform.Find("CurrentEnemies"))
        {
            Destroy(child.gameObject);
        }
        cleanUpPickups();

        gameManager.player.GetComponent<PlayerController>().isInvulnerable = false;

        gameManager.setPlayerTransform(rooms[currentRoom].GetComponent<Room>().spawnLocation);
        gameManager.moveCameraToPosition(gameManager.player.GetComponent<Transform>());
        gameManager.setCameraSize(rooms[currentRoom].GetComponent<Room>().cameraDistance);

        rooms[currentRoom].GetComponent<Room>().InitializeWithEnemies();
        rooms[currentRoom].GetComponent<Room>().InitializeBossRoomHazards();
        
        if (gameManager.isPaused)
            gameManager.Unpause();
    }

    public void playDoorOpeningSFX()
    {
        gameManager.StartCameraShake();
        audio.Play();
    }

    public void cleanUpPickups()
    {
        GameObject[] electricityLeftovers = GameObject.FindGameObjectsWithTag("ElectricityItem");
        GameObject[] healthLeftovers = GameObject.FindGameObjectsWithTag("HealthItem");
        if(electricityLeftovers.Length > 0)
        {
            foreach(GameObject e in electricityLeftovers)
                Destroy(e);
        }
        if(healthLeftovers.Length > 0)
        {
            foreach(GameObject h in healthLeftovers)
                Destroy(h); 
        }
    }

    public int getNumberOfEnemiesLeft()
    {
        Room room = rooms[currentRoom].GetComponent<Room>(); 
        return room.currentEnemies.childCount; 
    }
}
