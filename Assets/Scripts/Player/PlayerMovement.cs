using UnityEngine;
using UnityEngine.InputSystem;

// Hito 3 — Movimiento y combate del jugador

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador.

Componentes necesarios:
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0, Freeze Rotation Z = true.
- PlayerInput: Actions = InputSystem_Actions asset, Behavior = Send Messages.
- PlayerStats en el mismo GameObject.

Referencias del Inspector:
- stats: arrastrar el componente PlayerStats del mismo GameObject.

Layers y Tags:
- Ninguno requerido por este script.

Animación e Input:
- El componente PlayerInput llama automáticamente a OnMove cuando el jugador
  presiona las teclas de movimiento (WASD o flechas).
- Para animaciones: leer GetFacingDirection() desde el controlador de animación.

Notas:
- El movimiento usa Rigidbody2D.linearVelocity para que funcione con la física 2D.
*/
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Recibe el input de movimiento desde el sistema de input (PlayerInput → Send Messages).
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Move(moveInput);
    }

    // Aplica el movimiento al Rigidbody2D usando la velocidad de las estadísticas.
    private void Move(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * stats.MoveSpeed;
    }

    // Devuelve la dirección en la que mira el jugador según su último movimiento.
    // Devuelve Vector2.down si el jugador está quieto (orientación por defecto).
    public Vector2 GetFacingDirection()
    {
        if (moveInput != Vector2.zero)
            return moveInput.normalized;

        return Vector2.down;
    }
}
