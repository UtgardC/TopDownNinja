using UnityEngine;

// Hito 7 — Segundo enemigo (ranged)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject "RangedEnemy" con este script.

Componentes necesarios:
- Health en el mismo GameObject.
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0, Freeze Rotation Z = true.
- Collider2D para el cuerpo del enemigo.

Referencias del Inspector (herencia de EnemyBase):
- health: arrastrar el componente Health del mismo GameObject.
- moveSpeed: velocidad de movimiento (recomendado: 1.5).
- target: arrastrar el Transform del jugador.

Referencias del Inspector (propias):
- projectilePrefab: arrastrar el prefab del Projectile.
- attackDamage: daño que inflige cada proyectil.
- shootRange: distancia máxima a la que dispara al jugador.
- stopRange: distancia mínima a la que se mantiene del jugador.
- shootCooldown: tiempo entre disparos.

Layers y Tags:
- Asignar Layer "Enemy" a este GameObject.

Notas:
- IA: mantiene distancia del jugador. Si está en rango, dispara; si está muy lejos, se acerca.
- Demuestra herencia (extiende EnemyBase) y polimorfismo (sobreescribe TickBehavior).
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

    // IA: se acerca si está muy lejos, se aleja si está demasiado cerca, dispara en rango.
    protected override void TickBehavior()
    {
        float distance = GetDistanceToTarget();

        if (!IsTargetDetected())
        {
            StopMovement();
            return;
        }

        if (distance > shootRange)
        {
            // Fuera de rango: se acerca al jugador.
            MoveTowardsTarget();
        }
        else if (distance < stopRange)
        {
            // Demasiado cerca: retrocede del jugador.
            MoveAwayFromTarget();
        }
        else
        {
            // En rango ideal: se detiene y dispara.
            StopMovement();
            TryShoot();
        }
    }

    // Mueve al enemigo en dirección opuesta al jugador.
    private void MoveAwayFromTarget()
    {
        if (target == null) return;

        Vector2 direction = ((Vector2)transform.position - (Vector2)target.position).normalized;
        SetVelocity(direction * moveSpeed);
    }

    // Intenta disparar un proyectil si el cooldown lo permite. Devuelve verdadero si disparó.
    private bool TryShoot()
    {
        if (shootCooldownTimer > 0f || projectilePrefab == null || target == null) return false;

        ShootProjectile();
        shootCooldownTimer = shootCooldown;
        return true;
    }

    // Instancia un proyectil y lo lanza hacia el jugador.
    private void ShootProjectile()
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;

        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetDamage(attackDamage);
            projectile.Launch(direction);
            NotifyAttackPerformed();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shootRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}
