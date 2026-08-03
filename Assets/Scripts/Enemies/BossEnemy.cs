using System;
using UnityEngine;

// Hito 11 — Jefe final (BossEnemy)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un objeto de gran tamaño (escala recomendada x2 o x3) con el Sprite del Boss.

Componentes necesarios:
- Todos los requeridos por EnemyBase (Health, Rigidbody2D, Collider2D).

Referencias del Inspector (adicionales a EnemyBase):
- attackDamage: daño del golpe normal.
- attackRange: rango del golpe normal.
- attackCooldown: cadencia de golpes melee del jefe.
- chargeSpeed: velocidad punta al realizar la embestida.
- chargeCooldown: tiempo de espera entre embestidas.
- chargeDistance: distancia necesaria desde la que puede iniciar una embestida.
- playerLayer: seleccionar la Layer del jugador ("Player").
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

    // Evento para avisar al ObjectiveTracker que el jefe ha muerto.
    public event Action OnBossDefeated;

    protected override void Start()
    {
        base.Start();
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

    // Sobreescribe la IA: si el jugador está lejos echa una embestida, si no va a combate cuerpo a cuerpo.
    protected override void TickBehavior()
    {
        if (isCharging) return; // Si está embistiendo, no decide acciones normales.

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

    // Ataque melee básico circular.
    private bool TryMeleeAttack()
    {
        if (attackCooldownTimer > 0f) return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponentInParent<IDamageable>();
            if (damageable != null && damageable.IsAlive())
            {
                damageable.TakeDamage(attackDamage);
            }
        }

        attackCooldownTimer = attackCooldown;
        NotifyAttackPerformed();
        return true;
    }

    // Inicia la embestida: sale disparado hacia la posición del jugador.
    private void StartCharge()
    {
        if (target == null) return;

        isCharging = true;
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * chargeSpeed;

        // Detiene la embestida a la fuerza tras medio segundo.
        Invoke(nameof(EndCharge), 0.5f);
        chargeCooldownTimer = chargeCooldown;
        NotifySpecialPerformed();
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

    // Visualiza el rango melee (rojo) y el rango de embestida (magenta).
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
