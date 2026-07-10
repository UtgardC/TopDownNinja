using UnityEngine;

// Hito 8 — Coleccionables y puntuación

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject en el nivel que represente comida (poción, manzana, etc.).

Componentes necesarios:
- Collider2D configurado como Trigger.
- SpriteRenderer con el sprite de comida correspondiente.

Referencias del Inspector:
- healAmount: cantidad de vida que recupera el jugador al recogerla.

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Llama a Health.Heal a través del PlayerCollector.
- Solo tiene efecto si el jugador no tiene la vida al máximo, pero Heal ya lo maneja.
*/
public class FoodCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private int healAmount = 25;

    // Cura al jugador y desactiva el objeto.
    public void Collect(PlayerCollector collector)
    {
        collector.Health.Heal(healAmount);
        gameObject.SetActive(false);
    }
}
