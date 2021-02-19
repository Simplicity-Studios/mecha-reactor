using UnityEngine;

public class ElectricityHandler : MonoBehaviour
{
    [Range(1, 10)]
    public int decreaseScale = 1;

    public ReactorBar reactorBar;
    private RectTransform currElectricityBar;
    private float maxWidth;
    private float height;

    void Start()
    {
        currElectricityBar = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        currElectricityBar.anchoredPosition = new Vector2(-gameObject.GetComponent<RectTransform>().rect.width / 2, 0);
        maxWidth = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;
        currElectricityBar.sizeDelta = new Vector2(maxWidth, height);
    }

    void Update()
    {
        float pointsScale = reactorBar.GetMaxPoints() - reactorBar.GetCurrPoints();
        float widthDelta = currElectricityBar.sizeDelta.x - (pointsScale * decreaseScale * 0.005f);
        currElectricityBar.sizeDelta = new Vector2(widthDelta, height);
    }
}
