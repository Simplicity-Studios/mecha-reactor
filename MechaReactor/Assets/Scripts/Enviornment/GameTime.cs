using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    private Text text;
    public GameManager gameManager;
    
    void Start()
    {
        text = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        text.text = gameManager.timeValue.ToString("0.00");
    }
}
