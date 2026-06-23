using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string itemName = "Item";

    public event Action OnCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        OnCollected?.Invoke();

        Destroy(gameObject);
    }
}
