using System;
using UnityEngine;

// Hito 2 — Sistema de salud y daño

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir este componente al Player y a todos los enemigos.

Componentes necesarios:
- Requiere un Collider2D en el mismo GameObject configurado para recibir impactos
  (los enemigos suelen tenerlo en modo normal y los coleccionables en trigger).

Referencias del Inspector:
- maxHealth: cantidad de vida máxima con la que inicia la entidad.

Layers y Tags:
- No requiere configuraciones específicas por sí mismo, pero depende
  de que las capas de colisión estén bien configuradas en el motor físico.

Notas:
- Ofrece los eventos OnHealthChanged (para actualizar barras de vida o UI)
  y OnDied (para desencadenar animaciones de muerte o desactivación).
*/
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool isPlayer = false;

    [Header("Auto Regeneración")]
    [SerializeField] private bool autoHealEnabled = false;
    [SerializeField] private float outOfCombatTime = 5f;
    [SerializeField] private float healInterval = 0.5f;
    [SerializeField] private int healAmount = 10; // 10 equivale a 1 corazón

    private int currentHealth;
    private float timeSinceLastDamage = 0f;
    private float healTimer = 0f;

    // Evento que notifica cambios en la salud. Pasa (vidaActual, vidaMaxima).
    public event Action<int, int> OnHealthChanged;

    // Evento que se ejecuta cada vez que la entidad recibe daño.
    public event Action<int> OnDamaged;

    // Evento que se ejecuta únicamente cuando la salud llega a cero.
    public event Action OnDied;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        if (isPlayer && PlayerPrefs.HasKey("PlayerHealth"))
        {
            currentHealth = PlayerPrefs.GetInt("PlayerHealth");
        }
        else
        {
            currentHealth = maxHealth;
        }
    }

    private void Update()
    {
        if (!autoHealEnabled || !IsAlive() || currentHealth >= maxHealth) return;

        timeSinceLastDamage += Time.deltaTime;
        
        if (timeSinceLastDamage >= outOfCombatTime)
        {
            healTimer += Time.deltaTime;
            if (healTimer >= healInterval)
            {
                healTimer -= healInterval; // Reiniciamos el temporizador para el siguiente tick
                Heal(healAmount);
            }
        }
        else
        {
            healTimer = 0f;
        }
    }

    // Resta vida a la entidad. Si llega a 0, lanza OnDied.
    public void TakeDamage(int amount)
    {
        if (!IsAlive()) return;
        if (amount < 0) amount = 0;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        timeSinceLastDamage = 0f; // Reinicia el contador de fuera de combate

        if (isPlayer) PlayerPrefs.SetInt("PlayerHealth", currentHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamaged?.Invoke(amount);

        if (currentHealth == 0)
        {
            if (isPlayer) 
            {
                PlayerPrefs.DeleteKey("PlayerHealth");
                PlayerPrefs.DeleteKey("PlayerScore");
            }
            OnDied?.Invoke();
        }
    }

    // Cura a la entidad sin sobrepasar el límite de vida máxima.
    public void Heal(int amount)
    {
        if (!IsAlive()) return;
        if (amount < 0) amount = 0;

        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (isPlayer) PlayerPrefs.SetInt("PlayerHealth", currentHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // Devuelve si la entidad tiene más de 0 de vida.
    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    // Restablece la salud al valor máximo y notifica los cambios.
    public void RestoreToFull()
    {
        currentHealth = maxHealth;
        if (isPlayer) PlayerPrefs.SetInt("PlayerHealth", currentHealth);
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
