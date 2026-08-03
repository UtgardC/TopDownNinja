using UnityEngine;
using UnityEngine.InputSystem;

// Hito 3 — Ataque cuerpo a cuerpo del jugador

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador ("Player").

Componentes necesarios:
- PlayerInput configurado con "Send Messages".
- PlayerStats en el mismo GameObject.
- PlayerMovement en el mismo GameObject.

Referencias del Inspector:
- stats: arrastrar el componente PlayerStats del mismo GameObject.
- movement: arrastrar el componente PlayerMovement del mismo GameObject.
- attackOrigin: arrastrar un GameObject hijo que actúe como centro visual del ataque
                (suele colocarse levemente adelante del ninja).
- attackRange: radio del círculo de impacto para el ataque cuerpo a cuerpo.
- enemyLayer: seleccionar la Layer correspondiente a los enemigos ("Enemy").
*/
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private LayerMask enemyLayer;

    private float attackCooldownTimer = 0f;
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    // Mensaje automático enviado por PlayerInput al presionar el botón de ataque.
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            TryAttack();
        }
    }

    // Intenta realizar un ataque. Devuelve true si el ataque fue ejecutado.
    public bool TryAttack()
    {
        if (attackCooldownTimer > 0f) return false;

        int damage = CalculateDamage();
        ApplyAttackInArea(damage);

        if (playerAnimator != null) playerAnimator.TriggerAttack();

        attackCooldownTimer = stats.AttackCooldown;
        return true;
    }

    // Computa el daño final del ataque aplicando modificadores si los hubiera.
    public int CalculateDamage()
    {
        return stats.Damage;
    }

    // Detecta enemigos en un círculo alrededor de attackOrigin y les aplica daño.
    private void ApplyAttackInArea(int damage)
    {
        Vector2 origin = attackOrigin != null ? (Vector2)attackOrigin.position : (Vector2)transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable target = hit.GetComponentInParent<IDamageable>();
            if (target != null && target.IsAlive())
            {
                target.TakeDamage(damage);
            }
        }
    }

    // Dibuja el rango del ataque en la escena de Unity para facilitar la calibración.
    private void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
    }
}
