using UnityEngine;

// Opcional — Conecta el Animator con el estado del jugador

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador junto a PlayerMovement, PlayerAttack y Health.

Componentes necesarios:
- Animator en el mismo GameObject con el NinjaAnimator asignado.

Referencias del Inspector:
- movement: arrastrar el componente PlayerMovement del jugador.
- health: arrastrar el componente Health del jugador.

Animación:
- El Animator Controller debe tener: Speed (Float), MoveX (Float), MoveY (Float),
  Attack (Trigger), Hit (Trigger) y Dead (Bool).
- Los eventos de PlayerAttack y Health disparan Attack, Hit y Dead automáticamente.

Notas:
- Este script es opcional. El juego funciona sin él; solo mejora el feedback visual.
*/
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private void Awake()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (playerAttack == null) playerAttack = GetComponent<PlayerAttack>();
        if (health == null) health = GetComponent<Health>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (playerAttack != null)
        {
            playerAttack.OnAttackPerformed += HandleAttack;
        }

        if (health != null)
        {
            health.OnDamaged += HandleDamaged;
            health.OnDied += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (playerAttack != null)
        {
            playerAttack.OnAttackPerformed -= HandleAttack;
        }

        if (health != null)
        {
            health.OnDamaged -= HandleDamaged;
            health.OnDied -= HandleDeath;
        }
    }

    private void Update()
    {
        if (movement == null || animator == null) return;

        Vector2 facing = movement.GetFacingDirection();
        animator.SetFloat(SpeedHash, movement.IsMoving ? movement.Velocity.magnitude : 0f);
        animator.SetFloat(MoveXHash, facing.x);
        animator.SetFloat(MoveYHash, facing.y);
    }

    // Llama a este método desde PlayerAttack cuando ejecuta un ataque exitoso.
    private void HandleAttack()
    {
        if (animator != null) animator.SetTrigger(AttackHash);
    }

    // Llama a este método desde Health cuando el jugador recibe daño.
    private void HandleDamaged(int amount)
    {
        if (animator != null) animator.SetTrigger(HitHash);
    }

    private void HandleDeath()
    {
        if (animator != null) animator.SetBool(DeadHash, true);
    }
}
