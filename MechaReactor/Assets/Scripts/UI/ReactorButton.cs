using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReactorButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject player, overallReactorBar;
    public Sprite[] pointsbarSprites;
    public string attributeName;
    public float minScaleValue, maxScaleValue;

    private int pointsAllocated;
    private float step, maxAllocated, maxOverall;
    private ReactorAttributes stats;
    private Image fillBar;

    void Start()
    {
        pointsAllocated = 0;
        // Max possible points one button can allocate to an attribute
        maxAllocated = pointsbarSprites.Length - 1;
        maxOverall = overallReactorBar.GetComponent<ReactorBar>().GetMaxPoints();
        // Amount to scale by for each additional point allocated
        step = (maxScaleValue - minScaleValue) / maxAllocated;
        // Image component of the bar that shows the user how many points have been allocated
        fillBar = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        stats = player.GetComponent<ReactorAttributes>();
        // Initializing statistic to lowest possible value
        stats[attributeName] = minScaleValue;
    }

    public int GetPointsAllocated()
    {
        return pointsAllocated;
    }

    // Handling allocating points when user clicks the button. Does bounds checking to
    // make sure points aren't over/under distributed, and changes sprite as necessary.
    public void OnPointerClick(PointerEventData eventData)
    {
        float pointsRemaining = overallReactorBar.GetComponent<ReactorBar>().GetCurrPoints();

        if (eventData.button == PointerEventData.InputButton.Left 
            && pointsRemaining > 0 && pointsAllocated < maxAllocated)
        {
            stats[attributeName] += step;
            pointsAllocated += 1;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && pointsAllocated > 0)
        {
            stats[attributeName] -= step;
            pointsAllocated -= 1;
        }
        
        fillBar.sprite = pointsbarSprites[pointsAllocated];
    }
}
