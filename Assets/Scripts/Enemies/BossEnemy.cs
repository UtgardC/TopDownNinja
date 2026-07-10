using System;
using UnityEngine;

// Hito 11 — Jefe final

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject "Boss" con este script.

Componentes necesarios:
- Health en el mismo GameObject (recomendado: maxHealth = 300 o más).
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0, Freeze Rotation Z = true.
- Collider2D para el cuerpo del jefe.

Referencias del Inspector (herencia de EnemyBase):
- health: arrastrar el componente Health del mismo GameObject.
- moveSpeed: velocidad de movimiento (recomendado: 1.5).
- target: arrastrar el Transform del jugador.

Referencias del Inspector (propias):
- attackDamage: daño del ataque cuerpo a cuerpo.
- attackRange: radio del ataque.
- attackCooldown: tiempo entre ataques básicos.
- chargeSpeed: velocidad de la embestida especial.
- chargeCooldown: tiempo entre embestidas.
- chargeDistance: distancia mínima al jugador para activar la embestida.
- playerLayer: Layer del jugador.

Layers y Tags:
- Asignar Layer "Enemy" a este GameObject.
- El jugador debe tener Layer "Player".

Notas:
- Comportamiento: persigue al jugador y ataca cuerpo a cuerpo.
  Cuando el jugador está lejos, ejecuta una embestida especial (carga rápida).
- OnBossDefeated es escuchado por ObjectiveTracker para registrar la victoria.
- Demuestra herencia (EnemyBase) y polimorfismo (TickBehavior con lógica propia).
*/
public class BossEnemy : EnemyBase
{
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float chargeSpeed = 8f;
    [SerializeField] private float chargeCooldown = 5f;
    [SerializeField] private float chargeDistance = 4f;
    [SerializeField] private LayerMask playerLayer;

    private float attackCooldownTimer = 0f;
    private float chargeCooldownTimer = 0f;
    private bool isCharging = false;

    // Notifica cuando el jefe es derrotado. Escuchado por ObjectiveTracker.
    public event Action OnBossDefeated;

    protected override void Start()
    {
        base.Start();
        // Suscribir la derrota del jefe al evento de muerte de Health.
        if (health != null)
        {
            health.OnDied += NotifyBossDefeated;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (attackCooldownTimer > 0f) attackCooldownTimer -= Time.deltaTime;
        if (chargeCooldownTimer > 0f) chargeCooldownTimer -= Time.deltaTime;
    }

    // IA: si está cerca, ataca cuerpo a cuerpo; si está lejos y puede, embiste.
    protected override void TickBehavior()
    {
        if (isCharging) return;

        float distance = GetDistanceToTarget();

        if (distance <= attackRange)
        {
            StopMovement();
            TryMeleeAttack();
        }
        else if (distance >= chargeDistance && chargeCooldownTimer <= 0f)
        {
            StartCharge();
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    // Intenta ejecutar el ataque cuerpo a cuerpo. Devuelve verdadero si atacó.
    private bool TryMeleeAttack()
    {
        if (attackCooldownTimer > 0f) return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && damageable.IsAlive())
            {
                damageable.TakeDamage(attackDamage);
            }
        }

        attackCooldownTimer = attackCooldown;
        return true;
    }

    // Inicia una embestida rápida hacia el jugador.
    private void StartCharge()
    {
        if (target == null) return;

        isCharging = true;
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * chargeSpeed;

        // La embestida dura medio segundo y luego el jefe vuelve al comportamiento normal.
        Invoke(nameof(EndCharge), 0.5f);
        chargeCooldownTimer = chargeCooldown;
    }

    private void EndCharge()
    {
        isCharging = false;
        StopMovement();
    }

    private void NotifyBossDefeated()
    {
        OnBossDefeated?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, chargeDistance);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (health != null)
        {
            health.OnDied -= NotifyBossDefeated;
        }
    }
}
