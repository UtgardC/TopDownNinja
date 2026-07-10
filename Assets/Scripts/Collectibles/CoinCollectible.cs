using UnityEngine;

// Hito 8 — Coleccionables y puntuación

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject en el nivel que represente una moneda.

Componentes necesarios:
- Collider2D configurado como Trigger.
- SpriteRenderer con el sprite de moneda.

Referencias del Inspector:
- scoreValue: cuántos puntos otorga esta moneda al ser recogida.

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Al ser recogida llama a ScoreTracker.AddScore a través del PlayerCollector.
- El GameObject se desactiva al recogerse (en lugar de destruirse) para
  facilitar el reuso por Object Pooling si se implementa más adelante.
*/
public class CoinCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private int scoreValue = 10;

    // Suma puntos al marcador del jugador y desactiva el objeto.
    public void Collect(PlayerCollector collector)
    {
        collector.ScoreTracker.AddScore(scoreValue);
        gameObject.SetActive(false);
    }
}
