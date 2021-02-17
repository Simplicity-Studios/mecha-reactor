using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReactorButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject player;
    public Sprite[] fillBarSprites;
    public GameObject totalPointsObject;

    public string attributeName;
    public float minValue, maxValue;

    private int pointsAllocated;
    private float step, maxLevel;
    private ReactorAttributes stats;
    private Image fillBar;

    void Start()
    {
        maxLevel = fillBarSprites.Length - 1;
        pointsAllocated = 0;
        step = (maxValue - minValue) / maxLevel;

        fillBar = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        stats = player.GetComponent<ReactorAttributes>();
        stats[attributeName] = Mathf.Clamp(stats[attributeName], minValue, maxValue);
    }

    public int GetPointsAllocated()
    {
        return pointsAllocated;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (totalPointsObject.GetComponent<PointsLeftoverBar>().GetLevel() > 0)
        {
            if (eventData.button == PointerEventData.InputButton.Left) 
            {
                stats[attributeName] += step;
                pointsAllocated += 1;
            }
            if (eventData.button == PointerEventData.InputButton.Right) 
            {
                stats[attributeName] -= step;
                pointsAllocated -= 1;
            }
            
            stats[attributeName] = Mathf.Clamp(stats[attributeName], minValue, maxValue);
            pointsAllocated = (int) Mathf.Clamp(pointsAllocated, 0, maxLevel);

            fillBar.sprite = fillBarSprites[pointsAllocated];
        }
    }
}
