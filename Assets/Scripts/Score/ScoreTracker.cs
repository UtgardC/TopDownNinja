using System;
using UnityEngine;

// Hito 8 — Coleccionables y puntuación

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador.

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- Ninguna (el puntaje es interno).

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Suscribir OnScoreChanged desde HUDController para mostrar el puntaje actualizado.
- Los coleccionables llaman a AddScore a través de PlayerCollector.ScoreTracker.
*/
public class ScoreTracker : MonoBehaviour
{
    private int score = 0;

    // Notifica cuando el puntaje cambia. Envía el puntaje nuevo.
    public event Action<int> OnScoreChanged;

    public int Score => score;

    // Agrega puntos al puntaje actual y notifica el cambio.
    public void AddScore(int amount)
    {
        if (amount < 0) amount = 0;

        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    // Devuelve el puntaje actual. Útil para mostrarlo en HUD o pantalla final.
    public int GetScore()
    {
        return score;
    }
}
