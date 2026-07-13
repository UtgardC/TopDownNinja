using UnityEngine;

// Hito 8 — Comida recolectable (curación)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un objeto en la escena con SpriteRenderer.

Componentes necesarios:
- Collider2D (ej: CircleCollider2D) con "Is Trigger" marcado = true.

Referencias del Inspector:
- healAmount: cantidad de vida que restaura al jugador.
*/
public class FoodCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private int healAmount = 25;

    // Se ejecuta al ser tocado por el PlayerCollector. Cura al jugador y se desactiva.
    public void Collect(PlayerCollector collector)
    {
        collector.Health.Heal(healAmount);
        gameObject.SetActive(false);
    }
}
