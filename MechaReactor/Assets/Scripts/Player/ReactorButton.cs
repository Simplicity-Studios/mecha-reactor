using UnityEngine;
using UnityEngine.EventSystems;

public class ReactorButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject player;

    public string attributeName;
    public float minValue, maxValue;
    public int maxLevel;
    
    private int pointsAllocated;
    private float step;
    private ReactorAttributes stats;

    void Start()
    {
        pointsAllocated = 0;
        step = (maxValue - minValue) / maxLevel;
        stats = player.GetComponent<ReactorAttributes>();
        stats[attributeName] = Mathf.Clamp(stats[attributeName], minValue, maxValue);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            stats[attributeName] += step;
        if (eventData.button == PointerEventData.InputButton.Right)
            stats[attributeName] -= step;
        
        stats[attributeName] = Mathf.Clamp(stats[attributeName], minValue, maxValue);
    }
}
