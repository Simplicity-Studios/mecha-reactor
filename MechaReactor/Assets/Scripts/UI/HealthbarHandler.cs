using UnityEngine;

public class HealthbarHandler : MonoBehaviour
{
    public PlayerController player;
    private RectTransform currHealthbar;
    private float maxWidth;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        currHealthbar = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        currHealthbar.anchoredPosition = new Vector2(-gameObject.GetComponent<RectTransform>().rect.width / 2, 0);
        maxWidth = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        float widthDelta = maxWidth / player.GetMaxHealth();
        currHealthbar.sizeDelta = new Vector2(player.GetHealth() * widthDelta, height);
    }
}
