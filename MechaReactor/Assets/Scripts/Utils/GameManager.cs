using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public GameObject mainCamera;

    public GameObject playerDeathEffect;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private PlayerController playerController;
    private RoomManager roomManager;
    private bool isDying = false;

    private AudioSource loopingMusic;

    void Start()
    {
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
    }

    void Update()
    {
        if(playerController.GetHealth() <= 0.0f && !isDying)
        {
            killPlayer();
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
        StartCoroutine(PlayerDeath());
    }

    public void GoBackToPreviousRoom()
    {
        roomManager.goBack();
        
        float maxElec = player.GetComponent<ReactorAttributes>().GetMaxElectricity();
        player.GetComponent<ReactorAttributes>().AddElectricity(maxElec);

        playerController.setHealth(playerController.GetMaxHealth());
        
        isDying = false;
        playerController.enabled = true;
        player.SetActive(true);
    }

    IEnumerator PlayerDeath()
    {
        isDying = true;
        Camera cam = mainCamera.GetComponent<Camera>();
        playerController.enabled = false;
        SpriteRenderer playerRender = player.GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < 10; ++i)
        {
            playerRender.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(0.2f);
            playerRender.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);
        }
        GameObject fx = Instantiate(playerDeathEffect, player.GetComponent<Transform>().position, Quaternion.identity);
        Destroy(fx, 1);
        player.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        GoBackToPreviousRoom();
    }
}
