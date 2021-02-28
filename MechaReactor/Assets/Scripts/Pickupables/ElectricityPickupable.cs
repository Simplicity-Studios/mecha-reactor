using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityPickupable : MonoBehaviour
{
    public float amountOfElectricity = 25f;
    public AudioClip source;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // if the thing collided with is a player, 
        if (collider.gameObject.name == "Player") 
        {
            ReactorAttributes player = collider.gameObject.GetComponent<ReactorAttributes>();
            // give the player the electricity 
            player.AddElectricity(amountOfElectricity);
            // play sound
            AudioSource.PlayClipAtPoint(source, Camera.main.transform.position, 0.50f);
            // and get rid of this pickupable
            Destroy(gameObject); 
        }
    }
}
