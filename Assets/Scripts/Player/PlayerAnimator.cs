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
    - Speed (Float): controla si reproduce Idle o Walk (0 = quieto, 1 = caminando).
    - MoveX (Float): componente horizontal de la dirección (para Blend Trees direccionales).
    - MoveY (Float): componente vertical de la dirección (para Blend Trees direccionales).
    - IsAttacking (Trigger): activa la animación de ataque.
    - IsHit (Trigger): activa la animación de golpe recibido.
    - IsDead (Bool): activa la animación de muerte.

Notas:
- Este script es opcional. El juego funciona sin él; solo mejora el feedback visual.
*/
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Health health;

    private Animator animator;

    private void Awake()
    {
        // Si hay dos Animators (raíz e hijo), usa el del hijo porque ahí están los clips.
        // GetComponentInChildren busca raíz primero, por eso recorremos manualmente.
        Animator[] animators = GetComponentsInChildren<Animator>(true);
        foreach (Animator a in animators)
        {
            if (a.gameObject != gameObject)
            {
                animator = a;
                break;
            }
        }

        // Fallback: si no hay Animator en ningún hijo, usar el del raíz.
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (health != null)
        {
            health.OnDied += HandleDeath;
            health.OnDamaged += HandleDamaged;
        }
    }

    private void Update()
    {
        if (movement == null || animator == null) return;

        // Speed: 1 si el jugador presiona teclas, 0 si está quieto.
        float speed = movement.MoveInput.magnitude;
        animator.SetFloat("Speed", speed);

        // MoveX/MoveY: dirección hacia donde mira el jugador (persiste al soltar teclas).
        Vector2 facing = movement.GetFacingDirection();
        animator.SetFloat("MoveX", facing.x);
        animator.SetFloat("MoveY", facing.y);
    }

    public void TriggerAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("IsAttacking");
        }
    }

    public void TriggerHit()
    {
        if (animator != null)
        {
            animator.SetTrigger("IsHit");
        }
    }

    private void HandleDamaged(int amount)
    {
        TriggerHit();
    }

    private void HandleDeath()
    {
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDied -= HandleDeath;
            health.OnDamaged -= HandleDamaged;
        }
    }
}