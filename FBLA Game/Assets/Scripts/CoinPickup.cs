using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    //config
    [SerializeField] AudioClip coinPickUpSFX = null;
    [SerializeField] int pointsForCoinPickup = 100;
    

    //On trigger if the player collides with the coin; remove the coin and add to score
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position, 0.5f);
            Destroy(gameObject);
        }
    }
}
