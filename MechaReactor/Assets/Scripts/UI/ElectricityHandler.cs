using UnityEngine;

public class ElectricityHandler : MonoBehaviour
{
    public ReactorAttributes stats;

    private RectTransform currElectricityBar;
    private float maxWidth, height, conversionRatio;

    void Start()
    {
        // Set up the moving bar size (child) based on parent's width and height
        currElectricityBar = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        currElectricityBar.anchoredPosition = new Vector2(-gameObject.GetComponent<RectTransform>().rect.width / 2, 0);
        maxWidth = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;
        currElectricityBar.sizeDelta = new Vector2(maxWidth, height);

        // Ratio to go from electricity to UI width
        conversionRatio = maxWidth / stats.GetMaxElectricity();
    }

    void Update()
    {
        currElectricityBar.sizeDelta = new Vector2(stats.GetElectricity() * conversionRatio, height);
    }
}
