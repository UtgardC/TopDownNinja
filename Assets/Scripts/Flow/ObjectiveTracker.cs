using System;
using UnityEngine;

// Hito 12 — Rastreador de objetivos (victoria)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject vacío "ObjectiveTracker" en la escena.

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- boss: arrastrar el GameObject del Boss final de la escena.

Notas:
- Se suscribe a la muerte del jefe final. Cuando muere, lanza el evento OnVictory
  para que HUDController o GameResultController muestren los paneles y pausen.
*/
public class ObjectiveTracker : MonoBehaviour
{
    [SerializeField] private BossEnemy boss;

    private bool objectiveComplete = false;

    // Evento de victoria del nivel.
    public event Action OnVictory;

    private void Start()
    {
        if (boss != null)
        {
            boss.OnBossDefeated += CompleteObjective;
        }
    }

    // Completa el objetivo y dispara el evento de victoria.
    public void CompleteObjective()
    {
        if (objectiveComplete) return;

        objectiveComplete = true;
        OnVictory?.Invoke();
    }

    public bool IsComplete()
    {
        return objectiveComplete;
    }

    private void OnDestroy()
    {
        if (boss != null)
        {
            boss.OnBossDefeated -= CompleteObjective;
        }
    }
}
