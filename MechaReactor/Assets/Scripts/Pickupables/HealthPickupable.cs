using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupable : MonoBehaviour
{

    public float amountOfHealth = 25f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // if the thing collided with is a player, 
        if (collider.gameObject.name == "Player") 
        {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            // give the player the health 
            player.AddHealth(amountOfHealth);
            // and get rid of this pickupable
            Destroy(gameObject); 
        }
    }
}
