using UnityEngine;
using UnityEngine.UI;

public class PointsLeftoverBar : MonoBehaviour
{
    public GameObject[] reactorButtons;
    public Sprite[] sprites;

    private int level;
    private Image barSprite;

    void Start()
    {
        level = reactorButtons.Length - 1;
        barSprite = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        int tempMax = sprites.Length - 1;
        foreach (GameObject rButton in reactorButtons)
            tempMax -= rButton.GetComponent<ReactorButton>().GetPointsAllocated();
        level = tempMax;

        barSprite.sprite = sprites[level];
    }

    public int GetLevel()
    {
        return level;
    }
}
