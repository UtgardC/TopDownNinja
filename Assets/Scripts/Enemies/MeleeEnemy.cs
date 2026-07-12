using UnityEngine;

// Hito 5 — Primer enemigo

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject "MeleeEnemy" con este script.

Componentes necesarios:
- Health en el mismo GameObject (maxHealth configurable en Inspector).
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0, Freeze Rotation Z = true.
- Collider2D para el cuerpo del enemigo.

Referencias del Inspector (herencia de EnemyBase):
- health: arrastrar el componente Health del mismo GameObject.
- moveSpeed: velocidad de movimiento (recomendado: 2.0).
- target: arrastrar el Transform del jugador.

Referencias del Inspector (propias):
- attackDamage: daño que inflige al jugador al contacto.
- attackRange: distancia mínima al jugador para atacar.
- attackCooldown: tiempo entre ataques sucesivos.
- playerLayer: capa del jugador (para detectar colisiones de ataque).

Layers y Tags:
- Asignar Layer "Enemy" a este GameObject.
- El jugador debe tener Layer "Player"; asignarla en el campo playerLayer.

Notas:
- IA: persigue al jugador y lo daña cuando está a attackRange de distancia.
- Demuestra herencia (extiende EnemyBase) y polimorfismo (sobreescribe TickBehavior).
*/
public class MeleeEnemy : EnemyBase
{
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private LayerMask playerLayer;

    private float attackCooldownTimer = 0f;

    protected override void Update()
    {
        base.Update();

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    // IA: si está cerca del jugador, ataca; si no, lo persigue.
    protected override void TickBehavior()
    {
        float distance = GetDistanceToTarget();

        if (!IsTargetDetected())
        {
            StopMovement();
            return;
        }

        if (distance <= attackRange)
        {
            StopMovement();
            TryAttack();
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    // Intenta atacar al jugador si el cooldown lo permite. Devuelve verdadero si atacó.
    private bool TryAttack()
    {
        if (attackCooldownTimer > 0f) return false;

        int damage = CalculateAttackDamage();
        ApplyMeleeAttack(damage);

        attackCooldownTimer = attackCooldown;
        NotifyAttackPerformed();
        return true;
    }

    // Calcula el daño del ataque cuerpo a cuerpo.
    private int CalculateAttackDamage()
    {
        return attackDamage;
    }

    // Aplica daño a todos los IDamageable del jugador dentro del rango.
    private void ApplyMeleeAttack(int damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable target = hit.GetComponentInParent<IDamageable>();
            if (target != null && target.IsAlive())
            {
                target.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
