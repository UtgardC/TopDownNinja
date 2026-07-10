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
- Llamar TriggerAttack() desde PlayerAttack.TryAttack() cuando el ataque se ejecuta.
- Llamar TriggerHit() desde Health.TakeDamage() si se quiere feedback visual al recibir daño.

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
        animator = GetComponent<Animator>();
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
        // Actualiza Speed: 1 si el jugador se está moviendo, 0 si está quieto.
        if (movement != null)
        {
            float speed = movement.GetFacingDirection().magnitude;
            animator.SetFloat("Speed", speed);
        }
    }

    // Llama a este método desde PlayerAttack cuando ejecuta un ataque exitoso.
    public void TriggerAttack()
    {
        animator.SetTrigger("IsAttacking");
    }

    // Llama a este método desde Health cuando el jugador recibe daño.
    public void TriggerHit()
    {
        animator.SetTrigger("IsHit");
    }

    private void HandleDeath()
    {
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
