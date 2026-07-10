using UnityEngine;
using UnityEngine.InputSystem;

// Hito 3 — Movimiento y combate del jugador

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador junto a PlayerMovement y PlayerStats.

Componentes necesarios:
- PlayerInput: Actions = InputSystem_Actions asset, Behavior = Send Messages.
- PlayerStats en el mismo GameObject.
- PlayerMovement en el mismo GameObject.

Referencias del Inspector:
- stats: arrastrar el componente PlayerStats del mismo GameObject.
- movement: arrastrar el componente PlayerMovement del mismo GameObject.
- attackOrigin: Transform vacío hijo del jugador desde donde sale el ataque.
- attackRange: radio del área de daño del ataque cuerpo a cuerpo (en unidades).
- enemyLayer: capa (Layer) asignada a los enemigos en Unity.

Layers y Tags:
- Crear una Layer llamada "Enemy" y asignarla a todos los GameObjects enemigos.
- Asignar esa Layer al campo enemyLayer de este script.

Animación e Input:
- El componente PlayerInput llama automáticamente a OnAttack cuando el jugador
  presiona el botón de ataque configurado en el InputSystem_Actions asset.

Notas:
- El ataque detecta todos los IDamageable dentro de un radio usando Physics2D.OverlapCircleAll.
- CalculateDamage() devuelve el daño final, demostrando métodos con retorno.
*/
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private LayerMask enemyLayer;

    private float attackCooldownTimer = 0f;

    private void Update()
    {
        // Reduce el timer de cooldown con el tiempo transcurrido.
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    // Recibe el input de ataque desde el sistema de input (PlayerInput → Send Messages).
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            TryAttack();
        }
    }

    // Intenta ejecutar un ataque. Devuelve verdadero si el ataque se realizó.
    public bool TryAttack()
    {
        if (attackCooldownTimer > 0f) return false;

        int damage = CalculateDamage();
        ApplyAttackInArea(damage);

        attackCooldownTimer = stats.AttackCooldown;
        return true;
    }

    // Calcula el daño final del ataque según las estadísticas actuales del jugador.
    public int CalculateDamage()
    {
        return stats.Damage;
    }

    // Aplica daño a todos los IDamageable dentro del radio de ataque.
    private void ApplyAttackInArea(int damage)
    {
        Vector2 origin = attackOrigin != null ? (Vector2)attackOrigin.position : (Vector2)transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable target = hit.GetComponent<IDamageable>();
            if (target != null && target.IsAlive())
            {
                target.TakeDamage(damage);
            }
        }
    }

    // Dibuja el rango de ataque en el Editor para facilitar su configuración.
    private void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
    }
}
