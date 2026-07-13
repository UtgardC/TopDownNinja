using UnityEngine;

// Hito 7 — Enemigo a distancia (RangedEnemy)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un objeto en la escena con el Sprite del enemigo a distancia.

Componentes necesarios:
- Todos los requeridos por EnemyBase (Health, Rigidbody2D, Collider2D).

Referencias del Inspector (adicionales a EnemyBase):
- projectilePrefab: arrastrar el Prefab del proyectil (con script Projectile).
- attackDamage: daño que tendrá el proyectil al crearse.
- shootRange: rango máximo desde el que empezará a disparar.
- stopRange: distancia mínima para mantener su posición (si el jugador se acerca más, retrocederá).
- shootCooldown: cadencia de tiro en segundos.
*/
public class RangedEnemy : EnemyBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int attackDamage = 8;
    [SerializeField] private float shootRange = 5f;
    [SerializeField] private float stopRange = 2.5f;
    [SerializeField] private float shootCooldown = 2f;

    private float shootCooldownTimer = 0f;

    protected override void Update()
    {
        base.Update();

        if (shootCooldownTimer > 0f)
        {
            shootCooldownTimer -= Time.deltaTime;
        }
    }

    // Sobreescribe la IA para mantener la distancia ideal y disparar.
    protected override void TickBehavior()
    {
        float distance = GetDistanceToTarget();

        if (distance > shootRange)
        {
            // Demasiado lejos: avanzar hacia el jugador.
            MoveTowardsTarget();
        }
        else if (distance < stopRange)
        {
            // Demasiado cerca: retroceder en dirección opuesta.
            MoveAwayFromTarget();
        }
        else
        {
            // Distancia ideal: quedarse quieto y disparar.
            StopMovement();
            TryShoot();
        }
    }

    // Empuja al enemigo en dirección contraria al jugador para evadir el combate melee.
    private void MoveAwayFromTarget()
    {
        if (target == null) return;

        Vector2 direction = ((Vector2)transform.position - (Vector2)target.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    // Dispara si el cooldown está listo.
    private bool TryShoot()
    {
        if (shootCooldownTimer > 0f || projectilePrefab == null || target == null) return false;

        ShootProjectile();
        NotifyAttackPerformed();
        shootCooldownTimer = shootCooldown;
        return true;
    }

    // Instancia el proyectil y lo lanza en dirección al jugador.
    private void ShootProjectile()
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;

        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetDamage(attackDamage);
            projectile.Launch(direction);
        }
    }

    // Dibuja los rangos de tiro y evasión en la escena.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shootRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}
