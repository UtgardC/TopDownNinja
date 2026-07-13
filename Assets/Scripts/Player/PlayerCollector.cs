using System;
using UnityEngine;

// Hito 8 — Recolección de coleccionables

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador ("Player").

Componentes necesarios:
- Collider2D configurado en modo Trigger.
- Rigidbody2D (Dynamic, configurado en PlayerMovement).
- Health, ScoreTracker y TemporaryPowerUpController en el mismo GameObject.

Referencias del Inspector:
- health: arrastrar el componente Health del jugador.
- scoreTracker: arrastrar el componente ScoreTracker del jugador.
- powerUpController: arrastrar el componente TemporaryPowerUpController del jugador.
*/
public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private TemporaryPowerUpController powerUpController;
    [SerializeField] private ScrollLoadout scrollLoadout;

    // Evento útil si se quiere añadir efectos de partículas o sonido globales.
    public event Action<ICollectible> OnCollected;

    public Health Health => health;
    public ScoreTracker ScoreTracker => scoreTracker;
    public TemporaryPowerUpController PowerUpController => powerUpController;
    public ScrollLoadout ScrollLoadout => scrollLoadout;

    // Detecta el contacto físico (Trigger) con coleccionables.
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
