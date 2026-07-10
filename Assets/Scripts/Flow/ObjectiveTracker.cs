using System;
using UnityEngine;

// Hito 12 — Tutorial, progresión y objetivo

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject vacío "ObjectiveTracker" en la escena del nivel principal.

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- boss: arrastrar el componente BossEnemy del jefe final de la escena.

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Se suscribe al evento OnBossDefeated del jefe para detectar la victoria.
- OnVictory es escuchado por GameResultController para mostrar la pantalla de victoria.
- Si el juego tiene más condiciones de victoria (ej: recuperar la comida),
  agregar más referencias y llamar CompleteObjective() desde los coleccionables correspondientes.
*/
public class ObjectiveTracker : MonoBehaviour
{
    [SerializeField] private BossEnemy boss;

    private bool objectiveComplete = false;

    // Notifica cuando se cumple el objetivo del nivel.
    public event Action OnVictory;

    private void Start()
    {
        if (boss != null)
        {
            boss.OnBossDefeated += CompleteObjective;
        }
    }

    // Marca el objetivo como completado y lanza el evento de victoria.
    public void CompleteObjective()
    {
        if (objectiveComplete) return;

        objectiveComplete = true;
        OnVictory?.Invoke();
    }

    // Indica si el objetivo ya fue cumplido.
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
