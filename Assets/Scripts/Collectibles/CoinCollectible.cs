using UnityEngine;

// Hito 8 — Moneda recolectable

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un objeto en la escena con SpriteRenderer.
- Asignar la layer "Ignore Raycast" o similar si da problemas, pero lo
  más importante es que use un Collider2D en modo Trigger.

Componentes necesarios:
- Collider2D (ej: CircleCollider2D) con "Is Trigger" marcado = true.

Referencias del Inspector:
- scoreValue: cantidad de puntos que otorga al jugador al ser recolectada.
*/
public class CoinCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private int scoreValue = 10;

    // Se ejecuta al ser tocado por el PlayerCollector. Suma puntos y se desactiva.
    public void Collect(PlayerCollector collector)
    {
        collector.ScoreTracker.AddScore(scoreValue);
        gameObject.SetActive(false);
    }
}
