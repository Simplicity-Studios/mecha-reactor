﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReactorButton : MonoBehaviour, IPointerClickHandler
{
    public AudioSource allocSound, deallocSound;
    public GameObject player, overallReactorBar;
    public Sprite[] pointsbarSprites;
    public string attributeName;

    private ReactorAttributes stats;
    private Image fillBar;

    void Start()
    {
        // Image component of the bar that shows the user how many points have been allocated
        fillBar = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        stats = player.GetComponent<ReactorAttributes>();
    }

    void Update()
    {
        fillBar.sprite = pointsbarSprites[stats[attributeName].pointsAllocated];
        GetComponent<Button>().interactable = stats[attributeName].IsEnabled();
    }

    // Handling allocating points when user clicks the button. Does bounds checking to
    // make sure points aren't over/under distributed, and changes sprite as necessary.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (stats[attributeName].IsEnabled()
            && eventData.pointerPressRaycast.gameObject.CompareTag("ReactorButton"))
        {
            float pointsRemaining = stats.GetMaxPoints() - stats.GetTotalPointsAllocated() - 1;

            if (eventData.button == PointerEventData.InputButton.Left && pointsRemaining > 0)
            {
                stats[attributeName].pointsAllocated += 1;
                allocSound.Play(0);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                stats[attributeName].pointsAllocated -= 1;
                deallocSound.Play(0);
            }
        }
    }
}
