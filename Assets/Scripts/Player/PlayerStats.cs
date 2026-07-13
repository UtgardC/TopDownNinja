using UnityEngine;

// Hito 3 — Control de estadísticas y modificadores

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador ("Player").

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- moveSpeed: velocidad de movimiento base.
- baseDamage: daño por defecto en ataques cuerpo a cuerpo.
- attackCooldown: tiempo de espera base entre ataques.

Notas:
- Centraliza las estadísticas del jugador para que otros sistemas (como PowerUps)
  puedan aplicar modificadores temporales de forma ordenada.
*/
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float attackCooldown = 0.5f;

    // Multiplicadores aplicados por buffs temporales
    private float speedMultiplier = 1f;
    private float damageMultiplier = 1f;
    private float attackSpeedMultiplier = 1f;

    // Propiedades públicas que devuelven el valor modificado final
    public float MoveSpeed => moveSpeed * speedMultiplier;
    public int Damage => Mathf.RoundToInt(baseDamage * damageMultiplier);
    public float AttackCooldown => attackCooldown / attackSpeedMultiplier;

    // Métodos para aplicar modificadores (llamados por el TemporaryPowerUpController)
    public void ApplySpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void ApplyDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public void ApplyAttackSpeedMultiplier(float multiplier)
    {
        attackSpeedMultiplier = multiplier;
    }

    // Métodos para restablecer los modificadores a su valor base (x1)
    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f;
    }

    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1f;
    }

    public void ResetAttackSpeedMultiplier()
    {
        attackSpeedMultiplier = 1f;
    }
}
