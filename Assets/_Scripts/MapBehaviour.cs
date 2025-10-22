using System;
using System.Collections;
using UnityEngine;
public class MapBehaviour : MonoBehaviour
{
    private PlayerObject player;
     // Static event to notify listeners that player died
    
    private void Start() {
        player = GetComponent<PlayerObject>();
        if (player == null) {
            Debug.LogError("PlayerObject component missing on this GameObject!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Spikes")) {
            if (player != null && !player.isDead) {
                player.die();
            }
        }
    }
}
