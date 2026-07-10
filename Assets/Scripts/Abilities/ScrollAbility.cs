using UnityEngine;

// Hito 10 — Pergaminos y habilidades

/*
ScrollAbility es una clase abstracta que define el contrato común
para todas las habilidades de pergamino del ninja.

No se añade directamente a ningún GameObject. Extenderla con clases concretas
como FireAbility para implementar habilidades específicas.

Demuestra: Abstracción (solo define qué puede hacer, no cómo) y
           Herencia (FireAbility extiende esta clase).
*/
public abstract class ScrollAbility : MonoBehaviour
{
    [SerializeField] protected float cooldown = 1.5f;
    [SerializeField] protected int damage = 15;

    private float cooldownTimer = 0f;

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    // Intenta usar la habilidad. Devuelve verdadero si se pudo activar.
    public bool TryUse(Vector2 direction)
    {
        if (!CanUse()) return false;

        Execute(direction);
        cooldownTimer = cooldown;
        return true;
    }

    // Indica si la habilidad puede usarse ahora (cooldown terminado).
    public bool CanUse()
    {
        return cooldownTimer <= 0f;
    }

    // Cada habilidad concreta define su efecto aquí.
    protected abstract void Execute(Vector2 direction);

    // Devuelve el tiempo restante de cooldown. Útil para mostrar en HUD.
    public float GetCooldownRemaining()
    {
        return cooldownTimer;
    }
}
