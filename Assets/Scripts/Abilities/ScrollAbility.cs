using UnityEngine;

// Hito 10 — Habilidad base de Pergamino (Clase abstracta)

/*
CONFIGURACIÓN EN UNITY
- NO añadir directamente. Usar FireAbility o clases derivadas.
*/
public abstract class ScrollAbility : MonoBehaviour
{
    [SerializeField] protected float cooldown = 1.5f;
    [SerializeField] protected int damage = 15;

    public abstract ScrollType AbilityType { get; }

    private float cooldownTimer = 0f;

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    // Método principal para intentar lanzar la habilidad.
    public bool TryUse(Vector2 direction)
    {
        if (!CanUse()) return false;

        Execute(direction);
        cooldownTimer = cooldown;
        return true;
    }

    // Comprueba si el cooldown está listo.
    public bool CanUse()
    {
        return cooldownTimer <= 0f;
    }

    // Lógica interna de cada habilidad concreta (ej: instanciar bola de fuego).
    protected abstract void Execute(Vector2 direction);

    public float GetCooldownRemaining()
    {
        return cooldownTimer;
    }
}
