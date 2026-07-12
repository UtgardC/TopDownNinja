using System;
using System.Collections;
using UnityEngine;

// Hito 4 — Base de enemigos

/*
CONFIGURACIÓN EN UNITY

GameObject:
- NO añadir directamente. Usar MeleeEnemy, RangedEnemy o BossEnemy.

Componentes necesarios:
- Health en el mismo GameObject.
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0, Freeze Rotation Z = true.
- Collider2D apropiado para el tamaño del enemigo.

Referencias del Inspector:
- health: arrastrar el componente Health del mismo GameObject.
- moveSpeed: velocidad de desplazamiento del enemigo.
- target: arrastrar el Transform del jugador desde la escena.

Layers y Tags:
- Asignar la Layer "Enemy" al GameObject del enemigo.
- El jugador debe tener la Layer "Player" (para que MeleeEnemy detecte colisiones).

Notas:
- TickBehavior se llama cada Update. Las subclases lo sobreescriben para definir su IA.
- Al morir lanza OnEnemyDied para que ObjectiveTracker pueda escucharlo.
*/
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Health health;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected Transform target;
    [SerializeField] protected float detectionRange = 10f;
    [SerializeField] private float deathDisableDelay = 0.75f;

    protected Rigidbody2D rb;

    // Notifica cuando el enemigo muere.
    public event Action OnEnemyDied;
    public event Action OnAttackPerformed;
    public event Action OnSpecialPerformed;

    public Vector2 Velocity => rb != null ? rb.linearVelocity : Vector2.zero;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (health == null) health = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        TryFindPlayerTarget();

        if (health != null)
        {
            health.OnDied += HandleDeath;
        }
    }

    protected virtual void Update()
    {
        if (health != null && !health.IsAlive()) return;
        TickBehavior();
    }

    // Cada subclase define su comportamiento de IA aquí.
    protected abstract void TickBehavior();

    // Devuelve la distancia actual al objetivo (jugador).
    protected float GetDistanceToTarget()
    {
        if (target == null) return float.MaxValue;
        return Vector2.Distance(transform.position, target.position);
    }

    // Mueve al enemigo hacia el objetivo a la velocidad configurada.
    protected void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        SetVelocity(direction * moveSpeed);
    }

    // Detiene el movimiento del enemigo.
    protected void StopMovement()
    {
        SetVelocity(Vector2.zero);
    }

    protected bool IsTargetDetected()
    {
        return target != null && GetDistanceToTarget() <= detectionRange;
    }

    protected void SetVelocity(Vector2 velocity)
    {
        if (rb != null) rb.linearVelocity = velocity;
    }

    protected void NotifyAttackPerformed()
    {
        OnAttackPerformed?.Invoke();
    }

    protected void NotifySpecialPerformed()
    {
        OnSpecialPerformed?.Invoke();
    }

    // Maneja la muerte del enemigo: detiene el movimiento, lanza el evento y desactiva el objeto.
    private void HandleDeath()
    {
        StopMovement();

        foreach (Collider2D enemyCollider in GetComponentsInChildren<Collider2D>())
        {
            enemyCollider.enabled = false;
        }

        OnEnemyDied?.Invoke();
        StartCoroutine(DisableAfterDeath());
    }

    private IEnumerator DisableAfterDeath()
    {
        if (deathDisableDelay > 0f)
        {
            yield return new WaitForSeconds(deathDisableDelay);
        }

        gameObject.SetActive(false);
    }

    // Permite asignar el objetivo (jugador) desde código si no se arrastra desde el Inspector.
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void TryFindPlayerTarget()
    {
        if (target != null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) target = player.transform;
    }

    protected virtual void OnDestroy()
    {
        if (health != null)
        {
            health.OnDied -= HandleDeath;
        }
    }
}
