using UnityEngine;
using UnityEngine.UI;

public class ReactorBar : MonoBehaviour
{
    public ReactorAttributes stats;
    public Sprite[] sprites;

    private int points;
    private Image barSprite;

    void Start()
    {
        barSprite = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        points = (stats.GetMaxPoints() - 1) - stats.GetTotalPointsAllocated();
        barSprite.sprite = sprites[points];
    }
}
