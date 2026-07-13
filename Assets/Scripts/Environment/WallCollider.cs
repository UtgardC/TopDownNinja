using UnityEngine;

// Script para paredes/obstáculos con sprite individual.
// Ponélo en cualquier GameObject que tenga un SpriteRenderer y deba bloquear el paso.

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir a cada sprite de pared u obstáculo del nivel.

Componentes necesarios:
- SpriteRenderer en el mismo GameObject (ya existe si importaste el sprite).
- BoxCollider2D (o PolygonCollider2D) se agrega manualmente al mismo GameObject.

Cómo agregar el collider:
1. Seleccioná el GameObject de la pared en la jerarquía.
2. Inspector → Add Component → Box Collider 2D.
3. Unity ajusta automáticamente el tamaño al sprite. Si no, hacé clic en "Edit Collider".

Layers y Tags:
- Ningún tag especial necesario.
- Layer: podés dejar Default si es una pared estática.

Notas:
- Este script no hace nada por código. La colisión la maneja el BoxCollider2D solo.
- Si el jugador igual atraviesa la pared, el problema está en el Rigidbody2D del jugador
  (ver comentario al final del script).
*/
public class WallCollider : MonoBehaviour
{
    // Este script intencionalmente no tiene lógica.
    // La colisión la resuelve Unity de forma automática cuando:
    //   - Este GameObject tiene un Collider2D (no Trigger).
    //   - El jugador tiene un Rigidbody2D y un Collider2D.
    //
    // Si el jugador sigue atravesando las paredes, verificá en el Rigidbody2D del jugador:
    //   Collision Detection = Continuous   (no Discrete)
    //
    // Si el problema persiste, el Rigidbody2D del jugador puede tener:
    //   Body Type = Kinematic   (lo que deshabilita la física de colisión normal)
    //   → Cambiarlo a Dynamic.
}
