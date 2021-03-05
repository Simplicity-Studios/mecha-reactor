using UnityEngine;
using UnityEngine.UI;

public class CooldownVisual : MonoBehaviour
{
    public GameObject player;
    private Image img;
    private float baseTime;

    void Start()
    {
        img = GetComponent<Image>();
        baseTime = player.GetComponent<PlayerController>().baseSpecialTime;
    }

    void Update()
    {
        Color temp = img.color;
        temp.a = (player.GetComponent<PlayerController>().lastSpecialUse + baseTime - Time.time) / baseTime;
        img.color = temp;
    }
}
