using UnityEngine;

// Hito 5 — Enemigo cuerpo a cuerpo (MeleeEnemy)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un objeto en la escena con el Sprite del enemigo.

Componentes necesarios:
- Todos los requeridos por EnemyBase (Health, Rigidbody2D, Collider2D).

Referencias del Inspector (adicionales a EnemyBase):
- attackDamage: cantidad de vida que resta al jugador al golpearlo.
- attackRange: distancia máxima a la que puede golpear.
- attackCooldown: tiempo de espera en segundos entre golpes.
- playerLayer: seleccionar la Layer del jugador ("Player").
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

    // Sobreescribe la IA de la clase base.
    protected override void TickBehavior()
    {
        float distance = GetDistanceToTarget();

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

    // Ejecuta el ataque si el cooldown está listo.
    private bool TryAttack()
    {
        if (attackCooldownTimer > 0f) return false;

        int damage = CalculateAttackDamage();
        ApplyMeleeAttack(damage);
        NotifyAttackPerformed();

        attackCooldownTimer = attackCooldown;
        return true;
    }

    // Calcula el daño del ataque del enemigo.
    private int CalculateAttackDamage()
    {
        return attackDamage;
    }

    // Lanza un círculo de overlap para detectar al jugador y dañarlo.
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

    // Dibuja el rango de ataque en la escena.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
