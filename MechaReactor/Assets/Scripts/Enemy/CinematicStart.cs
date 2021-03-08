using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicStart : MonoBehaviour
{
    public AudioClip bossMusic;
    public AudioSource BossRoar;
    private AudioSource GameManagerMusicLoop;

    private GameObject player;
    private GameManager gameManager;

    private bool hasStartedCutscene = false;

    void Start()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameManagerMusicLoop = GameObject.Find("GameManager").GetComponents<AudioSource>()[0];
    }

    void Update()
    {
        if(Vector2.Distance(player.transform.position, this.transform.position) <= 9 && !hasStartedCutscene)
        {
            hasStartedCutscene = true;
            StartCoroutine(BossCutsceneStart());
        }
    }

    IEnumerator BossCutsceneStart()
    {
        GameManagerMusicLoop.Stop();
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<GunRotating>().enabled = false;
        player.GetComponent<MaxReactor>().enabled = false;
        GameManagerMusicLoop.clip = bossMusic;
        yield return new WaitForSeconds(1.0f);
        BossRoar.Play();
        for(int i = 0; i < 20; ++i)
        {
            gameManager.StartCameraShake();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        GameManagerMusicLoop.Play();
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<GunRotating>().enabled = true;
        player.GetComponent<MaxReactor>().enabled = true;

        GetComponent<EnemyController>().enabled = true;
        GetComponent<FinalBoss>().enabled = true;

        this.enabled = false;
    }
}
