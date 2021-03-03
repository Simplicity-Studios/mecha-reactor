using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCountIndicator : MonoBehaviour
{

    public RoomManager roomManager;
    public Text enemyCountIndicator;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        enemyCountIndicator = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCountIndicator.text = roomManager.getNumberOfEnemiesLeft().ToString();
    }
}
