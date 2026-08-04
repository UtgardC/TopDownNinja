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
    [SerializeField] protected int maxCharges = 10;

    [SerializeField] protected float castTime = 0.5f;

    public abstract ScrollType AbilityType { get; }
    public float CastTime => castTime;

    private float cooldownTimer = 0f;
    private int currentCharges = 0;

    protected virtual void Awake()
    {
        // Inicializa las cargas al máximo al arrancar
        currentCharges = maxCharges;
    }

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    // Comprueba si el cooldown está listo y si quedan cargas ANTES de iniciar la animación.
    public bool CheckCanUse()
    {
        return cooldownTimer <= 0f && currentCharges > 0;
    }

    // Método que se llama una vez terminada la animación de casteo para disparar el efecto.
    public void ConsumeAndExecute(Vector2 direction)
    {
        if (currentCharges <= 0) return; // Por si algo cambió durante el casteo

        Execute(direction);
        cooldownTimer = cooldown;
        currentCharges--;
    }

    // Recarga las cargas al máximo (útil para cuando se recoge el pergamino de nuevo).
    public void RefillCharges()
    {
        currentCharges = maxCharges;
    }

    public int GetCurrentCharges()
    {
        return currentCharges;
    }

    // Lógica interna de cada habilidad concreta (ej: instanciar bola de fuego).
    protected abstract void Execute(Vector2 direction);

    public float GetCooldownRemaining()
    {
        return cooldownTimer;
    }

    public float GetMaxCooldown()
    {
        return cooldown;
    }
}
