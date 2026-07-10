using UnityEngine;

// Hito 3 — Movimiento y combate del jugador

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador junto a PlayerMovement y PlayerAttack.

Componentes necesarios:
- Ninguno adicional (los otros scripts leen las propiedades de este).

Referencias del Inspector:
- moveSpeed: velocidad base de movimiento (unidades/segundo).
- baseDamage: daño base del ataque principal.
- attackCooldown: tiempo base entre ataques (segundos).

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- PlayerMovement lee MoveSpeed.
- PlayerAttack lee Damage y AttackCooldown.
- TemporaryPowerUpController modifica los multiplicadores para aplicar buffs.
- Los multiplicadores vuelven a 1.0 cuando el buff termina.
*/
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float attackCooldown = 0.5f;

    // Multiplicadores temporales (1.0 = sin cambio).
    private float speedMultiplier = 1f;
    private float damageMultiplier = 1f;
    private float attackSpeedMultiplier = 1f;

    // Velocidad de movimiento real (base × multiplicador de velocidad).
    public float MoveSpeed => moveSpeed * speedMultiplier;

    // Daño real del ataque (base × multiplicador de daño, redondeado).
    public int Damage => Mathf.RoundToInt(baseDamage * damageMultiplier);

    // Cooldown real del ataque (se reduce cuando attackSpeedMultiplier > 1).
    public float AttackCooldown => attackCooldown / attackSpeedMultiplier;

    // Aplica un multiplicador temporal a la velocidad de movimiento.
    public void ApplySpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    // Aplica un multiplicador temporal al daño.
    public void ApplyDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    // Aplica un multiplicador temporal a la velocidad de ataque.
    public void ApplyAttackSpeedMultiplier(float multiplier)
    {
        attackSpeedMultiplier = multiplier;
    }

    // Restaura el multiplicador de velocidad a su valor base.
    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f;
    }

    // Restaura el multiplicador de daño a su valor base.
    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1f;
    }

    // Restaura el multiplicador de velocidad de ataque a su valor base.
    public void ResetAttackSpeedMultiplier()
    {
        attackSpeedMultiplier = 1f;
    }
}
