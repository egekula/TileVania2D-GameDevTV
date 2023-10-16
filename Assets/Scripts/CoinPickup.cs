using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;

    private bool _wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !_wasCollected)
        {
            _wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX,Camera.main.transform.position);
            FindObjectOfType<GameSession>().TakeCoin(10);
            gameObject.SetActive(false);
        }
    }
}
