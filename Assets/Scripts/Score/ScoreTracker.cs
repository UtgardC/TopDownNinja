using System;
using UnityEngine;

// Hito 8 — Registro de puntuación

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador ("Player").

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- Ninguna requerida.

Notas:
- Ofrece el evento OnScoreChanged para notificar a la UI sobre cambios en los puntos.
*/
public class ScoreTracker : MonoBehaviour
{
    private int score = 0;

    // Evento que notifica la nueva puntuación cada vez que se añaden puntos.
    public event Action<int> OnScoreChanged;

    public int Score => score;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerScore"))
        {
            score = PlayerPrefs.GetInt("PlayerScore");
        }
    }

    // Suma puntos al marcador y lanza OnScoreChanged.
    public void AddScore(int amount)
    {
        if (amount < 0) amount = 0;

        score += amount;
        PlayerPrefs.SetInt("PlayerScore", score);
        OnScoreChanged?.Invoke(score);
    }

    // Devuelve la puntuación acumulada.
    public int GetScore()
    {
        return score;
    }
}
