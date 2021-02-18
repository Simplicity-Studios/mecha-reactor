using UnityEngine;

public class HealthbarHandler : MonoBehaviour
{
    public PlayerController player;
    private RectTransform currHealthbar;
    private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealthbar = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        currHealthbar.anchoredPosition = new Vector2(-gameObject.GetComponent<RectTransform>().rect.width / 2, 0);
        maxHealth = gameObject.GetComponent<RectTransform>().rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        float widthDelta = maxHealth / player.GetMaxHealth();
        currHealthbar.sizeDelta = new Vector2(player.GetHealth() * widthDelta, currHealthbar.sizeDelta.y);
        print(maxHealth + " ==> " + currHealthbar.sizeDelta);
    }
}
