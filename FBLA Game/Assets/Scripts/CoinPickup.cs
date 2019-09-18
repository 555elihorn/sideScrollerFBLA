using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX = null;
    [SerializeField] int pointsForCoinPickup = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.ToString().Contains("Capsule"))
        {
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position, 0.5f);
            Destroy(gameObject);
        }
    }
}
