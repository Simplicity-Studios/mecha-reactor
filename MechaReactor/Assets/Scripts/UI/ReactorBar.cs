using UnityEngine;
using UnityEngine.UI;

public class ReactorBar : MonoBehaviour
{
    public GameObject[] reactorButtons;
    public Sprite[] sprites;

    private int points;
    private Image barSprite;

    void Start()
    {
        points = reactorButtons.Length - 1;
        barSprite = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        int tempMax = sprites.Length - 1;
        foreach (GameObject rButton in reactorButtons)
            tempMax -= rButton.GetComponent<ReactorButton>().GetPointsAllocated();
        points = tempMax;

        barSprite.sprite = sprites[points];
    }

    public int GetCurrPoints()
    {
        return points;
    }

    public int GetMaxPoints()
    {
        return sprites.Length - 1;
    }
}
