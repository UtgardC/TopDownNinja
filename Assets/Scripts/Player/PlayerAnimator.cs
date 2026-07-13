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
- El Animator Controller (NinjaAnimator) debe tener estos parámetros:
    - Speed (Float): controla si reproduce Idle o Walk.
    - IsAttacking (Trigger): activa la animación de ataque.
    - IsHit (Trigger): activa la animación de golpe recibido.
    - IsDead (Bool): activa la animación de muerte.
*/
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Health health;

    private Animator animator;
    private bool hasSpeedParam;
    private bool hasAttackParam;
    private bool hasHitParam;
    private bool hasDeadParam;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("PlayerAnimator: no se encontró un Animator en el GameObject. Las animaciones están desactivadas.");
            return;
        }

        // Verifica la existencia de cada parámetro en el Animator de Unity para evitar errores silenciosos
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == "Speed") hasSpeedParam = true;
            if (param.name == "IsAttacking") hasAttackParam = true;
            if (param.name == "IsHit") hasHitParam = true;
            if (param.name == "IsDead") hasDeadParam = true;
        }

        if (!hasSpeedParam) Debug.LogError("PlayerAnimator: FALTAPARÁMETRO. El Animator del jugador no tiene un parámetro Float llamado 'Speed'.");
        if (!hasAttackParam) Debug.LogWarning("PlayerAnimator: AVISO. El Animator del jugador no tiene un parámetro Trigger llamado 'IsAttacking'.");
        if (!hasHitParam) Debug.LogWarning("PlayerAnimator: AVISO. El Animator del jugador no tiene un parámetro Trigger llamado 'IsHit'.");
        if (!hasDeadParam) Debug.LogWarning("PlayerAnimator: AVISO. El Animator del jugador no tiene un parámetro Bool llamado 'IsDead'.");
    }

    private void Start()
    {
        // Al morir, activa el estado de muerte en el Animator.
        if (health != null)
        {
            health.OnDied += HandleDeath;
        }
    }

    private void Update()
    {
        if (animator == null || movement == null) return;

        // Regresado a la lógica original basada en GetFacingDirection
        float speed = movement.GetFacingDirection().magnitude;
        animator.SetFloat("Speed", speed);
    }

    // Llama a este método desde PlayerAttack cuando ejecuta un ataque exitoso.
    public void TriggerAttack()
    {
        if (animator == null || !hasAttackParam) return;
        animator.SetTrigger("IsAttacking");
    }

    // Llama a este método desde Health cuando el jugador recibe daño.
    public void TriggerHit()
    {
        if (animator == null || !hasHitParam) return;
        animator.SetTrigger("IsHit");
    }

    private void HandleDeath()
    {
        if (animator == null || !hasDeadParam) return;
        animator.SetBool("IsDead", true);
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDied -= HandleDeath;
        }
    }
}
