using System;
using UnityEngine;

// Hito 2 — Salud, daño y eventos

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir a cualquier GameObject que necesite vida: jugador y enemigos.

Componentes necesarios:
- Ninguno adicional obligatorio.

Referencias del Inspector:
- maxHealth: vida máxima del objeto (entero positivo).

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Suscribir OnHealthChanged para actualizar la barra de vida en el HUD.
- Suscribir OnDied para manejar la muerte: destruir el objeto, notificar derrota, etc.
- Este script implementa IDamageable: cualquier ataque puede llamar TakeDamage
  sin conocer si el objetivo es el jugador, un enemigo o un destructible.
*/
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    // Notifica cuando la vida cambia: envía salud actual y máxima.
    public event Action<int, int> OnHealthChanged;

    // Notifican la cantidad real aplicada. Son útiles para animación, audio y feedback.
    public event Action<int> OnDamaged;
    public event Action<int> OnHealed;

    // Notifica cuando la vida llega a cero.
    public event Action OnDied;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Aplica daño al objeto. Llama a OnDied si la vida llega a cero.
    public void TakeDamage(int amount)
    {
        if (!IsAlive()) return;
        if (amount <= 0) return;

        int previousHealth = currentHealth;
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        int appliedDamage = previousHealth - currentHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamaged?.Invoke(appliedDamage);

        if (currentHealth == 0)
        {
            OnDied?.Invoke();
        }
    }

    // Aplica curación. No puede superar la vida máxima.
    public void Heal(int amount)
    {
        if (!IsAlive()) return;
        if (amount <= 0) return;

        int previousHealth = currentHealth;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        int appliedHealing = currentHealth - previousHealth;
        if (appliedHealing == 0) return;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnHealed?.Invoke(appliedHealing);
    }

    // Devuelve verdadero si la vida es mayor que cero.
    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
