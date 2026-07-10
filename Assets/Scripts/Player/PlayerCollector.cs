using System;
using UnityEngine;

// Hito 8 — Coleccionables y puntuación

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador.

Componentes necesarios:
- Collider2D configurado como Trigger en el GameObject del jugador.
- Health en el mismo GameObject (para que FoodCollectible pueda curar).
- ScoreTracker en el mismo GameObject.
- TemporaryPowerUpController en el mismo GameObject.

Referencias del Inspector:
- health: arrastrar el componente Health del jugador.
- scoreTracker: arrastrar el componente ScoreTracker del jugador.
- powerUpController: arrastrar el componente TemporaryPowerUpController del jugador.

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Los objetos coleccionables deben tener un Collider2D como Trigger.
- OnTriggerEnter2D detecta el contacto y llama a ICollectible.Collect.
- Las referencias a Health, ScoreTracker y PowerUpController son públicas
  para que los collectibles puedan acceder a ellas.
*/
public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private TemporaryPowerUpController powerUpController;

    // Notifica cuando se recoge un coleccionable.
    public event Action<ICollectible> OnCollected;

    public Health Health => health;
    public ScoreTracker ScoreTracker => scoreTracker;
    public TemporaryPowerUpController PowerUpController => powerUpController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollectible collectible = other.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.Collect(this);
            OnCollected?.Invoke(collectible);
        }
    }
}
