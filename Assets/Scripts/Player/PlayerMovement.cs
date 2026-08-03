using UnityEngine;
using UnityEngine.InputSystem;

// Hito 3 — Movimiento del jugador

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador ("Player").

Componentes necesarios:
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0, Collision Detection = Continuous,
               Constraints -> Freeze Rotation Z = true.
- PlayerInput: configurar para usar el Input System con "Send Messages"
               y asociar el asset InputSystem_Actions.
- PlayerStats en el mismo GameObject.

Referencias del Inspector:
- stats: arrastrar el componente PlayerStats del mismo GameObject.
*/
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastFacingDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Mensaje automático enviado por el PlayerInput al detectar movimiento.
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Move(moveInput);
    }

    // Aplica velocidad al Rigidbody2D basada en la entrada y la estadística de velocidad.
    private void Move(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * stats.MoveSpeed;
    }

    // Expone la entrada de movimiento sin procesar para el Animator.
    public Vector2 MoveInput => moveInput;

    // Devuelve la última dirección hacia la que el jugador apuntaba o se movía.
    // Al soltar las teclas, retorna la última dirección activa en lugar de Vector2.down.
    public Vector2 GetFacingDirection()
    {
        if (moveInput != Vector2.zero)
        {
            lastFacingDirection = moveInput.normalized;
            return lastFacingDirection;
        }

        return lastFacingDirection;
    }
}
