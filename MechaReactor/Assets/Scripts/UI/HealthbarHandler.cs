using UnityEngine;

public class HealthbarHandler : MonoBehaviour
{
    public PlayerController player;
    private RectTransform currHealthbar;
    private float maxWidth, height, conversionRatio;

    void Start()
    {
        // Set up moving bar size (child) based on parent's width and height
        currHealthbar = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        currHealthbar.anchoredPosition = new Vector2(-gameObject.GetComponent<RectTransform>().rect.width / 2, 0);
        maxWidth = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;

        conversionRatio = maxWidth / player.GetMaxHealth();
    }

    void Update()
    {
        currHealthbar.sizeDelta = new Vector2(player.GetHealth() * conversionRatio, height);
    }
}
