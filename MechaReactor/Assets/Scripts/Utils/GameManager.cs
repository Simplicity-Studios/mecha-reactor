﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public GameObject mainCamera;

    public GameObject playerDeathEffect;

    public GameObject pauseButtons;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private PlayerController playerController;
    private RoomManager roomManager;
    private bool isDying = false;
    [HideInInspector]
    public bool isPaused = false;

    public GameObject escape_menu_sprite;

    private AudioSource loopingMusic;
    private float loopingMusicVolume;
    
    public AudioSource allocSound, deallocSound;

    private AudioSource[] allAudioSources;

    [HideInInspector]
    public float timeValue;

    public GameObject deathSplash;

    void Start()
    {
        deallocSound.playOnAwake = false;
        allocSound.playOnAwake = false;
        loopingMusic = GetComponent<AudioSource>();
        playerController = player.GetComponent<PlayerController>();
        roomManager = FindObjectOfType<RoomManager>();
        //Bullets and missiles
        Physics2D.IgnoreLayerCollision(11, 12, true);
        //Bullets and obstacles-that-dont-collide
        Physics2D.IgnoreLayerCollision(10, 11, true);
        //Missiles and obstacles-that-dont-collide
        Physics2D.IgnoreLayerCollision(10, 12, true);
        loopingMusic.Play();
        loopingMusicVolume = loopingMusic.volume;
        timeValue = 0;
    }

    void Update()
    {
        if(playerController.GetHealth() <= 0.0f && !isDying)
        {
            killPlayer();
        } 

        if(!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            return;
        }

        if(isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Unpause();
            return;
        }

        int deltaPoints = Input.GetKey(KeyCode.LeftShift) ? -1 : 1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddPoints("movementSpeed", deltaPoints);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddPoints("attack", deltaPoints);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            AddPoints("defense", deltaPoints);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            AddPoints("attackSpeed", deltaPoints);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            AddPoints("special", deltaPoints);
    }

    void FixedUpdate()
    {
        timeValue += Time.fixedDeltaTime;
    }

    public void AddPoints(string statName, int points)
    {
        int pointsRemaining = player.GetComponent<ReactorAttributes>().GetMaxPoints() - 
                              player.GetComponent<ReactorAttributes>().GetTotalPointsAllocated() - 1;
        if (points > 0 && pointsRemaining > 0)
        {
            player.GetComponent<ReactorAttributes>()[statName].pointsAllocated += 1;
            allocSound.Play(0);
        }
        else if (points < 0)
        {
            player.GetComponent<ReactorAttributes>()[statName].pointsAllocated -= 1;
            deallocSound.Play(0);
        }
    }

    public void Pause()
    {
        Debug.Log("Paused");
        isPaused = true;
        Time.timeScale = 0.0f;
        loopingMusic.volume = loopingMusicVolume / 6;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<GunRotating>().enabled = false;
        player.GetComponent<MaxReactor>().enabled = false;
        escape_menu_sprite.SetActive(true);
        pauseButtons.SetActive(true);
        PauseAllSFX();
    }

    public void Unpause()
    {
        Debug.Log("Unpaused");
        isPaused = false;
        Time.timeScale = 1.0f;
        loopingMusic.volume = loopingMusicVolume;
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<GunRotating>().enabled = true;
        player.GetComponent<MaxReactor>().enabled = true;
        escape_menu_sprite.SetActive(false);
        pauseButtons.SetActive(false);
        ResumeAllSFX();
    }

    public void PauseAllSFX()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach( AudioSource audioS in allAudioSources) 
        {
            if(audioS.clip.name == "mecha_reactor_dungeon_crawl" || audioS.clip.name == "mecha_boss")
            {
                audioS.volume /= 2f;
            } 
            else
            {
                audioS.Pause();
            }
        }
    }

    public void ResumeAllSFX()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach( AudioSource audioS in allAudioSources) 
        {
            if(audioS.clip.name == "mecha_reactor_dungeon_crawl" || audioS.clip.name == "mecha_boss")
            {
                audioS.volume = 1f;
            } 
            else
            {
                audioS.UnPause();
            }
        }
    }

    public Transform getPlayerTransform()
    {
        return player.transform;
    }

    public void setPlayerTransform(Transform newTransform)
    {
        float xPos = newTransform.position.x;
        float yPos = newTransform.position.y;
        player.GetComponent<Transform>().position = new Vector3(xPos, yPos, 0.0f);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void StartCameraShake()
    {
        mainCamera.GetComponent<CameraShake>().StartShake();
    }

    //DEFUNCT FUNCTION
    public void setCameraBounds(float x, float y, float xOffset, float yOffset)
    {
        return;
    }

    public void setCameraSize(float newSize)
    {
        mainCamera.GetComponent<Camera>().orthographicSize = newSize;
    }
    public void moveCameraToPosition(Transform pos)
    {
        mainCamera.GetComponent<Transform>().position = pos.position + offset;
    }

    public void killPlayer()
    {
        foreach(Transform child in roomManager.rooms[roomManager.currentRoom].transform.Find("CurrentEnemies"))
        {
            // This is to prevent a quirky bug where the game doesn't like if an enemy and the player
            // kill each other at the same time :)
            child.GetComponent<EnemyController>().enabled = false;
        }
        StartCoroutine(PlayerDeath());
    }

    public void GoBackToPreviousRoom()
    {
        if(roomManager.rooms[roomManager.currentRoom].GetComponent<Room>().isBossRoom)
        {
            loopingMusic.Stop();
        }
        roomManager.goBack();
        
        float maxElec = player.GetComponent<ReactorAttributes>().GetMaxElectricity();
        player.GetComponent<ReactorAttributes>().AddElectricity(maxElec);

        playerController.setHealth(playerController.GetMaxHealth());
        player.GetComponent<SpriteRenderer>().enabled = true;
        
        isDying = false;
        playerController.enabled = true;
        playerController.isInvulnerable = false;
        deathSplash.SetActive(false);
        player.SetActive(true);
    }

    IEnumerator PlayerDeath()
    {
        isDying = true;
        deathSplash.SetActive(true);
        Camera cam = mainCamera.GetComponent<Camera>();
        playerController.enabled = false;
        SpriteRenderer playerRender = player.GetComponent<SpriteRenderer>();

        player.GetComponent<ReactorAttributes>().ResetPoints();
        GameObject fx = Instantiate(playerDeathEffect, player.GetComponent<Transform>().position, Quaternion.identity);
        player.SetActive(false);
        Destroy(fx, 1);
        yield return new WaitForSeconds(2.0f);
        GoBackToPreviousRoom();
    }
}
