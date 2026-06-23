using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string dialogueLine = "¡Hola, aventurero!";

    public event Action<string> OnInteract;

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
            OnInteract?.Invoke(dialogueLine);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
