using UnityEngine;

// Puente genérico entre EnemyBase/Health y un Animator Controller.
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int SpecialHash = Animator.StringToHash("Special");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private Vector2 facingDirection = Vector2.down;

    private void Awake()
    {
        if (enemy == null) enemy = GetComponentInParent<EnemyBase>();
        if (health == null) health = GetComponentInParent<Health>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (enemy != null)
        {
            enemy.OnAttackPerformed += HandleAttack;
            enemy.OnSpecialPerformed += HandleSpecial;
        }

        if (health != null)
        {
            health.OnDamaged += HandleDamaged;
            health.OnDied += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (enemy != null)
        {
            enemy.OnAttackPerformed -= HandleAttack;
            enemy.OnSpecialPerformed -= HandleSpecial;
        }

        if (health != null)
        {
            health.OnDamaged -= HandleDamaged;
            health.OnDied -= HandleDeath;
        }
    }

    private void Update()
    {
        if (animator == null || enemy == null) return;

        Vector2 velocity = enemy.Velocity;
        if (velocity.sqrMagnitude > 0.001f) facingDirection = velocity.normalized;

        animator.SetFloat(SpeedHash, velocity.magnitude);
        animator.SetFloat(MoveXHash, facingDirection.x);
        animator.SetFloat(MoveYHash, facingDirection.y);
    }

    private void HandleAttack()
    {
        if (animator != null) animator.SetTrigger(AttackHash);
    }

    private void HandleSpecial()
    {
        if (animator != null) animator.SetTrigger(SpecialHash);
    }

    private void HandleDamaged(int amount)
    {
        if (animator != null) animator.SetTrigger(HitHash);
    }

    private void HandleDeath()
    {
        if (animator != null) animator.SetBool(DeadHash, true);
    }
}
